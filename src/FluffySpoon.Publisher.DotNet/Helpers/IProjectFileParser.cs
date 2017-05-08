using System.Xml.Linq;

namespace FluffySpoon.Publisher.DotNet
{
  interface IProjectFileParser
  {
    XElement GetVersionElement(XDocument projectFile);
    XElement CreateVersionElement(XDocument projectFile);
  }
}