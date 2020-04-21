Imports System.IO

Module logging

#Region "Logging"

    Private Function LogFolder() As DirectoryInfo
        Return New DirectoryInfo(
            IO.Path.Combine(
               Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly.Location),
                String.Format(
                    "log\{0}",
                    Now.ToString("yyyy-MM")
                )
            )
        )

    End Function

    Private Function currentlog() As FileInfo
        With LogFolder()
            If Not .Exists Then .Create()
            Return New FileInfo(
                IO.Path.Combine(
                    .FullName,
                    String.Format(
                        "{0}.txt",
                        Now.ToString("yyMMdd")
                    )
                )
            )

        End With

    End Function

    Public Sub Log(ByVal str, ByVal ParamArray args())
        Console.WriteLine(str, args)
        Using log As New StreamWriter(currentlog.FullName, True)
            log.WriteLine("{0}> {1}", Format(Now, "HH:mm:ss"), String.Format(str, args))
        End Using

    End Sub

#End Region

End Module
