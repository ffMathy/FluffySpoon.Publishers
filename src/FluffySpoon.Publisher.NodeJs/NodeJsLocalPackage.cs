using FluffySpoon.Publisher.Local;

namespace FluffySpoon.Publisher.NodeJs;

class NodeJsLocalPackage : INodeJsLocalPackage
{
	public string PublishName { get; set; }
	public string FolderPath { get; set; }
	public string PackageJsonFilePath { get; set; }
	public string Version { get; set; }
	public string PublishUrl { get; set; }

	public ILocalPackageProcessor Processor { get; set; }
}