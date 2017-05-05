namespace FluffySpoon.Publisher.GitHub
{
  internal interface IGitHubSourceControlRepository : IRemoteSourceControlRepository
  {
    string Owner { get; set; }
    new string Name { get; set; }
  }
}