Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Metadata.Internal
Imports System

'------------------------------------------------------------------------------
'<auto-generated>
'    Ce code a été généré par un outil.
'    Version du runtime : 16.0.0.0
' 
'    Les changements apportés à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
'    le code est régénéré.
'</auto-generated>
'------------------------------------------------------------------------------
Namespace Templates
    '''<summary>
    '''Class to produce the template output
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")>  _
    Partial Friend Class MyEntityTypeGenerator
        Inherits MyCodeGeneratorBase
        '''<summary>
        '''Create the template output
        '''</summary>
        Public Overrides Function TransformText() As String
            Me.Write(""&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10))
            Me.Write(""&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10))
            Me.Write("Imports System"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Imports Microsoft.VisualBasic"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Imports System.Collections.Generic"& _ 
                    ""&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10))
 If UseDataAnnotations Then 
            Me.Write("Imports System.ComponentModel.DataAnnotations"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Imports System.ComponentModel.Data"& _ 
                    "Annotations.Schema"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Imports Microsoft.EntityFrameworkCore"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10))
 End If 
            Me.Write(""&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10))
 If Not String.IsNullOrEmpty(ModelNamespace) Then 
            Me.Write("Namespace ")
            Me.Write(Me.ToStringHelper.ToStringWithCulture(ModelNamespace))
            Me.Write(""&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10))
 End If
   Dim entityTypeComment = EntityType.GetComment()
   If entityTypeComment isnot nothing then 
            Me.Write(""&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"    ''' <summary>"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10))
 For Each line As String In entityTypeComment.Split({vbCrLf, vbCr, vbLf}, StringSplitOptions.None) 
            Me.Write("    ''' ")
            Me.Write(Me.ToStringHelper.ToStringWithCulture(Security.SecurityElement.Escape(line)))
            Me.Write(""&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10))
 Next 
            Me.Write("    ''' </summary>"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10))
 End If 
            Me.Write(""&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"    Public Partial Class ")
            Me.Write(Me.ToStringHelper.ToStringWithCulture(entityType.Name))
            Me.Write(""&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10))
 For Each prop In entityType.GetProperties().OrderBy(Function(p) p.GetColumnOrdinal())
        Dim PropertyComment = prop .GetComment()
        If PropertyComment isnot nothing then 
            Me.Write(""&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"            ''' <summary>"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"        ")
 For Each line As String In PropertyComment.Split({vbCrLf, vbCr, vbLf}, StringSplitOptions.None) 
            Me.Write("            ''' ")
            Me.Write(Me.ToStringHelper.ToStringWithCulture(Security.SecurityElement.Escape(line)))
            Me.Write(""&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"        ")
 Next 
            Me.Write("            ''' </summary>"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"        ")
 End If 
            Me.Write("        Public Property ")
            Me.Write(Me.ToStringHelper.ToStringWithCulture(code.Identifier(prop.Name)))
            Me.Write(" As ")
            Me.Write(Me.ToStringHelper.ToStringWithCulture(code.Reference(prop.ClrType)))
            Me.Write(""&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10))
 Next 
            Me.Write(""&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10))
 For Each navigation in EntityType.GetNavigations()
        Dim referencedTypeName = navigation.TargetEntityType.Name
        Dim navigationType = If(navigation.IsCollection, $"ICollection(Of {referencedTypeName})", referencedTypeName)

            Me.Write("        Public Overridable Property ")
            Me.Write(Me.ToStringHelper.ToStringWithCulture(navigation.Name))
            Me.Write(" As New ")
            Me.Write(Me.ToStringHelper.ToStringWithCulture(navigationType))
            Me.Write(""&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10))
 Next 
            Me.Write(""&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"    End Class"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10))
 If Not String.IsNullOrEmpty(ModelNamespace) Then 
            Me.Write("End Namespace"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10))
	End If 
            Return Me.GenerationEnvironment.ToString
        End Function

Private _EntityTypeField As Global.Microsoft.EntityFrameworkCore.Metadata.IEntityType

'''<summary>
'''Access the EntityType parameter of the template.
'''</summary>
Private ReadOnly Property EntityType() As Global.Microsoft.EntityFrameworkCore.Metadata.IEntityType
    Get
        Return Me._EntityTypeField
    End Get
End Property

Private _ModelNamespaceField As String

'''<summary>
'''Access the ModelNamespace parameter of the template.
'''</summary>
Private ReadOnly Property ModelNamespace() As String
    Get
        Return Me._ModelNamespaceField
    End Get
End Property

Private _UseDataAnnotationsField As Boolean

'''<summary>
'''Access the UseDataAnnotations parameter of the template.
'''</summary>
Private ReadOnly Property UseDataAnnotations() As Boolean
    Get
        Return Me._UseDataAnnotationsField
    End Get
End Property

Private _CodeField As Global.EntityFrameworkCore.VisualBasic.Design.IVisualBasicHelper

'''<summary>
'''Access the Code parameter of the template.
'''</summary>
Private ReadOnly Property Code() As Global.EntityFrameworkCore.VisualBasic.Design.IVisualBasicHelper
    Get
        Return Me._CodeField
    End Get
End Property


'''<summary>
'''Initialize the template
'''</summary>
Public Overrides Sub Initialize()
    MyBase.Initialize
    If (Me.Errors.HasErrors = false) Then
Dim EntityTypeValueAcquired As Boolean = false
If Me.Session.ContainsKey("EntityType") Then
    Me._EntityTypeField = CType(Me.Session("EntityType"),Global.Microsoft.EntityFrameworkCore.Metadata.IEntityType)
    EntityTypeValueAcquired = true
End If
If (EntityTypeValueAcquired = false) Then
    Dim data As Object = Global.System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("EntityType")
    If (Not (data) Is Nothing) Then
        Me._EntityTypeField = CType(data,Global.Microsoft.EntityFrameworkCore.Metadata.IEntityType)
    End If
End If
Dim ModelNamespaceValueAcquired As Boolean = false
If Me.Session.ContainsKey("ModelNamespace") Then
    Me._ModelNamespaceField = CType(Me.Session("ModelNamespace"),String)
    ModelNamespaceValueAcquired = true
End If
If (ModelNamespaceValueAcquired = false) Then
    Dim data As Object = Global.System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("ModelNamespace")
    If (Not (data) Is Nothing) Then
        Me._ModelNamespaceField = CType(data,String)
    End If
End If
Dim UseDataAnnotationsValueAcquired As Boolean = false
If Me.Session.ContainsKey("UseDataAnnotations") Then
    Me._UseDataAnnotationsField = CType(Me.Session("UseDataAnnotations"),Boolean)
    UseDataAnnotationsValueAcquired = true
End If
If (UseDataAnnotationsValueAcquired = false) Then
    Dim data As Object = Global.System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("UseDataAnnotations")
    If (Not (data) Is Nothing) Then
        Me._UseDataAnnotationsField = CType(data,Boolean)
    End If
End If
Dim CodeValueAcquired As Boolean = false
If Me.Session.ContainsKey("Code") Then
    Me._CodeField = CType(Me.Session("Code"),Global.EntityFrameworkCore.VisualBasic.Design.IVisualBasicHelper)
    CodeValueAcquired = true
End If
If (CodeValueAcquired = false) Then
    Dim data As Object = Global.System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("Code")
    If (Not (data) Is Nothing) Then
        Me._CodeField = CType(data,Global.EntityFrameworkCore.VisualBasic.Design.IVisualBasicHelper)
    End If
End If


    End If
End Sub


    End Class
End Namespace
