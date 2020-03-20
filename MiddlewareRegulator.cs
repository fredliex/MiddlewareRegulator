using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace WebApplication3
{
    public class MiddlewareRegulator
    {
        private readonly Action<IApplicationBuilder, MiddlewareMetadataCollection> regulateAction;

        public MiddlewareRegulator(Action<IApplicationBuilder, MiddlewareMetadataCollection> regulateAction)
            => this.regulateAction = regulateAction;

        public void Regulate(IApplicationBuilder applicationBuilder, MiddlewareMetadataCollection middlewares)
            => regulateAction(applicationBuilder, middlewares);
    }
}
