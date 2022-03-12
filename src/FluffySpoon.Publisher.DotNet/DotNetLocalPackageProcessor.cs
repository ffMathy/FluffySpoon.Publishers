using FluffySpoon.Publisher.Local;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluffySpoon.Publisher.DotNet.Helpers;
using FluffySpoon.Publisher.Remote;

namespace FluffySpoon.Publisher.DotNet;

class DotNetLocalPackageProcessor : ILocalPackageProcessor
{
	private readonly ISolutionFileParser _solutionFileParser;
	private readonly IProjectFileParser _projectFileParser;
	private readonly ISettings _repositoryFilter;
	private readonly IEnumerable<IDotNetLocalPackagePreprocessor> _dotNetLocalPackagePreprocessors;

	public DotNetLocalPackageProcessor(
		ISolutionFileParser solutionFileParser,
		IProjectFileParser projectFileParser,
		ISettings repositoryFilter,
		IEnumerable<IDotNetLocalPackagePreprocessor> localPackagePreprocessors)
	{
		_solutionFileParser = solutionFileParser;
		_projectFileParser = projectFileParser;
		_repositoryFilter = repositoryFilter;
		_dotNetLocalPackagePreprocessors = localPackagePreprocessors;
	}

	public async Task BuildPackageAsync(
		ILocalPackage package,
		IRemoteSourceControlRepository repository)
	{
		var nugetPackage = (IDotNetLocalPackage)package;
		foreach(var preprocessor in _dotNetLocalPackagePreprocessors) {
			await preprocessor.PreprocessPackageAsync(nugetPackage);
		}

		await UpdateProjectFileAsync(
			nugetPackage,
			repository);

		DotNetHelper.RestorePackages(nugetPackage.FolderPath);
		DotNetHelper.Build(nugetPackage.FolderPath);

		var folderDirectory = new DirectoryInfo(nugetPackage.FolderPath);
		var targetTestDirectoryPath = folderDirectory.Name + ".Tests";
		var testDirectory = folderDirectory
			.Parent
			.GetDirectories()
			.SingleOrDefault(x => x.Name == targetTestDirectoryPath);
		if(testDirectory != null)
		{
			Console.WriteLine("Test directory for package " + nugetPackage.PublishName + " found at " + testDirectory.FullName + ".");
			DotNetHelper.Test(testDirectory.FullName);
		} else
		{
			Console.WriteLine("No test directory for package " + nugetPackage.PublishName + " found.");
		}
	}

	private async Task UpdateProjectFileAsync(
		IDotNetLocalPackage nugetPackage,
		IRemoteSourceControlRepository repository)
	{
		var projectFileXml = XDocument.Load(nugetPackage.ProjectFilePath);
		await BumpProjectFileVersionAsync(
			nugetPackage,
			repository,
			projectFileXml);

		var projectUrlElement = GetPackageProjectUrlElement(projectFileXml);
		projectUrlElement.Value = repository.PublicUrl ?? string.Empty;

		var descriptionElement = GetDescriptionElement(projectFileXml);
		descriptionElement.Value = repository.Summary ?? string.Empty;

		using var stream = File.OpenWrite(nugetPackage.ProjectFilePath);
		projectFileXml.Save(stream);
	}

	private async Task BumpProjectFileVersionAsync(
		ILocalPackage package,
		IRemoteSourceControlRepository repository,
		XDocument projectFileXml)
	{
		var system = repository.System;

		var revision = await system.GetRevisionOfRepository(repository);
		Console.WriteLine("Updating project revision " + revision + " of project file for package " + package.PublishName);

		var versionElement = GetProjectFileVersionElement(projectFileXml);
		var repositoryUrlElement = GetPackageRepositoryUrlElement(projectFileXml);
		var repositoryTypeElement = GetPackageRepositoryTypeElement(projectFileXml);

		if (!Version.TryParse(versionElement.Value, out Version existingVersion))
			existingVersion = new Version(1, 0, 0, 0);

		versionElement.Value = package.Version = $"{existingVersion.Major}.{existingVersion.Minor+revision}.{existingVersion.Build}";
		repositoryUrlElement.Value = repository.ContributeUrl;
	}

	private XElement GetDescriptionElement(XDocument projectFileXml)
	{
		return _projectFileParser.GetDescriptionElement(projectFileXml) ??
		       _projectFileParser.CreateDescriptionElement(projectFileXml);
	}

	private XElement GetPackageProjectUrlElement(XDocument projectFileXml)
	{
		return _projectFileParser.GetPackageProjectUrlElement(projectFileXml) ??
		       _projectFileParser.CreatePackageProjectUrlElement(projectFileXml);
	}

	private XElement GetPackageRepositoryUrlElement(XDocument projectFileXml)
	{
		return _projectFileParser.GetPackageRepositoryUrlElement(projectFileXml) ??
		       _projectFileParser.CreatePackageRepositoryUrlElement(projectFileXml);
	}

	private XElement GetPackageRepositoryTypeElement(XDocument projectFileXml)
	{
		return _projectFileParser.GetPackageRepositoryTypeElement(projectFileXml) ??
		       _projectFileParser.CreatePackageRepositoryTypeElement(projectFileXml);
	}

	private XElement GetProjectFileVersionElement(XDocument projectFileXml)
	{
		return _projectFileParser.GetVersionElement(projectFileXml) ??
		       _projectFileParser.CreateVersionElement(projectFileXml);
	}

	public Task<IReadOnlyCollection<ILocalPackage>> ScanForPackagesInDirectoryAsync(string relativePath)
	{
		var sourceDirectory = new DirectoryInfo(
			Path.Combine(
				Environment.CurrentDirectory,
				relativePath,
				"src"));
		if (!sourceDirectory.Exists)
			return Task.FromResult<IReadOnlyCollection<ILocalPackage>>(new HashSet<ILocalPackage>());

		var solutionFile = sourceDirectory
			.GetFiles("*.sln")
			.SingleOrDefault(x => x
				.Name
				.StartsWith(_repositoryFilter.ProjectPrefix));
		if (solutionFile == null)
			return Task.FromResult<IReadOnlyCollection<ILocalPackage>>(new HashSet<ILocalPackage>());

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