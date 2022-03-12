using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FluffySpoon.Publisher.DotNet.Helpers;

class ProjectFileParser : IProjectFileParser
{
	private static XElement CreatePropertyGroupElement(XDocument projectFile, string name)
	{
		var firstGroup = GetPropertyGroups(projectFile).First();
		var element = new XElement(
			XName.Get(
				name,
				projectFile
					.Root
					.GetDefaultNamespace()
					.NamespaceName));
		firstGroup.Add(element);
		return element;
	}
	
	public XElement CreateItemGroupElement(XDocument projectFile, string name)
	{
		var firstGroup = GetItemGroups(projectFile).First();
		var element = new XElement(
			XName.Get(
				name,
				projectFile
					.Root
					.GetDefaultNamespace()
					.NamespaceName));
		firstGroup.Add(element);
		return element;
	}

	public XElement CreateVersionElement(XDocument projectFile)
	{
		return CreatePropertyGroupElement(projectFile, "Version");
	}

	public XElement? GetPackageProjectUrlElement(XDocument projectFile)
	{
		return GetPropertyGroupElement(projectFile, "PackageProjectUrl");
	}

	public XElement CreatePackageProjectUrlElement(XDocument projectFile)
	{
		return CreatePropertyGroupElement(projectFile, "PackageProjectUrl");
	}

	public XElement? GetDescriptionElement(XDocument projectFile)
	{
		return GetPropertyGroupElement(projectFile, "Description");
	}

	public XElement CreateDescriptionElement(XDocument projectFile)
	{
		return CreatePropertyGroupElement(projectFile, "Description");
	}

	public XElement? GetPackageRepositoryTypeElement(XDocument projectFile)
	{
		return GetPropertyGroupElement(projectFile, "RepositoryType");
	}

	public XElement CreatePackageRepositoryTypeElement(XDocument projectFile)
	{
		return CreatePropertyGroupElement(projectFile, "RepositoryType");
	}

	public XElement? GetPackageReadmeFileElement(XDocument projectFile)
	{
		return GetPropertyGroupElement(projectFile, "PackageReadmeFile");
	}

	public XElement CreatePackageReadmeFileElement(XDocument projectFile)
	{
		return CreatePropertyGroupElement(projectFile, "PackageReadmeFile");
	}

	public XElement? GetPackageRepositoryUrlElement(XDocument projectFile)
	{
		return GetPropertyGroupElement(projectFile, "RepositoryUrl");
	}

	public XElement CreatePackageRepositoryUrlElement(XDocument projectFile)
	{
		return CreatePropertyGroupElement(projectFile, "RepositoryUrl");
	}

	private static XElement? GetPropertyGroupElement(XDocument projectFile, string name)
	{
		return GetPropertyGroups(projectFile)
			.SelectMany(x => x.Elements())
			.SingleOrDefault(x => x.Name.LocalName == name);
	}

	public XElement? GetVersionElement(XDocument projectFile)
	{
		return GetPropertyGroupElement(projectFile, "Version");
	}

	private static IEnumerable<XElement> GetItemGroups(XDocument projectFile)
	{
		return projectFile
			.Root
			.Elements()
			.Where(x => x.Name.LocalName == "ItemGroup");
	}

	private static IEnumerable<XElement> GetPropertyGroups(XDocument projectFile)
	{
		return projectFile
			.Root
			.Elements()
			.Where(x => x.Name.LocalName == "PropertyGroup");
	}

	public XElement GetOrCreatePropertyGroupElement(XDocument projectFile, string property)
	{
		return GetPropertyGroupElement(projectFile, property) ??
		       CreatePropertyGroupElement(projectFile, property);
	}
}