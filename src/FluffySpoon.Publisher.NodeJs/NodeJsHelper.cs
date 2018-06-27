using System;
using System.Diagnostics;
using System.IO;

namespace FluffySpoon.Publisher.DotNet
{
	static class NodeJsHelper
	{
		public static void RestorePackages(string targetDirectory)
		{
			var basePath = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
				"nodejs");
			var npmPath = Path.Combine(basePath, "npm.cmd");

			var installPeersPath = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				"npm",
				"npm-install-peers.cmd");
			CommandLineHelper.LaunchAndWait(new ProcessStartInfo(installPeersPath)
			{
				Arguments = "",
				WorkingDirectory = targetDirectory
			});

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
