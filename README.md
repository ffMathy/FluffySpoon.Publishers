# FluffySpoon.Publishers
Publishes the FluffySpoon packages to the respective repositories.

## Usage
The following example will:
- Find all repositories in https://github.com/ffMathy that start with `FluffySpoon.`
- For each repository:
    - Scan for C# projects and compile them
    - Scan for NodeJS projects and build them
- For each C# project, publish it to NuGet
- For each NodeJS project, publish it to NPM

```csharp
class Program
{
    static void Main()
    {
        var services = new ServiceCollection();

        //configure the publisher to take all GitHub repositories starting with "FluffySpoon."
        services.AddRepositoryToPackagePublisher("FluffySpoon.");

        //configure the publisher to use specific credentials for GitHub
        services.AddGitHubProviderForAccessToken("ffMathy", "my GitHub access token");

        //configure NuGet publishing
        services.AddNuGetProvider("my NuGet API key");

        //configure NPM publishing
        services.AddNpmProvider("my NPM key");

        //configure .NET Core project support
        services.AddDotNetProvider();

        //configure NodeJS project support
        services.AddNodeJsProvider();

        var provider = services.BuildServiceProvider();

        var publisher = provider.GetRequiredService<IRepositoryToPackagePublisher>();
        publisher.RefreshAllPackagesFromAllRepositoriesAsync().Wait();

        Console.WriteLine("All done!");
    }
}
```
