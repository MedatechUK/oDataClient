Imports System.IO
Imports System.Reflection
Imports Newtonsoft.Json

Namespace oData

#Region "Attributes"

    <System.AttributeUsage(System.AttributeTargets.Class)>
    Public Class oFormClass
        Inherits System.Attribute

        Public rowType As Type = Nothing
        Public subForms As New List(Of String)

        Private rName As String

        Sub New(ByVal RowName As String, ParamArray KeyColumn() As String)

            rName = RowName
            For Each K As String In KeyColumn
                subForms.Add(K)

            Next

        End Sub

        Sub Load(ByRef Sender As Assembly)
            For Each ay In Sender.ExportedTypes
                If String.Compare(ay.Name, rName) = 0 Then
                    rowType = ay
                    Exit For
                End If

            Next

        End Sub

    End Class

#End Region

    ''' <summary>
    ''' Defines a Priorty Form for oData.
    ''' </summary>
    Public MustInherit Class oForm : Inherits List(Of oRow) : Implements IDisposable

        Public Keys As New List(Of String)
        Private _Parent As oRow = Nothing
        Private _Sender As Assembly

        ''' <summary>
        ''' The parent row of this form.
        ''' </summary>
        ''' <returns>oRow</returns>
        Public ReadOnly Property Parent As oRow
            Get
                Return _Parent
            End Get
        End Property

        ''' <summary>
        ''' The calling assembly.
        ''' </summary>
        ''' <returns>Assembly</returns>
        Public ReadOnly Property Sender As Assembly
            Get
                Return _Sender
            End Get
        End Property

        ''' <summary>
        ''' The name of the form.
        ''' </summary>
        ''' <returns>String</returns>
        Public ReadOnly Property Name As String
            Get
                Return Me.GetType().Name
            End Get
        End Property

        ''' <summary>
        ''' Contains the last exception from posting this row.
        ''' </summary>
        ''' <returns>oException</returns>
        Public Property Exception As Exception

        ''' <summary>
        ''' Form Constructor method.
        ''' </summary>
        ''' <param name="Parent">oRow</param>
        Public Sub New(ByRef Sender As Assembly, Optional Parent As oRow = Nothing)

            _Parent = Parent
            _Sender = Sender

            Dim r As oFormClass = Me.GetType().GetCustomAttribute(Of oFormClass)
            If Not r Is Nothing Then
                r.Load(Sender)
                If r.rowType Is Nothing Then
                    Throw New Exception(
                    String.Format(
                        "Type {0} not found in {1}.",
                        Name,
                        Sender.FullName
                    )
                )
                End If

                For Each I In r.rowType.GetProperties()
                    Dim c As oDataColumn = I.GetCustomAttribute(Of oDataColumn)
                    If Not c Is Nothing Then
                        If c.key Then Me.Keys.Add(I.Name)
                    End If
                Next
            Else
                Throw New Exception(
                    String.Format(
                        "Missing oFormClass Attribute on class {0}",
                        Me.Name
                    )
                )
            End If

        End Sub

#Region "Post"

        Public ReadOnly Property RequestStream As MemoryStream
            Get
                Using t As New StringWriter()
                    Using objx As New JTextWriter(t)
                        For Each row As oRow In Me
                            With objx
                                .WriteStartObject()
                                For Each ch As String In row.changes.Keys
                                    If Not row.changes(ch).ReadOnly Then
                                        .WriteAttributeObject(ch, row.getProperty(ch))

                                    End If
                                Next
                                For Each sf As oForm In row.SubForms.Values
                                    .WriteAttributeObject(String.Format("{0}_SUBFORM", sf.Name), sf.RequestStream)

                                Next
                                .WriteEndObject()
                                If Not row Is Me.Last Then
                                    .WriteRaw(" , ")

                                End If
                            End With

                        Next

                        Dim myEncoder As New System.Text.ASCIIEncoding
                        Return New MemoryStream(myEncoder.GetBytes(t.ToString))

                    End Using

                End Using

            End Get
        End Property

        ''' <summary>
        ''' Post the form oData to the Priority server.
        ''' </summary>
        Public Function Post(Optional Verb As String = "POST") As Boolean

            System.Net.ServicePointManager.ServerCertificateValidationCallback =
          Function(se As Object,
          cert As System.Security.Cryptography.X509Certificates.X509Certificate,
          chain As System.Security.Cryptography.X509Certificates.X509Chain,
          sslerror As System.Net.Security.SslPolicyErrors) True

            Dim e As Object
            Using client As New oClient("/" & Me(0).FormType.Name, Verb)
                Log(Me, "{0}", New StreamReader(RequestStream).ReadToEnd)
                RequestStream.Position = 0
                e = client.GetResponse(RequestStream)

            End Using

            Select Case TypeOf (e) Is Exception
                Case False
                    Dim o As Object
                    Using reader As New StreamReader(
                        TryCast(e, Net.WebResponse).GetResponseStream
                    )
                        o = JsonConvert.DeserializeObject(
                            reader.ReadToEnd(),
                            Me(0)._serialType
                        )

                    End Using

                    For Each i In o.GetType.GetProperties
                        If Not i.GetValue(o) Is Nothing Then
                            If Keys.Contains(i.Name) Then
                                Me(0)(i.Name) = i.GetValue(o)

                            End If

                        End If

                    Next

                    Me(0).changes.Clear()
                    Exception = Nothing
                    Log("{0}", "OK.")

                    Return True

                Case Else
                    Exception = e
                    Log(Me, e)

                    Return False

            End Select

        End Function

#End Region

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
