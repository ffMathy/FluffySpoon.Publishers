namespace FluffySpoon.Publisher.Local
{
  public interface ILocalPackage
  {
    string PublishName { get; }
    string FolderPath { get; }

    int Version { get; }
  }
}