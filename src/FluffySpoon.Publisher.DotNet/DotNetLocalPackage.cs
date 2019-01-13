using System.Collections.Generic;
using FluffySpoon.Publisher.Local;

namespace FluffySpoon.Publisher.DotNet
{
	class DotNetLocalPackage : IDotNetLocalPackage
	{
		public string PublishName { get; set; }
		public string FolderPath { get; set; }
		public string ProjectFilePath { get; set; }
		public string Version { get; set; }
		public string PublishUrl { get; set; }

		public ILocalPackageProcessor Processor { get; set; }
	}
}