﻿using FluffySpoon.Publisher.Remote;
using Microsoft.Extensions.DependencyInjection;
using Octokit;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluffySpoon.Publisher.NuGet
{
  public static class NuGetRegistrationExtensions
  {
    public static void AddNuGetProvider(this ServiceCollection services)
    {
      RegistrationExtensions.AddPackageSystem<NuGetRemotePackageSystem>();
      
      services.AddTransient<NuGetRemotePackageSystem>();
    }
  }
}