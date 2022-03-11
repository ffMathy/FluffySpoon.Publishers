using System.Collections.Generic;
using System.Threading.Tasks;
using FluffySpoon.Publisher.Remote;

namespace FluffySpoon.Publisher.Local;

public interface ILocalPackageProcessor
{
    Task<IReadOnlyCollection<ILocalPackage>> ScanForPackagesInDirectoryAsync(string folderPath);
    Task BuildPackageAsync(
        ILocalPackage package, 
        IRemoteSourceControlRepository repository);
}