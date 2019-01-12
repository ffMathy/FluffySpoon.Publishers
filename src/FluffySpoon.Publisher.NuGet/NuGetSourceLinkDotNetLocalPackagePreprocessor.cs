using FluffySpoon.Publisher.DotNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher.NuGet
{
	class NuGetSourceLinkDotNetLocalPackagePreprocessor : IDotNetLocalPackagePreprocessor
	{
		private readonly string _packageName;
		private readonly string _packageVersion;

		public NuGetSourceLinkDotNetLocalPackagePreprocessor(
			string packageName,
			string packageVersion)
		{
			_packageName = packageName;
			_packageVersion = packageVersion;
		}

		public Task PreprocessPackageAsync(IDotNetLocalPackage package)
		{
			AddSourceLinkPackage(package.FolderPath);
			return Task.CompletedTask;
		}

		private void AddSourceLinkPackage(string targetDirectory)
		{
			var arguments = $"add package {_packageName}";
			if(_packageVersion != null)
				arguments += $" --version {_packageVersion}";

			CommandLineHelper.LaunchAndWait(new ProcessStartInfo("dotnet.exe")
			{
				Arguments = arguments,
				WorkingDirectory = targetDirectory
			});
		}
	}
}
