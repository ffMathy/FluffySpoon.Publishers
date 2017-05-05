using Microsoft.Extensions.DependencyInjection;
using System;

namespace FluffySpoon.Publishers.Sample
{
  class Program
  {
    static void Main(string[] args)
    {
      var services = new ServiceCollection();
      services.AddPublishers();
    }
  }
}