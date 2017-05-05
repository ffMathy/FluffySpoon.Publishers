using System.Collections.Generic;

namespace FluffySpoon.Publisher.NuGet.DotNet
{
  interface ISolutionFileParser
  {
    IReadOnlyCollection<Project> GetProjectsFromSolutionFile(string solutionFile);
  }
}