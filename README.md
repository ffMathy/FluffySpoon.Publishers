# FluffySpoon.Publishers
Publishes NuGet and NPM packages to the respective repositories.

## Usage
You can also see a command-line runnable version of the following example [here](https://github.com/ffMathy/FluffySpoon.Publishers/blob/master/src/FluffySpoon.Publisher.Sample/Program.cs).

The code below will:
- Find all repositories in https://github.com/ffMathy that start with `FluffySpoon.`.
- For each repository:
    - Determine the version of the package. The version used will be `1.0.<number of commits in repository>`.
    - Scan for C# projects and compile them using `dotnet build`.
    - Scan for NodeJS projects and build them using `npm run test` first and then `npm run build`.
- For each C# project, publish it to NuGet.
- For each NodeJS project, publish it to NPM.

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

## Running in AppVeyor
Below is an example of running the [sample code](https://github.com/ffMathy/FluffySpoon.Publishers/blob/master/src/FluffySpoon.Publisher.Sample/Program.cs) on an AppVeyor build server using environment variables.

### AppVeyor.yml
```yml
version: 1.0.{build}
image: Visual Studio 2017
environment:
  NuGetKey:
    secure: 14GsJ75nn9jwVPMQXN7qN8xrwhyAY8TwIvvsQ+P1yzahdtfl83J8cyN+aA9WhtSY
  ProjectNamePrefix: FluffySpoon.
  GitHubUsername: ffMathy
  GitHubPassword:
    secure: 6RzxJuCM4hx6ZUex2kEJ/g==
  NpmAuthToken:
    secure: dg3EnwKFzX5E40SPkoPK53pW2D2W5sjCGV4xhORTCoe50OEASg8Xk9mI12SBVadI
  GitHubPersonalAccessToken:
    secure: ECBBXkriJnyuksnl3PYf7PQ/WLyRZLXf9qgLyIlOIeh4e8EnYCX5gkgmyyO1/HR+
install:
- ps: |
    Install-Product node '' x64
    npm install typescript -g
build_script:
- cmd: |
    cd src
    dotnet restore
    dotnet build
    cd FluffySpoon.Publisher.Sample
    dotnet run
```
