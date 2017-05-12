using System;
using System.Collections.Generic;
using System.Text;

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
