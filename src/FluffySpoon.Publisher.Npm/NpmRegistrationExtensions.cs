using FluffySpoon.Publisher.Npm;
using FluffySpoon.Publisher.Remote;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace FluffySpoon;

public static class NpmRegistrationExtensions
{
    public static void AddNpmProvider(
        this ServiceCollection services,
        string authToken)
    {
        services.AddTransient<NpmRemotePackageSystem>();
        services.AddTransient<INpmSettings>(provider => new NpmSettings(authToken));

        services.AddTransient<IRemotePackageSystem, NpmRemotePackageSystem>();
    }
}