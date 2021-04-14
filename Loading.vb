Imports System.Web
Imports MedatechUK.Logging

Namespace oData

    Public Class Loading
        Inherits Logable
        Implements IDisposable

        Private o As ZODA_TRANS
        Private otrans As rowZODA_TRANS

        Private _StrType As String
        Private bubbleid As String

        ''' <summary>
        ''' oData Loading Constructor
        ''' </summary>
        ''' <param name="strtype">Transaction Type</param>
        Sub New(strtype As String, logEventHandler As EventHandler)

            logHandlerDelegate = logEventHandler

            If HttpContext.Current Is Nothing Then
                bubbleid = System.Guid.NewGuid.ToString
            Else
                If HttpContext.Current.Items("bubbleid") Is Nothing Then
                    bubbleid = System.Guid.NewGuid.ToString
                Else
                    ' Use the bubbleid from the httpcontext if we're a web server
                    bubbleid = HttpContext.Current.Items("bubbleid")
                End If
            End If

            _StrType = strtype

            o = New ZODA_TRANS(Reflection.Assembly.GetExecutingAssembly)
            otrans = o.AddRow()
            With otrans
                .TYPENAME = _StrType
                .BUBBLEID = bubbleid

            End With

        End Sub

        ''' <summary>
        ''' Add a row to the loading.
        ''' </summary>
        ''' <param name="Recordtype">Integer record type for load row</param>
        ''' <returns>the created row</returns>
        Public Function AddRow(Recordtype As Integer) As rowZODA_LOAD

            Dim ret As rowZODA_LOAD = otrans.ZODA_LOAD.AddRow
            With ret
                .RECORDTYPE = Recordtype.ToString
            End With

            Return ret

        End Function

        ''' <summary>
        ''' Post the data to the server specified in the odata.config
        ''' </summary>
        ''' <returns>List(of exception)</returns>
        Public Function Post(Optional Environment As String = Nothing) As Exception

            If Not o.Post(Environment) Then Return o.Exception
            With TryCast(o(0), rowZODA_TRANS)
                .COMPLETE = "Y"
                If Not .Patch(Environment) Then Return .Exception
            End With

            Return Nothing

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