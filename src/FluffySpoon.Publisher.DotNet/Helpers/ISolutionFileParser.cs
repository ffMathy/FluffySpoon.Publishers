using System.Collections.Generic;

namespace FluffySpoon.Publisher.DotNet.Helpers
{
  interface ISolutionFileParser
  {
    IReadOnlyCollection<SolutionFileProject> GetProjectsFromSolutionFile(string solutionFile);
  }
}