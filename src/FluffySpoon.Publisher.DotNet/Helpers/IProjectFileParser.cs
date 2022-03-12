using System.Xml.Linq;

namespace FluffySpoon.Publisher.DotNet.Helpers;

public interface IProjectFileParser
{
	XElement CreateVersionElement(XDocument projectFile);
	XElement? GetPackageProjectUrlElement(XDocument projectFile);
	XElement CreatePackageProjectUrlElement(XDocument projectFile);
	XElement? GetDescriptionElement(XDocument projectFile);
	XElement CreateDescriptionElement(XDocument projectFile);
	XElement? GetPackageRepositoryTypeElement(XDocument projectFile);
	XElement CreatePackageRepositoryTypeElement(XDocument projectFile);
	XElement? GetPackageReadmeFileElement(XDocument projectFile);
	XElement CreatePackageReadmeFileElement(XDocument projectFile);
	XElement? GetPackageRepositoryUrlElement(XDocument projectFile);
	XElement CreatePackageRepositoryUrlElement(XDocument projectFile);
	XElement? GetVersionElement(XDocument projectFile);
	XElement GetOrCreatePropertyGroupElement(XDocument projectFile, string property);
	XElement CreateItemGroupElement(XDocument projectFile, string name);
}