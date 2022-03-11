# FluffySpoon.Publishers
Publishes NuGet and NPM packages to the respective repositories.

## Usage
You can also see a command-line runnable version of the following example [here](https://github.com/ffMathy/FluffySpoon.Publishers/blob/master/src/FluffySpoon.Publisher.Sample/Program.cs).

The code below will:
- Find all repositories in https://github.com/ffMathy that start with `FluffySpoon.` and have been modified within the past 30 days.
- For each repository, sorted by update date descending:
    - Determine the version of the package. The version used will be `1.0.<number of commits in repository>`.
    - For each C# project found in the `src` folder of the root of the repository:
		- Build `src/<ProjectName>` using `dotnet build`.
		- Test `src/<ProjectName>.Tests` if present using `dotnet test`.
		- Publish to NuGet if tests pass or no tests were present.
    - For each NodeJS project found in the `src` folder of the root of the repository:
		- Build the root directory using `npm run build`.
		- Test the root directory using `npm run test`.
		- Publish to NPM.
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

## Running in GitHub Actions
Below is an example of running the [sample code](https://github.com/ffMathy/FluffySpoon.Publishers/blob/master/src/FluffySpoon.Publisher.Sample/Program.cs) on an GitHub Actions using environment variables.

### dotnet.yml
```yml
name: .NET

on:
  push:
  schedule:
    - cron: '0 * * * *'
    
jobs:
  build:

    runs-on: ubuntu-latest

    steps:
    - uses: actions/checkout@v2
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: 6.0.x
        
    - name: Build
      env:
        ProjectNamePrefix: FluffySpoon.
        GitHubUsername: ffMathy
        NuGetKey: ${{ secrets.NuGetKey }}
        NpmAuthToken: ${{ secrets.NpmAuthToken }}
        GitHubPersonalAccessToken: ${{ secrets.GitHubPersonalAccessToken }}
      run: |
        cd src
        dotnet restore
        dotnet build
        cd FluffySpoon.Publisher.Sample
        dotnet run
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
