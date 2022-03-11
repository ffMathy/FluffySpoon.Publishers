using FluffySpoon.Publisher.Local;
using System.Collections.Generic;

namespace FluffySpoon.Publisher.DotNet;

public interface IDotNetLocalPackage : ILocalPackage
{
	string ProjectFilePath { get; }
}