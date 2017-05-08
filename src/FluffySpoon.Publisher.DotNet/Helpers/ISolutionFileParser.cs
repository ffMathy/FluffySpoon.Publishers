using System.Collections.Generic;

namespace FluffySpoon.Publisher.DotNet
{
  interface ISolutionFileParser
  {
    IReadOnlyCollection<SolutionFileProject> GetProjectsFromSolutionFile(string solutionFile);
  }
}