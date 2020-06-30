Imports Newtonsoft.Json

Namespace oData

    Public Class interfaceError
        '{"FORM":{"@TYPE":"ZODA_TRANS","InterfaceErrors":{"@XmlFormat":"0","text":"Line 1- Type TST: The value 'TST' does not exist in the Type column of the z oData Transaction Types table."}}}

        <JsonProperty("FORM")>
        Public form As interr

    End Class

    Public Class interr

        <JsonProperty("@TYPE")>
        Public TypeName As String

        <JsonProperty("InterfaceErrors")>
        Public Errs As Err

    End Class

    Public Class Err

        <JsonProperty("@XmlFormat")>
        Public XmlFormat As Integer

        <JsonProperty("text")>
        Public text As String

    End Class

End Namespace
