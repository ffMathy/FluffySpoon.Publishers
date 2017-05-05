using FluffySpoon.Publisher.Local;
using FluffySpoon.Publisher.NuGet.DotNet;
using FluffySpoon.Publishers.GitHub;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher.NuGet
{
  class NuGetPackageProcessor : ILocalPackageProcessor
  {
    private readonly ISolutionFileParser _solutionFileParser;

    public NuGetPackageProcessor(
      ISolutionFileParser solutionFileParser)
    {
      _solutionFileParser = solutionFileParser;
    }

    public async Task BuildPackageAsync(ILocalPackage package)
    {
      var nugetPackage = (NuGetPackage)package;
      CommandLineHelper.Build(nugetPackage.FolderPath);
    }

    public async Task<IReadOnlyCollection<ILocalPackage>> ScanForPackagesInDirectoryAsync(string folderPath)
    {
      var packages = new HashSet<ILocalPackage>();

      var sourceDirectory = new DirectoryInfo(folderPath);
      if (!sourceDirectory.Exists)
        return packages;

      var solutionFile = sourceDirectory
        .GetFiles("*.sln")
        .Where(x => x
          .Name
          .StartsWith($"{nameof(FluffySpoon)}."))
        .Single();

      var projects = _solutionFileParser.GetProjectsFromSolutionFile(solutionFile.FullName);
      return projects
        .Select(MapProjectToNuGetPackage)
        .ToArray();
    }

    private static NuGetPackage MapProjectToNuGetPackage(Project project)
    {
      return new NuGetPackage()
      {
        FolderPath = Path.GetDirectoryName(project.FilePath),
        PublishName = project.Name,
        ProjectFilePath = project.FilePath
      };
    }
  }
}
