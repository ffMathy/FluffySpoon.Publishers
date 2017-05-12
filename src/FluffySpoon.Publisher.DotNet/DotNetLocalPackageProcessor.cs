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
        private readonly ISettings _repositoryFilter;

        public DotNetLocalPackageProcessor(
          ISolutionFileParser solutionFileParser,
          IProjectFileParser projectFileParser,
          ISettings repositoryFilter)
        {
            _solutionFileParser = solutionFileParser;
            _projectFileParser = projectFileParser;
            _repositoryFilter = repositoryFilter;
        }

        public Task BuildPackageAsync(
          ILocalPackage package,
          int revision)
        {
            var nugetPackage = (IDotNetLocalPackage)package;
            BumpVersionOfProject(
              nugetPackage,
              revision);

            DotNetHelper.RestorePackages(package.FolderPath);
            DotNetHelper.Build(nugetPackage.FolderPath);

            return Task.CompletedTask;
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

        public Task<IReadOnlyCollection<ILocalPackage>> ScanForPackagesInDirectoryAsync(string relativePath)
        {
            var packages = new HashSet<ILocalPackage>();

            var sourceDirectory = new DirectoryInfo(
              Path.Combine(
                AppContext.BaseDirectory,
                relativePath,
                "src"));
            if (!sourceDirectory.Exists)
                return Task.FromResult<IReadOnlyCollection<ILocalPackage>>(packages);

            var solutionFile = sourceDirectory
              .GetFiles("*.sln")
              .Where(x => x
                .Name
                .StartsWith(_repositoryFilter.ProjectPrefix))
              .SingleOrDefault();
            if (solutionFile == null)
                return Task.FromResult<IReadOnlyCollection<ILocalPackage>>(packages);

            var projects = _solutionFileParser.GetProjectsFromSolutionFile(solutionFile.FullName);
            var result = projects
              .Select(MapProjectToNuGetPackage)
              .ToArray();

            return Task.FromResult<IReadOnlyCollection<ILocalPackage>>(result);
        }

        private ILocalPackage MapProjectToNuGetPackage(SolutionFileProject project)
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
