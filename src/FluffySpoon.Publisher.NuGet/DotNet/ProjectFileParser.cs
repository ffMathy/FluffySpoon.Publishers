using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FluffySpoon.Publisher.NuGet.DotNet
{
  class ProjectFileParser : IProjectFileParser
  {
    public XElement CreateVersionElement(XDocument projectFile)
    {
      var propertyGroups = GetPropertyGroups(projectFile);
      var firstGroup = GetPropertyGroups(projectFile).First();
      var version = new XElement(
        XName.Get(
          "Version",
          projectFile
            .Root
            .GetDefaultNamespace()
            .NamespaceName));
      firstGroup.Add(version);
      return version;
    }

    public XElement GetVersionElement(XDocument projectFile)
    {
      return GetPropertyGroups(projectFile)
        .SelectMany(x => x.Elements())
        .SingleOrDefault(x => x.Name.LocalName == "Version");
    }

    private static IEnumerable<XElement> GetPropertyGroups(XDocument projectFile)
    {
      return projectFile
        .Elements()
        .Where(x => x.Name.LocalName == "PropertyGroup");
    }
  }
}
