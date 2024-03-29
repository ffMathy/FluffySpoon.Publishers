﻿using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher.Npm;

static class NpmHelper
{
	public static Task PublishAsync(string projectPath, string authToken)
	{
		var projectDirectory = new DirectoryInfo(projectPath);
		foreach(var directory in projectDirectory.GetDirectories()) {
			if(!directory.Name.StartsWith(".") && directory.Name != "node_modules")
				continue;
				
			Console.WriteLine("Purging invalid NPM directory " + directory.Name + " before publishing.");
				
			var allFiles = directory.GetFiles("*", SearchOption.AllDirectories);
			foreach(var file in allFiles) {
				file.Attributes = FileAttributes.Normal;
			}
				
			var allDirectories = directory.GetDirectories("*", SearchOption.AllDirectories);
			foreach(var subDirectory in allDirectories) {
				subDirectory.Attributes = FileAttributes.Normal;
			}
				
			directory.Delete(true);
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

		return Task.CompletedTask;
	}
}