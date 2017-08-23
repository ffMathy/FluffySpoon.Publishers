namespace FluffySpoon.Publisher
{
    class Settings : ISettings
    {
        public string ProjectPrefix { get; }

        public Settings(string repositoryPrefix)
        {
            ProjectPrefix = repositoryPrefix;
        }
    }
}
