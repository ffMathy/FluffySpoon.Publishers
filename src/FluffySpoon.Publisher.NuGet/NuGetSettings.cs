namespace FluffySpoon.Publisher.NuGet
{
    class NuGetSettings : INuGetSettings
    {
        public string ApiKey { get; }

        public NuGetSettings(string apiKey)
        {
            ApiKey = apiKey;
        }
    }
}
