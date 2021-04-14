Imports System.IO
Imports System.Net
Imports System.Xml.Serialization
Imports System.Web
Imports System.Web.Configuration
Imports Newtonsoft.Json
Imports MedatechUK.Logging

Namespace oData

    ''' <summary>
    ''' An oData client
    ''' </summary>
    Public Class oClient
        Inherits Logable
        Implements IDisposable

        ''' <summary>
        ''' This method load the approproiate configuration
        ''' depending if there is an http context.
        ''' If there is no context settings come from odata.config 
        ''' in the executing path.
        ''' Otherwise the settings come from the web.config file
        ''' of the website.
        ''' </summary>
        ''' <returns></returns>
        Private ReadOnly Property settings As odataConfig
            Get
                Static ret As odataConfig = Nothing
                If ret Is Nothing Then
                    If HttpContext.Current Is Nothing Then ' Use config file
                        Log("Detecting Stream Context ... Stream is FILE.")
                        Dim cFile As FileInfo
                        Select Case New DirectoryInfo(Environment.CurrentDirectory).FullName
                            Case New DirectoryInfo(Environment.SystemDirectory).FullName
                                cFile = New FileInfo(
                                    Path.Combine(
                                         New FileInfo(Reflection.Assembly.GetEntryAssembly.Location).Directory.FullName,
                                        "odata.config"
                                    )
                                )
                            Case Else
                                cFile = New FileInfo(
                                    Path.Combine(
                                         New DirectoryInfo(Environment.CurrentDirectory).FullName,
                                        "odata.config"
                                    )
                                )

                        End Select
                        Log("Loading oData settings from [{0}] ...", cFile.FullName)

                        If cFile.Exists Then
                            Try
                                Dim s As New XmlSerializer(GetType(odataConfig))
                                Using sr As New StreamReader(cFile.FullName)
                                    ret = s.Deserialize(sr)
                                End Using

                            Catch ex As Exception
                                Throw New Exception(
                                    String.Format(
                                        "Error loading odata.config {0}. {1}",
                                        cFile.FullName,
                                        ex.Message
                                    )
                                )

                            End Try

                        Else
                            Throw New Exception(
                                String.Format(
                                    "Missing odata.config in {0}.",
                                    cFile.DirectoryName
                                )
                            )

                        End If

                    Else ' Use web config
                        Log("Detecting Stream Context ... Stream is HTTP.")
                        Log("Loading oData settings from [{0}] ... ", HttpContext.Current.Server.MapPath("/"))
                        Dim c As New odataConfig
                        With c
                            .oDataHost = WebConfigurationManager.AppSettings("oDataHost")
                            .tabulaini = WebConfigurationManager.AppSettings("tabulaini")
                            .environment = HttpContext.Current.Request("environment")
                            .ouser = WebConfigurationManager.AppSettings("ouser")
                            .opass = WebConfigurationManager.AppSettings("opass")

                        End With

                        ret = c

                    End If

                End If
                Return ret

            End Get

        End Property

        Private _Request As Net.HttpWebRequest

        ''' <summary>
        ''' oClient constructor method.
        ''' </summary>
        ''' <param name="Path">String</param>
        ''' <param name="Method">String</param>
        Sub New(Path As String, Environment As String, Optional Method As String = "POST", Optional Query As String = "")

            Try
                Dim config As odataConfig = settings
                Dim uri As New UriBuilder
                If Environment Is Nothing Then Environment = config.environment

                With uri
                    .Scheme = Split(config.oDataHost, "://")(0)
                    .Host = Split(config.oDataHost, "://")(1)
                    .Path = String.Format(
                        "/odata/Priority/{0}/{1}{2}",
                        config.tabulaini,
                        Environment,
                        Path
                    )
                    .Query = Query

                    Log(Me, "{0} {1}", Method.ToUpper, uri.ToString)

                    _Request = CType(Net.HttpWebRequest.Create(.Uri), Net.HttpWebRequest)
                    With _Request
                        .Method = Method
                        .Proxy = Nothing
                        .UserAgent = "Medatech .net oData Client"
                        .ContentType = "application/json"
                        .Credentials = New NetworkCredential(
                            config.ouser,
                            config.opass
                        )

                    End With

                End With

            Catch ex As Exception
                Throw New Exception(
                    String.Format(
                        "{0}",
                        ex.Message
                    )
                )

            End Try

        End Sub

        ''' <summary>
        ''' Returns a WebResponse or an exception in 
        ''' response to the Requested MemoryStream.
        ''' </summary>
        ''' <param name="Request"></param>
        ''' <returns>Object</returns>
        Public Function GetResponse(Optional ByRef Request As MemoryStream = Nothing) As Object

            Dim e As Object
            Dim buffer(1024) As Byte
            Dim bytesRead As Integer

            System.Net.ServicePointManager.ServerCertificateValidationCallback =
          Function(se As Object,
          cert As System.Security.Cryptography.X509Certificates.X509Certificate,
          chain As System.Security.Cryptography.X509Certificates.X509Chain,
          sslerror As System.Net.Security.SslPolicyErrors) True

            With _Request
                Try
                    If Not Request Is Nothing Then
                        .ContentLength = Request.Length
                        Using requestStream As Stream = .GetRequestStream()
                            With requestStream
                                While True
                                    bytesRead = Request.Read(buffer, 0, buffer.Length)
                                    If bytesRead = 0 Then
                                        Exit While

                                    End If
                                    .Write(buffer, 0, bytesRead)

                                End While

                            End With

                        End Using

                    End If

                    e = .GetResponse()

                Catch ex As WebException
                    With ex
                        If TryCast(.Response, HttpWebResponse) Is Nothing Then
                            e = New Exception(ex.Status.ToString)

                        Else
                            Dim str As String
                            With .Response
                                Using reader As New StreamReader(.GetResponseStream)
                                    str = reader.ReadToEnd
                                End Using
                            End With

                            Try
                                Dim er As interfaceError = JsonConvert.DeserializeObject(str, GetType(interfaceError))
                                With TryCast(.Response, HttpWebResponse)
                                    e = New Exception(
                                        String.Format("({1}) {2}",
                                            CInt(.StatusCode).ToString,
                                            er.form.TypeName,
                                            er.form.Errs.text
                                        )
                                    )

                                End With

                            Catch xmlex As Exception
                                If Len(str) > 0 Then
                                    If _Request.Method.ToUpper = "GET" _
                                        And TryCast(.Response, HttpWebResponse).StatusCode = HttpStatusCode.InternalServerError Then
                                        With TryCast(.Response, HttpWebResponse)
                                            e = New oException(
                                                HttpStatusCode.NotFound,
                                                String.Format("[{0}] {1}",
                                                    CInt(HttpStatusCode.NotFound).ToString,
                                                    HttpStatusCode.NotFound.ToString
                                                )
                                            )
                                        End With

                                    Else

                                        Select Case TryCast(.Response, HttpWebResponse).StatusCode
                                            Case HttpStatusCode.NotFound
                                                With TryCast(.Response, HttpWebResponse)
                                                    e = New oException(
                                                    .StatusCode,
                                                    String.Format("[{0}] {1}",
                                                        CInt(.StatusCode).ToString,
                                                        .StatusDescription
                                                    )
                                                )
                                                End With

                                            Case Else
                                                With TryCast(.Response, HttpWebResponse)
                                                    e = New oException(
                                                    .StatusCode,
                                                    String.Format("[{0}] {1}",
                                                        CInt(.StatusCode).ToString,
                                                        .StatusDescription
                                                    )
                                                )
                                                End With

                                        End Select

                                    End If

                                Else
                                    With TryCast(.Response, HttpWebResponse)
                                        e = New oException(
                                        .StatusCode,
                                        String.Format("[{0}] {1}",
                                            CInt(.StatusCode).ToString,
                                            .StatusDescription
                                        )
                                    )
                                    End With

                                End If

                            End Try

                        End If

                    End With

                Catch ex As Exception
                    e = ex

                End Try

            End With

            Return e

        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            ' TODO: uncomment the following line if Finalize() is overridden above.
            ' GC.SuppressFinalize(Me)
        End Sub

#End Region

    End Class

End Namespace