﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace FluffySpoon.Publisher.DotNet.Helpers;

class SolutionFileParser : ISolutionFileParser
{
  public IReadOnlyCollection<SolutionFileProject> GetProjectsFromSolutionFile(string solutionFile)
  {
    Console.WriteLine("Getting projects for: " + solutionFile);
    
    var projects = new List<SolutionFileProject>();
      
    var guidRegex = "\\{[A-F0-9]{8}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{12}\\}";
    var projectRegularExpression = new Regex($"Project\\(\"{guidRegex}\"\\) = \"(.+)\", \"(.+)\", \"{guidRegex}\"");
    foreach (var line in File.ReadAllLines(solutionFile))
    {
      var match = projectRegularExpression.Match(line);
      if (match.Groups.Count < 3)
        continue;
      
      var filePath = Path
        .Combine(
          Path.GetDirectoryName(solutionFile), 
          match.Groups[2].Value)
        .Replace('\\', Path.DirectorySeparatorChar)
        .Replace('/', Path.DirectorySeparatorChar);
      if (!filePath.EndsWith(".csproj"))
        continue;
      
      Console.WriteLine("Found project: " + line);

      projects.Add(new SolutionFileProject()
      {
        FilePath = filePath,
        Name = match.Groups[1].Value
      });
    }

    return projects;
  }
}