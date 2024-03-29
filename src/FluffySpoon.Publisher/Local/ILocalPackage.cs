﻿using System.Collections.Generic;

namespace FluffySpoon.Publisher.Local;

public interface ILocalPackage
{
	string PublishName { get; }
	string FolderPath { get; }
	string Version { get; set; }
	string PublishUrl { get; set; }

	ILocalPackageProcessor Processor { get; }
}