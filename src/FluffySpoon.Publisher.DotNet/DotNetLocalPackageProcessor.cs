using FluffySpoon.Publisher.Local;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FluffySpoon.Publisher.DotNet
{
  class DotNetLocalPackageProcessor : ILocalPackageProcessor
  {
    private readonly ISolutionFileParser _solutionFileParser;
    private readonly IProjectFileParser _projectFileParser;

    public DotNetLocalPackageProcessor(
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
      var nugetPackage = (IDotNetLocalPackage)package;
      BumpVersionOfProject(
        nugetPackage, 
        revision);

      CommandLineHelper.RestorePackages(package.FolderPath);
      CommandLineHelper.Build(nugetPackage.FolderPath);
    }

    private void BumpVersionOfProject(
      IDotNetLocalPackage nugetPackage,
      int revision)
    {
      var projectFileXml = XDocument.Load(nugetPackage.ProjectFilePath);

      var versionElement = GetProjectFileVersionElement(projectFileXml);

      if (!Version.TryParse(versionElement.Value, out Version existingVersion))
        existingVersion = new Version(1, 0, 0, 0);

      versionElement.Value = nugetPackage.Version = $"{existingVersion.Major}.{existingVersion.Minor}.{revision}";

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

    public async Task<IReadOnlyCollection<ILocalPackage>> ScanForPackagesInDirectoryAsync(string relativePath)
    {
      var packages = new HashSet<ILocalPackage>();

      var sourceDirectory = new DirectoryInfo(
        Path.Combine(
          AppContext.BaseDirectory,
          relativePath,
          "src"));
      if (!sourceDirectory.Exists)
        return packages;

      var solutionFile = sourceDirectory
        .GetFiles("*.sln")
        .Where(x => x
          .Name
          .StartsWith($"{nameof(FluffySpoon)}."))
        .SingleOrDefault();
      if (solutionFile == null)
        return packages;

      var projects = _solutionFileParser.GetProjectsFromSolutionFile(solutionFile.FullName);
      return projects
        .Select(MapProjectToNuGetPackage)
        .ToArray();
    }

    private IDotNetLocalPackage MapProjectToNuGetPackage(SolutionFileProject project)
    {
      var projectFileXml = XDocument.Load(project.FilePath);
      var versionElement = GetProjectFileVersionElement(projectFileXml);
      return new DotNetLocalPackage()
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
