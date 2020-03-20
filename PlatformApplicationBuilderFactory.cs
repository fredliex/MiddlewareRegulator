using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication3
{
    public class PlatformApplicationBuilderFactory : IApplicationBuilderFactory
    {
        private readonly IApplicationBuilderFactory instance;

        public PlatformApplicationBuilderFactory(IApplicationBuilderFactory instance)
            => this.instance = instance;

        public IApplicationBuilder CreateBuilder(IFeatureCollection serverFeatures)
            => new PlatformApplicationBuilder(instance.CreateBuilder(serverFeatures));
    }
}
