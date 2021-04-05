﻿Imports System.Collections.Generic
Imports System.Globalization
Imports System.Text

''' <summary>
''' Base class for transformations. The default one generated by T4 isn't compatible with .NET Standard.
''' </summary>
Public MustInherit Class MyCodeGeneratorBase

#Region "Fields"
    Private _GenerationEnvironment As StringBuilder
    Private _Errors As CompilerErrorCollection
    Private _IndentLengths As List(Of Integer)
    Private _CurrentIndent As String = ""
    Private _EndsWithNewline As Boolean
#End Region

#Region "Properties"
    ''' <summary>
    ''' The string builder that generation-time code is using to assemble generated output
    ''' </summary>
    Protected Property GenerationEnvironment As StringBuilder
        Get
            _generationEnvironment = If(_generationEnvironment, New StringBuilder)
            Return _generationEnvironment
        End Get
        Set(Value As StringBuilder)
            _generationEnvironment = Value
        End Set
    End Property

    '''<summary>
    '''The error collection for the generation process
    '''</summary>
    Protected ReadOnly Property Errors As CompilerErrorCollection
        Get
            If Me._Errors Is Nothing Then
                Me._Errors = New CompilerErrorCollection()
            End If
            Return Me._Errors
        End Get
    End Property

    '''<summary>
    '''A list of the lengths of each indent that was added with PushIndent
    '''</summary>
    Private ReadOnly Property indentLengths() As System.Collections.Generic.List(Of Integer)
        Get
            If Me._IndentLengths Is Nothing Then
                Me._IndentLengths = New List(Of Integer)()
            End If
            Return Me._IndentLengths
        End Get
    End Property

    ''' <summary>
    ''' Gets the current indent we use when adding lines to the output
    ''' </summary>
    Protected Property CurrentIndent As String
        Get
            Return _CurrentIndent
        End Get
        Private Set(Value As String)
            _CurrentIndent = Value
        End Set
    End Property

    ''' <summary>
    ''' Current transformation session
    ''' </summary>
    Public Overridable Property Session As IDictionary(Of String, Object)
#End Region

#Region "Transform-time helpers"
    Public Overloads Sub Write(ByVal textToAppend As String)
        If String.IsNullOrEmpty(textToAppend) Then
            Return
        End If
        'If we're starting off, or if the previous text ended with a newline,
        'we have to append the current indent first.
        If Me.GenerationEnvironment.Length = 0 OrElse Me._EndsWithNewline Then
            Me.GenerationEnvironment.Append(Me._CurrentIndent)
            Me._EndsWithNewline = False
        End If
        'Check if the current text ends with a newline
        If textToAppend.EndsWith(Global.System.Environment.NewLine, Global.System.StringComparison.CurrentCulture) Then
            Me._EndsWithNewline = True
        End If
        'This is an optimization. If the current indent is "", then we don't have to do any
        'of the more complex stuff further down.
        If Me._CurrentIndent.Length = 0 Then
            Me.GenerationEnvironment.Append(textToAppend)
            Return
        End If
        'Everywhere there is a newline in the text, add an indent after it
        textToAppend = textToAppend.Replace(Environment.NewLine, Environment.NewLine + Me._CurrentIndent)
        'If the text ends with a newline, then we should strip off the indent added at the very end
        'because the appropriate indent will be added when the next time Write() is called
        If Me._EndsWithNewline Then
            Me.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - Me._CurrentIndent.Length))
        Else
            Me.GenerationEnvironment.Append(textToAppend)
        End If
    End Sub

    '''<summary>
    '''Write text directly into the generated output
    '''</summary>
    Public Overloads Sub WriteLine(ByVal textToAppend As String)
        Me.Write(textToAppend)
        Me.GenerationEnvironment.AppendLine()
        Me._EndsWithNewline = True
    End Sub

    '''<summary>
    '''Write formatted text directly into the generated output
    '''</summary>
    Public Overloads Sub Write(ByVal format As String, <System.ParamArrayAttribute()> ByVal args() As Object)
        Me.Write(String.Format(CultureInfo.CurrentCulture, format, args))
    End Sub

    '''<summary>
    '''Write formatted text directly into the generated output
    '''</summary>
    Public Overloads Sub WriteLine(ByVal format As String, <System.ParamArrayAttribute()> ByVal args() As Object)
        Me.WriteLine(String.Format(Global.System.Globalization.CultureInfo.CurrentCulture, format, args))
    End Sub

    ''' <summary>
    ''' Raise an error
    ''' </summary>
    Public Sub [Error](message As String)
        Throw New Exception(message)
    End Sub

    '''<summary>
    '''Increase the indent
    '''</summary>
    Public Sub PushIndent(ByVal indent As String)
        If indent = Nothing Then
            Throw New ArgumentNullException("indent")
        End If
        Me._CurrentIndent = Me.CurrentIndent + indent
        Me.indentLengths.Add(indent.Length)
    End Sub

    '''<summary>
    '''Remove the last indent that was added with PushIndent
    '''</summary>
    Public Function PopIndent() As String
        Dim returnValue As String = ""
        If Me.indentLengths.Count > 0 Then
            Dim indentLength As Integer = Me.indentLengths((Me.indentLengths.Count - 1))
            Me.indentLengths.RemoveAt(Me.indentLengths.Count - 1)
            If indentLength > 0 Then
                returnValue = Me._CurrentIndent.Substring(Me.CurrentIndent.Length - indentLength)
                Me._CurrentIndent = Me._CurrentIndent.Remove(Me._CurrentIndent.Length - indentLength)
            End If
        End If
        Return returnValue
    End Function

    '''<summary>
    '''Remove any indentation
    '''</summary>
    Public Sub ClearIndent()
        Me.indentLengths.Clear()
        Me._CurrentIndent = ""
    End Sub

#End Region

#Region "ToString Helpers"
    '''<summary>
    '''Utility class to produce culture-oriented representation of an object as a string.
    '''</summary>
    Public Class ToStringInstanceHelper

        Private _formatProvider As System.IFormatProvider = Global.System.Globalization.CultureInfo.InvariantCulture

        '''<summary>
        '''Gets or sets format provider to be used by ToStringWithCulture method.
        '''</summary>
        Public Property FormatProvider() As System.IFormatProvider
            Get
                Return Me._formatProvider
            End Get
            Set
                If Value IsNot Nothing Then
                    Me._formatProvider = Value
                End If
            End Set
        End Property
        '''<summary>
        '''This is called from the compile/run appdomain to convert objects within an expression block to a string
        '''</summary>
        Public Function ToStringWithCulture(ByVal objectToConvert As Object) As String
            If objectToConvert Is Nothing Then
                Throw New ArgumentNullException(NameOf(objectToConvert))
            End If
            Dim t As Type = objectToConvert.GetType
            Dim method As Reflection.MethodInfo = t.GetMethod("ToString", New Type() {GetType(IFormatProvider)})
            If method Is Nothing Then
                Return objectToConvert.ToString
            Else
                Return CType(method.Invoke(objectToConvert, New Object() {Me._formatProvider}), String)
            End If
        End Function
    End Class

    ''' <summary>
    ''' Helper to produce culture-oriented representation of an object as a string
    ''' </summary>
    Protected ReadOnly Property ToStringHelper As ToStringInstanceHelper = New ToStringInstanceHelper
#End Region

    ''' <summary>
    ''' Initialize the template
    ''' </summary>
    Public Overridable Sub Initialize()
    End Sub

    ''' <summary>
    ''' Create the template output
    ''' </summary>
    Public MustOverride Function TransformText() As String

    Protected Class CompilerErrorCollection
        Public ReadOnly Property HasErrors As Boolean
            Get
                Return False
            End Get
        End Property
    End Class
End Class

Namespace Global.System.Runtime.Remoting.Messaging
    Module CallContext
        Public Function LogicalGetData(s As String) As Object
            Return Nothing
        End Function
    End Module
End Namespace