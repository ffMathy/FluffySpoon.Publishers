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
				Arguments = "publish",
				WorkingDirectory = projectPath
			});
		}
	}
}
