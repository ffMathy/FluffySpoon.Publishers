using System;
using System.Diagnostics;
using System.IO;

namespace FluffySpoon.Publisher.DotNet
{
	static class NodeJsHelper
	{
		public static void RestorePackages(string targetDirectory)
		{
			var npmPath = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
				"nodejs",
				"npm.cmd");
			CommandLineHelper.LaunchAndWait(new ProcessStartInfo(npmPath) {
				Arguments = "install",
				WorkingDirectory = targetDirectory
			});
		}

		public static void Build(string targetDirectory)
		{
			var tscPath = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				"npm",
				"tsc.cmd");
			CommandLineHelper.LaunchAndWait(new ProcessStartInfo(tscPath) {
				Arguments = "",
				WorkingDirectory = targetDirectory
			});
		}
	}
}
