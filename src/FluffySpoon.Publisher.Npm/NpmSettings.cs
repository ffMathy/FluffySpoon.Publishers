namespace FluffySpoon.Publisher.NuGet
{
    class NpmSettings : INpmSettings
    {
        public string AuthToken { get; }

        public NpmSettings(string authToken)
        {
            AuthToken = authToken;
        }
    }
}
