Imports System.Reflection

Namespace oData

    ''' <summary>
    '''
    ''' This code was generated by the Schema Utility.
    '''
    ''' Form: ZODA_TRANS as List(Of rowZODA_TRANS)
    ''' Date: 26/05/2020
    '''
    ''' </summary>
    <oFormClass("rowZODA_TRANS", "ZODA_LOAD")>
    Public Class ZODA_TRANS : Inherits oForm

        ''' <summary>
        ''' ZODA_TRANS Constructor method.
        ''' </summary>
        ''' <param name="Sender">The Assembly.GetExecutingAssembly of your project</param>
        ''' <param name="Parent">Optional: Parent oRow of this form</param>
        Sub New(ByRef Sender As Assembly, Optional Parent As oRow = Nothing)
            MyBase.New(Sender, Parent)

        End Sub

        ''' <summary>
        ''' Add a new row to the ZODA_TRANS form.
        ''' </summary>
        ''' <returns>rowZODA_TRANS</returns>
        Public Function AddRow() As rowZODA_TRANS
            With Me
                .Add(New rowZODA_TRANS(Me))
                Return .Last

            End With

        End Function

    End Class

    ''' <summary>
    ''' Defines rows in the ZODA_TRANS form.
    ''' </summary>
    <oRowClass("ZODA_TRANS", "serialZODA_TRANS")>
    Public Class rowZODA_TRANS : Inherits oRow

        ''' <summary>
        ''' rowZODA_TRANS Constructor method.
        ''' </summary>
        ''' <param name="Parent">Parent oForm of this row</param>
        Sub New(Parent As oForm)
            MyBase.New(Parent)

        End Sub

        ''' <summary>
        ''' Get/set the ZODA_LOAD subform.
        ''' </summary>
        ''' <returns>ZODA_LOAD</returns>
        Public Property ZODA_LOAD As ZODA_LOAD
            Get
                Return MyBase.SubForms("ZODA_LOAD")
            End Get
            Set(value As ZODA_LOAD)
                MyBase.SubForms("ZODA_LOAD") = value
            End Set
        End Property

        ''' <summary>
        ''' Get / Set the "Type" Value of ZODA_TRANS.
        ''' </summary>
        ''' <returns>String</returns>	
        <oDataColumn("Type", length:=3)>
        Public Property TYPENAME As String
            Get
                Return getProperty()
            End Get
            Set(value As String)
                setProperty(value)
            End Set
        End Property

        ''' <summary>
        ''' Get / Set the "Reference" Value of ZODA_TRANS.
        ''' </summary>
        ''' <returns>String</returns>	
        <oDataColumn("Reference", Key:=True, length:=50)>
        Public Property BUBBLEID As String
            Get
                Return getProperty()
            End Get
            Set(value As String)
                setProperty(value)
            End Set
        End Property

        ''' <summary>
        ''' Get the Read Only "Create Date" Value of ZODA_TRANS.
        ''' </summary>
        ''' <returns>Date</returns>	
        <oDataColumn("Create Date", ReadOnly:=True)>
        Public ReadOnly Property CREATEDATE As Date
            Get
                Return getProperty()
            End Get

        End Property

        ''' <summary>
        ''' Get / Set the "Finished sending?" Value of ZODA_TRANS.
        ''' </summary>
        ''' <returns>String</returns>	
        <oDataColumn("Finished sending?", length:=1)>
        Public Property COMPLETE As String
            Get
                Return getProperty()
            End Get
            Set(value As String)
                setProperty(value)
            End Set
        End Property

        ''' <summary>
        ''' Get the Read Only "Complete Date" Value of ZODA_TRANS.
        ''' </summary>
        ''' <returns>Date</returns>	
        <oDataColumn("Complete Date", ReadOnly:=True)>
        Public ReadOnly Property COMPLETEDATE As Date
            Get
                Return getProperty()
            End Get

        End Property

        ''' <summary>
        ''' Get / Set the "Loaded?" Value of ZODA_TRANS.
        ''' </summary>
        ''' <returns>String</returns>	
        <oDataColumn("Loaded?", length:=1)>
        Public Property LOADED As String
            Get
                Return getProperty()
            End Get
            Set(value As String)
                setProperty(value)
            End Set
        End Property

        ''' <summary>
        ''' Get the Read Only "Load Date" Value of ZODA_TRANS.
        ''' </summary>
        ''' <returns>Date</returns>	
        <oDataColumn("Load Date", ReadOnly:=True)>
        Public ReadOnly Property LOADDATE As Date
            Get
                Return getProperty()
            End Get

        End Property

        ''' <summary>
        ''' Get / Set the "Ln" Value of ZODA_TRANS.
        ''' </summary>
        ''' <returns>Integer</returns>	
        <oDataColumn("Ln")>
        Public Property LINE As Integer
            Get
                Return getProperty()
            End Get
            Set(value As Integer)
                setProperty(value)
            End Set
        End Property

        ''' <summary>
        ''' Get / Set the "Type (ID)" Value of ZODA_TRANS.
        ''' </summary>
        ''' <returns>Integer</returns>	
        <oDataColumn("Type (ID)", Key:=True)>
        Public Property LOADTYPE As Integer
            Get
                Return getProperty()
            End Get
            Set(value As Integer)
                setProperty(value)
            End Set
        End Property


    End Class

    ''' <summary>
    ''' A nullable version of rowZODA_TRANS.
    '''
    ''' This is used to deserialise JSON data
    ''' and should not be instantiated directly.
    ''' </summary>
    Public Class serialZODA_TRANS

        Public Property TYPENAME As String
        Public Property BUBBLEID As String
        Public Property CREATEDATE As Date?
        Public Property COMPLETE As String
        Public Property COMPLETEDATE As Date?
        Public Property LOADED As String
        Public Property LOADDATE As Date?
        Public Property LINE As Integer?
        Public Property LOADTYPE As Integer?


    End Class

End Namespace