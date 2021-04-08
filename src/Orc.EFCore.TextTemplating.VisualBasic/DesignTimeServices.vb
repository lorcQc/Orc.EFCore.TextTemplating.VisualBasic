Imports Microsoft.EntityFrameworkCore.Design
Imports Microsoft.EntityFrameworkCore.Scaffolding
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.DependencyInjection.Extensions

''' <summary>
''' Specifies our design-time services. Reference this class from your startup project using
''' <see cref="DesignTimeServicesReferenceAttribute" />.
''' </summary>
Class DesignTimeServices
    Implements IDesignTimeServices

    Public Sub ConfigureDesignTimeServices(services As IServiceCollection) Implements IDesignTimeServices.ConfigureDesignTimeServices
        services.Replace(ServiceDescriptor.Singleton(Of IModelCodeGenerator, MyModelGenerator)())
    End Sub
End Class