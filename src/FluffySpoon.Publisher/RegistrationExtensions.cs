using FluffySpoon.Publisher.Local;
using FluffySpoon.Publisher.Remote;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using FluffySpoon.Publisher;

// ReSharper disable once CheckNamespace
namespace FluffySpoon;

public static class RegistrationExtensions
{
    public static void AddRepositoryToPackagePublisher(
        this ServiceCollection services,
        string repositoryPrefix)
    {
        services.AddTransient<IRepositoryToPackagePublisher, RepositoryToPackagePublisher>();
        services.AddTransient<ISettings>(provider => new Settings(repositoryPrefix));
    }
}