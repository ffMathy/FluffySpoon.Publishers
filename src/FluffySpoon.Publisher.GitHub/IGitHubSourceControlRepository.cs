using FluffySpoon.Publisher.Remote;

namespace FluffySpoon.Publisher.GitHub
{
    internal interface IGitHubSourceControlRepository : IRemoteSourceControlRepository
    {
        string Owner { get; set; }
        new string Name { get; set; }
        string Summary { get; set; }

        new IRemoteSourceControlSystem System { get; set; }
    }
}