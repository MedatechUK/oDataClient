
Option Strict Off
Option Explicit On

Imports System.Xml.Serialization

Namespace oData

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432"),
 System.SerializableAttribute(),
 System.Diagnostics.DebuggerStepThroughAttribute(),
 System.ComponentModel.DesignerCategoryAttribute("code"),
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True),
 System.Xml.Serialization.XmlRootAttribute([Namespace]:="", IsNullable:=False)>
    Partial Public Class odataConfig

        Private oDataHostField As String
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property oDataHost() As String
            Get
                Return Me.oDataHostField
            End Get
            Set
                Me.oDataHostField = Value
            End Set
        End Property

        Private tabulainigField As String
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property tabulaini() As String
            Get
                Return Me.tabulainigField
            End Get
            Set
                Me.tabulainigField = Value
            End Set
        End Property

        Private ouserField As String
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ouser() As String
            Get
                Return Me.ouserField
            End Get
            Set
                Me.ouserField = Value
            End Set
        End Property

        Private opassField As String
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property opass() As String
            Get
                Return Me.opassField
            End Get
            Set
                Me.opassField = Value
            End Set
        End Property

        Private environmentField As String
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property environment() As String
            Get
                Return Me.environmentField
            End Get
            Set
                Me.environmentField = Value
            End Set
        End Property

    End Class
End Namespace