namespace FluffySpoon.Publisher
{
  interface ILocalPackage
  {
    string PublishName { get; }
    string FolderPath { get; }
  }
}