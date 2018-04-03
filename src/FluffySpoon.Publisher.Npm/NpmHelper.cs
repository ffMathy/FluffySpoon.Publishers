using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher.NuGet
{
	static class NpmHelper
	{
		public static async Task PublishAsync(string projectPath, string authToken)
		{
			var directories = Directory.GetDirectories(projectPath);
			foreach(var directory in directories) {
				var directoryName = Path.GetFileName(directory);
				if(!directoryName.StartsWith("."))
					continue;
				
				Console.WriteLine("Purging invalid NPM directory " + directoryName + " before publishing.");
				Directory.Delete(directory, true);
			}
			
			var npmPath = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
				"nodejs",
				"npm.cmd");
			CommandLineHelper.LaunchAndWait(new ProcessStartInfo(npmPath) {
				Arguments = "set //registry.npmjs.org/:_authToken " + authToken,
				RedirectStandardOutput = true,
				WorkingDirectory = projectPath
			});

			CommandLineHelper.LaunchAndWait(new ProcessStartInfo(npmPath) {
				Arguments = "publish --access public",
				WorkingDirectory = projectPath
			});
		}
	}
}
