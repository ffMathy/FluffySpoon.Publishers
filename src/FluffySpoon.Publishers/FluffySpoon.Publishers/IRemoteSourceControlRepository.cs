using System.Threading.Tasks;

namespace FluffySpoon.Publishers
{
  public interface IRemoteSourceControlRepository
  {
    string Name { get; }

    Task DownloadToDirectoryAsync(string folderPath);
  }
}