using System;
using System.Diagnostics;
using System.IO;

namespace FluffySpoon.Publisher.DotNet
{
	static class NodeJsHelper
	{
		public static void RestorePackages(string targetDirectory)
		{
			string npmPath = GetNpmPath();

			CommandLineHelper.LaunchAndWait(new ProcessStartInfo(npmPath) {
				Arguments = "install",
				WorkingDirectory = targetDirectory
			});
			CommandLineHelper.LaunchAndWait(new ProcessStartInfo(npmPath) {
				Arguments = "upgrade",
				WorkingDirectory = targetDirectory
			});
		}

		public static void Build(string targetDirectory)
		{
			CommandLineHelper.LaunchAndWait(new ProcessStartInfo(GetNpmPath())
			{
				Arguments = "run build",
				WorkingDirectory = targetDirectory
			});
		}

		public static void Test(string targetDirectory)
		{
			CommandLineHelper.LaunchAndWait(new ProcessStartInfo(GetNpmPath())
			{
				Arguments = "run test",
				WorkingDirectory = targetDirectory
			});
		}

		private static string GetNpmPath()
		{
			var basePath = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
				"nodejs");
			var npmPath = Path.Combine(basePath, "npm.cmd");
			return npmPath;
		}
	}
}
