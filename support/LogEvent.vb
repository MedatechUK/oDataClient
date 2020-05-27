Namespace oData

    Public Class LogArgs : Inherits EventArgs

        Public isException As Boolean = False
        Private _Message As String
        Public Property Message As String
            Get
                Return _Message
            End Get
            Set(value As String)
                _Message = value
            End Set
        End Property

        Sub New(format As String, ParamArray Args() As String)
            _Message = String.Format(format, Args)

        End Sub

        Sub New(ex As Exception)
            isException = True
            _Message = String.Format("{0}{2}{1}", ex.Message, ex.StackTrace, vbCrLf)

        End Sub

    End Class

End Namespace