using System.Xml.Linq;

namespace FluffySpoon.Publisher.NuGet.DotNet
{
  interface IProjectFileParser
  {
    XElement GetVersionElement(XDocument projectFile);
    XElement CreateVersionElement(XDocument projectFile);
  }
}