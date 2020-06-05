Imports MedatechUK.oData

Module Module1

    Sub Main()

        Dim l As New Loading("TST", AddressOf ologHandler)
        With l
            With .AddRow(1)
                .TEXT1 = "a"

            End With
            With .AddRow(1)
                .TEXT1 = "a"

            End With
            Dim er As List(Of Exception) = .Post()
            Beep()
        End With

    End Sub

    Sub ologHandler(sender As Object, e As LogArgs)
        Console.WriteLine(e.Message)

    End Sub

End Module
