namespace FluffySpoon.Publisher
{
  public interface ILocalPackage
  {
    string PublishName { get; }
    string FolderPath { get; }

    int Version { get; }
  }
}