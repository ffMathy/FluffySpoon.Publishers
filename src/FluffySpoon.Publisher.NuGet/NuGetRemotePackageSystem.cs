using FluffySpoon.Publisher.Local;
using FluffySpoon.Publisher.NuGet.DotNet;
using FluffySpoon.Publisher.Remote;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FluffySpoon.Publisher.NuGet
{
  class NuGetRemotePackageSystem : IRemotePackageSystem
  {

    public bool CanPublishPackage(ILocalPackage package)
    {
      return package is NuGetLocalPackage;
    }

    public async Task<bool> DoesPackageWithVersionExistAsync(ILocalPackage package)
    {
      var nugetPackage = (NuGetLocalPackage)package;
      using (var client = new HttpClient())
      {
        var response = await client.GetAsync($"https://www.nuget.org/packages/{package.PublishName}/{package.Version}");
        if(response.StatusCode != HttpStatusCode.NotFound && response.IsSuccessStatusCode)
        {
          return true;
        }
      }

      return false;
    }

    public async Task UpsertPackageAsync(ILocalPackage package)
    {
    }
  }
}
