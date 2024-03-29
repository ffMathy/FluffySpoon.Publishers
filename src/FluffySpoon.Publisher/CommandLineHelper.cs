﻿using System;
using System.Diagnostics;

namespace FluffySpoon.Publisher;

public static class CommandLineHelper
{
	public static void LaunchAndWait(ProcessStartInfo startInformation)
	{
		startInformation.RedirectStandardError = true;
		startInformation.RedirectStandardOutput = true;
		
		Console.WriteLine("Executing: " + startInformation.FileName + " " + startInformation.Arguments);
		
		using var process = Process.Start(startInformation);
		if (process == null)
			throw new InvalidOperationException("Could not get process.");
		
		process.WaitForExit();

		var standardOutput = process.StandardOutput.ReadToEnd();
		if(!string.IsNullOrWhiteSpace(standardOutput))
			Console.WriteLine(standardOutput);

		var standardError = process.StandardError.ReadToEnd();
		if(!string.IsNullOrWhiteSpace(standardError))
			Console.WriteLine(standardError);

		if(process.ExitCode != 0)
			throw new CommandLineException("An external tool (" + startInformation.FileName + ") returned a non-zero exit code: " + process.ExitCode);
	}
}