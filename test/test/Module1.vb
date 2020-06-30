Imports MedatechUK.oData

Module Module1

    Sub Main()

        Dim l As New Loading("TST", AddressOf ologHandler)
        With l
            With .AddRow(1)
                .TEXT1 = "a"

            End With
            With .AddRow(2)
                .TEXT1 = "b"

            End With
            With .AddRow(2)
                .TEXT1 = "c"

            End With

            Dim ex As Exception = .Post()
            If Not ex Is Nothing Then
                Console.WriteLine(ex.Message)

            End If

        End With

    End Sub

    Sub ologHandler(sender As Object, e As LogArgs)
        Console.WriteLine(e.Message)

    End Sub

End Module
