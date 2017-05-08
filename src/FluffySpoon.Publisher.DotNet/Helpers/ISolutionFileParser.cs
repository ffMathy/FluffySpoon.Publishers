using System.Collections.Generic;

namespace FluffySpoon.Publisher.DotNet
{
  interface ISolutionFileParser
  {
    IReadOnlyCollection<Project> GetProjectsFromSolutionFile(string solutionFile);
  }
}