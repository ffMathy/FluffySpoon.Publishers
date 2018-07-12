using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FluffySpoon.Publisher.DotNet.Helpers
{
    class ProjectFileParser : IProjectFileParser
    {
        private static XElement CreateElement(XDocument projectFile, string name)
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

        public XElement CreateVersionElement(XDocument projectFile)
        {
            return CreateElement(projectFile, "Version");
        }

        public XElement GetPackageProjectUrlElement(XDocument projectFile)
        {
            return GetElement(projectFile, "PackageProjectUrl");
        }

        public XElement CreatePackageProjectUrlElement(XDocument projectFile)
        {
            return CreateElement(projectFile, "PackageProjectUrl");
        }

        public XElement GetDescriptionElement(XDocument projectFile)
        {
            return GetElement(projectFile, "Description");
        }

        public XElement CreateDescriptionElement(XDocument projectFile)
        {
            return CreateElement(projectFile, "Description");
		}

		public XElement GetPackageRepositoryTypeElement(XDocument projectFile)
		{
			return GetElement(projectFile, "RepositoryType");
		}

		public XElement CreatePackageRepositoryTypeElement(XDocument projectFile)
		{
			return CreateElement(projectFile, "RepositoryType");
		}

		public XElement GetPackageRepositoryUrlElement(XDocument projectFile)
		{
			return GetElement(projectFile, "RepositoryUrl");
		}

		public XElement CreatePackageRepositoryUrlElement(XDocument projectFile)
		{
			return CreateElement(projectFile, "RepositoryUrl");
		}

		private static XElement GetElement(XDocument projectFile, string name)
        {
            return GetPropertyGroups(projectFile)
                .SelectMany(x => x.Elements())
                .SingleOrDefault(x => x.Name.LocalName == name);
        }

        public XElement GetVersionElement(XDocument projectFile)
        {
            return GetElement(projectFile, "Version");
        }

        private static IEnumerable<XElement> GetPropertyGroups(XDocument projectFile)
        {
            return projectFile
              .Root
              .Elements()
              .Where(x => x.Name.LocalName == "PropertyGroup");
        }
	}
}
