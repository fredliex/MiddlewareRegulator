using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3
{
    public sealed class MiddlewareMetadataCollection : List<MiddlewareMetadata>
    {
        private readonly PlatformApplicationBuilder applicationBuilder;

        internal MiddlewareMetadataCollection(PlatformApplicationBuilder applicationBuilder)
            => this.applicationBuilder = applicationBuilder;

        public MiddlewareMetadata CreateMetadata(Func<RequestDelegate, RequestDelegate> middleware)
            => new MiddlewareMetadata(applicationBuilder.GetMiddlewareType(middleware), middleware);
    }
}
