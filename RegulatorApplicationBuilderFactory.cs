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
    public class RegulatorApplicationBuilderFactory : IApplicationBuilderFactory
    {
        private readonly IApplicationBuilderFactory instance;

        public RegulatorApplicationBuilderFactory(IApplicationBuilderFactory instance)
            => this.instance = instance;

        public IApplicationBuilder CreateBuilder(IFeatureCollection serverFeatures)
            => new RegulatorApplicationBuilder(instance.CreateBuilder(serverFeatures));
    }
}
