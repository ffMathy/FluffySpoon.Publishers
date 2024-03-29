﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using FluffySpoon.Publisher.Local;
using FluffySpoon.Publisher.Remote;
using Newtonsoft.Json;

namespace FluffySpoon.Publisher.NodeJs;

class NodeJsLocalPackageProcessor : ILocalPackageProcessor
{
	private readonly ISettings _repositoryFilter;

	public NodeJsLocalPackageProcessor(
		ISettings repositoryFilter)
	{
		_repositoryFilter = repositoryFilter;
	}

	public async Task BuildPackageAsync(
		ILocalPackage package,
		IRemoteSourceControlRepository repository)
	{
		var nugetPackage = (INodeJsLocalPackage)package;
		await UpdateProjectFileAsync(
			nugetPackage,
			repository);

		NodeJsHelper.RestorePackages(package.FolderPath);
		NodeJsHelper.Build(nugetPackage.FolderPath);
		NodeJsHelper.Test(nugetPackage.FolderPath);
	}

	private async Task UpdateProjectFileAsync(
		INodeJsLocalPackage package,
		IRemoteSourceControlRepository repository)
	{
		dynamic packageJson = GetPackageJson(package.PackageJsonFilePath);

		packageJson.homepage = repository.PublicUrl ?? string.Empty;
		packageJson.description = repository.Summary ?? string.Empty;

		var system = repository.System;
		var revision = await system.GetRevisionOfRepository(repository);
		Console.WriteLine("Updating project revision " + revision + " of package.json for package " + package.PublishName);

		if (!Version.TryParse(packageJson.version, out Version existingVersion))
			existingVersion = new Version(1, 0, 0, 0);

		packageJson.version = package.Version = $"{existingVersion.Major}.{existingVersion.Minor+revision}.{existingVersion.Build}";

		await File.WriteAllTextAsync(package.PackageJsonFilePath, JsonConvert.SerializeObject(packageJson));
	}

	public Task<IReadOnlyCollection<ILocalPackage>> ScanForPackagesInDirectoryAsync(string relativePath)
	{
		var sourceDirectory = new DirectoryInfo(
			Path.Combine(
				Environment.CurrentDirectory,
				relativePath));

		var packageJsonFilePath = Path.Combine(Environment.CurrentDirectory, relativePath, "package.json");
		if (!File.Exists(packageJsonFilePath))
			return Task.FromResult<IReadOnlyCollection<ILocalPackage>>(new HashSet<ILocalPackage>());

		dynamic packageJson = GetPackageJson(packageJsonFilePath);

		return Task.FromResult<IReadOnlyCollection<ILocalPackage>>(new[] {
			new NodeJsLocalPackage() {
				FolderPath = sourceDirectory.FullName,
				PackageJsonFilePath = packageJsonFilePath,
				Processor = this,
				PublishName = packageJson.name,
				Version = packageJson.version
			}
		});
	}

	private static ExpandoObject GetPackageJson(string packageJsonFilePath)
	{
		return JsonConvert.DeserializeObject<ExpandoObject>(File.ReadAllText(packageJsonFilePath));
	}
}