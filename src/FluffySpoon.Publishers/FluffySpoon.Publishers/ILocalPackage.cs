namespace FluffySpoon.Publishers
{
  interface ILocalPackage
  {
    string PublishName { get; }
    string FolderPath { get; }
  }
}