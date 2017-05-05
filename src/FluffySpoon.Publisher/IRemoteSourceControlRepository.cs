using System.Threading.Tasks;

namespace FluffySpoon.Publisher
{
  public interface IRemoteSourceControlRepository
  {
    string Name { get; }

    Task DownloadToDirectoryAsync(string folderPath);
  }
}