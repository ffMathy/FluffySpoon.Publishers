using System.Xml.Linq;

namespace FluffySpoon.Publisher.DotNet.Helpers
{
    interface IProjectFileParser
    {
        XElement GetVersionElement(XDocument projectFile);
        XElement CreateVersionElement(XDocument projectFile);

        XElement GetPackageProjectUrlElement(XDocument projectFile);
        XElement CreatePackageProjectUrlElement(XDocument projectFile);

        XElement GetDescriptionElement(XDocument projectFile);
        XElement CreateDescriptionElement(XDocument projectFile);
    }
}