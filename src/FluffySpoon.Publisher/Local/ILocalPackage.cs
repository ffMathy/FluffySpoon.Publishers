namespace FluffySpoon.Publisher.Local
{
  public interface ILocalPackage
  {
    string PublishName { get; }
    string FolderPath { get; }
    string Version { get; }

    ILocalPackageProcessor Processor { get; }
  }
}