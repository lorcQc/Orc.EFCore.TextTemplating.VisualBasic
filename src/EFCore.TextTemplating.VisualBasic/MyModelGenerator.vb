Imports Bricelam.EntityFrameworkCore.VisualBasic.Scaffolding.Internal
Imports Microsoft.EntityFrameworkCore.Metadata
Imports Microsoft.EntityFrameworkCore.Scaffolding

Public Class MyModelGenerator
    Inherits ModelCodeGenerator

    Public Overridable ReadOnly Property _VBDbContextGenerator As IVisualBasicDbContextGenerator
    Public Overridable ReadOnly Property _VBEntityTypeGenerator As IVisualBasicEntityTypeGenerator

    Public Sub New(dependencies As ModelCodeGeneratorDependencies,
                       vbDbContextGenerator As IVisualBasicDbContextGenerator,
                       vbEntityTypeGenerator As IVisualBasicEntityTypeGenerator)

        MyBase.New(dependencies)

        If vbDbContextGenerator Is Nothing Then Throw New ArgumentNullException(NameOf(vbDbContextGenerator))
        If vbEntityTypeGenerator Is Nothing Then Throw New ArgumentNullException(NameOf(vbEntityTypeGenerator))

        _VBDbContextGenerator = vbDbContextGenerator
        _VBEntityTypeGenerator = vbEntityTypeGenerator
    End Sub


    Private Const FileExtension = ".vb"

    Public Overrides ReadOnly Property Language As String
        Get
            Return "VB"
        End Get
    End Property

    Public Overrides Function GenerateModel(model As IModel, options As ModelCodeGenerationOptions) As ScaffoldedModel

        Dim resultingFiles As New ScaffoldedModel

        Dim connectionString = options.ConnectionString

        ' HACK: Work around dotnet/efcore#19799
        If File.Exists(connectionString) Then
            connectionString = "Data Source=(local);Initial Catalog=" & CStr(model("Scaffolding:DatabaseName"))
        End If

        Dim contextGenerator As New MyDbContextGenerator With {
            .Session = New Dictionary(Of String, Object) From {
                {"Model", model},
                {"ModelNamespace", options.ModelNamespace},
                {"Namespace", options.ContextNamespace},
                {"ContextName", options.ContextName},
                {"ConnectionString", connectionString},
                {"SuppressConnectionStringWarning", options.SuppressConnectionStringWarning},
                {"UseDataAnnotations", options.UseDataAnnotations},
                {"Code", _csharpHelper},
                {"ProviderCode", _providerConfigurationCodeGenerator},
                {"AnnotationCode", _annotationCodeGenerator
        }
        }
        }
        contextGenerator.Initialize()
        Private generatedCode = contextGenerator.TransformText()

        Private dbContextFileName = options.ContextName & ".cs"
        resultingFiles.ContextFile = New ScaffoldedFile With
        {
        .Path = Path.Combine(options.ContextDir, dbContextFileName),
        .Code = generatedCode
        }

        For Each entityType In model.GetEntityTypes()
            Dim entityGenerator As New MyEntityTypeGenerator With
        {
            .Session = New Dictionary(Of String, Object) From {
        {"EntityType", entityType},
        {"Namespace", options.ModelNamespace},
        {"UseDataAnnotations", options.UseDataAnnotations},
        {"Code", _csharpHelper
        }
            }
        }
            entityGenerator.Initialize()
            generatedCode = entityGenerator.TransformText()

            resultingFiles.AdditionalFiles.Add(
        New ScaffoldedFile With
                {
            .Path = entityType.Name & ".cs",
            .Code = generatedCode
                })

            Dim configurationGenerator As New MyEntityTypeConfigurationGenerator With
            {
            .Session = New Dictionary(Of String, Object) From {
                    {"EntityType", entityType},
                    {"ModelNamespace", options.ModelNamespace},
                    {"Namespace", options.ContextNamespace},
                    {"UseDataAnnotations", options.UseDataAnnotations},
                    {"Code", _csharpHelper},
                    {"AnnotationCode", _annotationCodeGenerator
        }
            }
            }
            configurationGenerator.Initialize()
            generatedCode = configurationGenerator.TransformText()

            resultingFiles.AdditionalFiles.Add(
        New ScaffoldedFile With
                {
            .Path = Path.Combine(options.ContextDir, entityType.Name & "Configuration.cs"),
            .Code = generatedCode
                })
        Next

        Return resultingFiles

    End Function
End Class
