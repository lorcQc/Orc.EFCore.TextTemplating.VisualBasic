Imports System.IO
Imports EntityFrameworkCore.VisualBasic.Design
Imports Orc.EFCore.TextTemplating.VisualBasic.Templates
Imports Microsoft.EntityFrameworkCore.Design
Imports Microsoft.EntityFrameworkCore.Metadata
Imports Microsoft.EntityFrameworkCore.Scaffolding

Class MyModelGenerator
    Inherits ModelCodeGenerator

    ReadOnly _providerConfigurationCodeGenerator As IProviderConfigurationCodeGenerator
    ReadOnly _annotationCodeGenerator As IAnnotationCodeGenerator
    Private ReadOnly _VbHelper As IVisualBasicHelper

    Public Sub New(dependencies As ModelCodeGeneratorDependencies,
                   providerConfigurationCodeGenerator As IProviderConfigurationCodeGenerator,
                   annotationCodeGenerator As IAnnotationCodeGenerator,
                   vbHelper As IVisualBasicHelper)

        MyBase.New(dependencies)

        If providerConfigurationCodeGenerator Is Nothing Then Throw New ArgumentNullException(NameOf(providerConfigurationCodeGenerator))
        If annotationCodeGenerator Is Nothing Then Throw New ArgumentNullException(NameOf(annotationCodeGenerator))
        If vbHelper Is Nothing Then Throw New ArgumentNullException(NameOf(vbHelper))

        _providerConfigurationCodeGenerator = providerConfigurationCodeGenerator
        _annotationCodeGenerator = annotationCodeGenerator
        _VbHelper = vbHelper
    End Sub

    Private Const FileExtension = ".vb"

    Public Overrides ReadOnly Property Language As String
        Get
            Return "VB"
        End Get
    End Property

    Public Overrides Function GenerateModel(model As IModel,
                                            options As ModelCodeGenerationOptions) As ScaffoldedModel

        Dim resultingFiles As New ScaffoldedModel

        Dim connectionString = options.ConnectionString

        '' HACK: Work around dotnet/efcore#19799
        'If File.Exists(connectionString) Then
        '    connectionString = "Data Source=(local);Initial Catalog=" & CStr(model("Scaffolding:DatabaseName"))
        'End If

        Dim contextGenerator As New MyDbContextGenerator With {
            .Session = New Dictionary(Of String, Object) From {
                {"Model", model},
                {"ModelNamespace", options.ModelNamespace},
                {"Namespace", options.ContextNamespace},
                {"ContextName", options.ContextName},
                {"ConnectionString", connectionString},
                {"SuppressConnectionStringWarning", options.SuppressConnectionStringWarning},
                {"UseDataAnnotations", options.UseDataAnnotations},
                {"Code", _VbHelper},
                {"ProviderCode", _providerConfigurationCodeGenerator},
                {"AnnotationCode", _annotationCodeGenerator}
            }
        }
        contextGenerator.Initialize()
        Dim generatedCode = contextGenerator.TransformText()

        Dim dbContextFileName = options.ContextName & FileExtension

        resultingFiles.ContextFile = New ScaffoldedFile With {
            .Path = Path.Combine(options.ContextDir, dbContextFileName),
            .Code = generatedCode
        }

        For Each entityType In model.GetEntityTypes()
            Dim entityGenerator As New MyEntityTypeGenerator With {
                .Session = New Dictionary(Of String, Object) From {
                    {"EntityType", entityType},
                    {"Namespace", options.ModelNamespace},
                    {"UseDataAnnotations", options.UseDataAnnotations},
                    {"Code", _VbHelper}
                }
            }
            entityGenerator.Initialize()
            generatedCode = entityGenerator.TransformText()

            resultingFiles.AdditionalFiles.Add(
                New ScaffoldedFile With {
                    .Path = entityType.Name & FileExtension,
                    .Code = generatedCode
                }
            )

            Dim configurationGenerator As New MyEntityTypeConfigurationGenerator With {
                .Session = New Dictionary(Of String, Object) From {
                    {"EntityType", entityType},
                    {"ModelNamespace", options.ModelNamespace},
                    {"Namespace", options.ContextNamespace},
                    {"UseDataAnnotations", options.UseDataAnnotations},
                    {"Code", _VbHelper},
                    {"AnnotationCode", _annotationCodeGenerator}
                }
            }

            configurationGenerator.Initialize()
            generatedCode = configurationGenerator.TransformText()

            resultingFiles.AdditionalFiles.Add(
                New ScaffoldedFile With {
                    .Path = Path.Combine(options.ContextDir, entityType.Name & "Configuration" & FileExtension),
                    .Code = generatedCode
                })
        Next

        Return resultingFiles

    End Function
End Class
