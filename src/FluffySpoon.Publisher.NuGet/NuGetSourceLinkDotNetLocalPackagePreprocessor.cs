using FluffySpoon.Publisher.DotNet;
using FluffySpoon.Publisher.DotNet.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FluffySpoon.Publisher.NuGet;

class NuGetSourceLinkDotNetLocalPackagePreprocessor : IDotNetLocalPackagePreprocessor
{
	private readonly IProjectFileParser _projectFileParser;

	private readonly string _packageName;
	private readonly string _packageVersion;

	public NuGetSourceLinkDotNetLocalPackagePreprocessor(
		IProjectFileParser projectFileParser,
		string packageName,
		string packageVersion)
	{
		_projectFileParser = projectFileParser;
		_packageName = packageName;
		_packageVersion = packageVersion;
	}

	public Task PreprocessPackageAsync(IDotNetLocalPackage package)
	{
		AddSourceLinkPackage(package.FolderPath);
		AddSourceLinkProjectProperties(package);

		return Task.CompletedTask;
	}

	private void AddSourceLinkProjectProperties(IDotNetLocalPackage package)
	{
		var projectFileXml = XDocument.Load(package.ProjectFilePath);

		var publishRepositoryUrlProperty = _projectFileParser.GetOrCreateElement(projectFileXml, "PublishRepositoryUrl");
		publishRepositoryUrlProperty.Value = "true";

		var embedUntrackedSourcesProperty = _projectFileParser.GetOrCreateElement(projectFileXml, "EmbedUntrackedSources");
		embedUntrackedSourcesProperty.Value = "true";

		var includeSymbolsProperty = _projectFileParser.GetOrCreateElement(projectFileXml, "IncludeSymbols");
		includeSymbolsProperty.Value = "true";

		var symbolPackageFormatProperty = _projectFileParser.GetOrCreateElement(projectFileXml, "SymbolPackageFormat");
		symbolPackageFormatProperty.Value = "symbols.nupkg";

		using var stream = File.OpenWrite(package.ProjectFilePath);
		projectFileXml.Save(stream);
	}

	private void AddSourceLinkPackage(string targetDirectory)
	{
		var arguments = $"add package {_packageName}";
		if(_packageVersion != null)
			arguments += $" --version {_packageVersion}";

		CommandLineHelper.LaunchAndWait(new ProcessStartInfo("dotnet")
		{
			Arguments = arguments,
			WorkingDirectory = targetDirectory
		});
	}
}