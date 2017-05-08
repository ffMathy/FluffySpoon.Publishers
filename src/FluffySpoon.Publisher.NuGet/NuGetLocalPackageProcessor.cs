using FluffySpoon.Publisher.Local;
using FluffySpoon.Publisher.NuGet.DotNet;
using FluffySpoon.Publishers.GitHub;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FluffySpoon.Publisher.NuGet
{
  class NuGetLocalPackageProcessor : ILocalPackageProcessor
  {
    private readonly ISolutionFileParser _solutionFileParser;
    private readonly IProjectFileParser _projectFileParser;

    public NuGetLocalPackageProcessor(
      ISolutionFileParser solutionFileParser,
      IProjectFileParser projectFileParser)
    {
      _solutionFileParser = solutionFileParser;
      _projectFileParser = projectFileParser;
    }

    public async Task BuildPackageAsync(
      ILocalPackage package,
      int revision)
    {
      var nugetPackage = (NuGetLocalPackage)package;
      BumpVersionOfProject(
        nugetPackage, 
        revision);

      CommandLineHelper.Build(nugetPackage.FolderPath);
    }

    private void BumpVersionOfProject(
      NuGetLocalPackage nugetPackage,
      int revision)
    {
      var projectFileXml = XDocument.Load(nugetPackage.ProjectFilePath);

      var versionElement = GetProjectFileVersionElement(projectFileXml);

      var existingVersion = new Version(versionElement.Value);
      versionElement.Value = $"{existingVersion.Major}.{existingVersion.Minor}.{revision}";

      using (var stream = File.OpenWrite(nugetPackage.ProjectFilePath))
      {
        projectFileXml.Save(stream);
      }
    }

    private XElement GetProjectFileVersionElement(XDocument projectFileXml)
    {
      return _projectFileParser.GetVersionElement(projectFileXml) ??
              _projectFileParser.CreateVersionElement(projectFileXml);
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

    private NuGetLocalPackage MapProjectToNuGetPackage(Project project)
    {
      var projectFileXml = XDocument.Load(project.FilePath);
      var versionElement = GetProjectFileVersionElement(projectFileXml);
      return new NuGetLocalPackage()
      {
        FolderPath = Path.GetDirectoryName(project.FilePath),
        PublishName = project.Name,
        ProjectFilePath = project.FilePath,
        Version = versionElement.Value,
        Processor = this
      };
    }
  }
}
