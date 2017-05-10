using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FluffySpoon.Publisher
{
  public static class CommandLineHelper
  {
    public static void LaunchAndWait(ProcessStartInfo startInformation)
    {
      using (var process = Process.Start(startInformation))
      {
        process.WaitForExit();
      }
    }
  }
}
