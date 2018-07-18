using System;
using System.Diagnostics;

namespace FluffySpoon.Publisher
{
	public static class CommandLineHelper
	{
		public static void LaunchAndWait(ProcessStartInfo startInformation)
		{
			using (var process = Process.Start(startInformation))
			{
				process.WaitForExit();

				if(process.ExitCode != 0)
					throw new CommandLineException("An external tool (" + startInformation.FileName + ") returned a non-zero exit code: " + process.ExitCode);
			}
		}
	}
}
