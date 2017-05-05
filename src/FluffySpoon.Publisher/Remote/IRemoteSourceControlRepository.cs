using System.Threading.Tasks;

namespace FluffySpoon.Publisher.Remote
{
  public interface IRemoteSourceControlRepository
  {
    string Name { get; }

    Task DownloadToDirectoryAsync(string folderPath);
  }
}