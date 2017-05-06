using System.Threading.Tasks;

namespace FluffySpoon.Publisher.Remote
{
  public interface IRemoteSourceControlRepository
  {
    string Name { get; }

    IRemoteSourceControlSystem System { get; }

    Task DownloadToDirectoryAsync(string folderPath);
  }
}