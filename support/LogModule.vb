Namespace oData

    Module LogModule

        Public Event LogEvent(sender As Object, e As LogArgs)

        Public Sub Log(ByRef Sender As Object, str As String, ParamArray args() As String)
            RaiseEvent LogEvent(Sender, New LogArgs(str, args))

        End Sub

        Public Sub Log(str As String, ParamArray args() As String)
            RaiseEvent LogEvent(Nothing, New LogArgs(str, args))

        End Sub

        Public Sub Log(ByRef Sender As Object, ex As Exception)
            RaiseEvent LogEvent(Sender, New LogArgs(ex))

        End Sub

        Public Sub Log(ex As Exception)
            RaiseEvent LogEvent(Nothing, New LogArgs(ex))

        End Sub

    End Module

End Namespace
