﻿using FluffySpoon.Publisher.DotNet;
using FluffySpoon.Publisher.Local;
using FluffySpoon.Publisher.Remote;
using System;
using System.Collections.Generic;
using System.IO;
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
      return package is IDotNetLocalPackage;
    }

    public async Task<bool> DoesPackageWithVersionExistAsync(ILocalPackage package)
    {
      var nugetPackage = package;
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
      Console.WriteLine("Upserting package " + package.PublishName + " version " + package.Version + " to remote package repository.");

      var packageFile = new FileInfo(
        Path.Combine(
          package.FolderPath,
          package.PublishName + "." + package.Version + ".nupkg"));
      if(!packageFile.Exists)
      {
        throw new InvalidOperationException("Can't find compiled package at " + packageFile);
      }
    }
  }
}