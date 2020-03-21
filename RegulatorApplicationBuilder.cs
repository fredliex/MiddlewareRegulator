using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3
{
    public partial class RegulatorApplicationBuilder : List<MiddlewareMetadata>, IRegulatorApplicationBuilder
    {
        private readonly IApplicationBuilder instance;

        public RegulatorApplicationBuilder(IApplicationBuilder instance) => this.instance = instance;

        public IServiceProvider ApplicationServices
        { 
            get => instance.ApplicationServices; 
            set => instance.ApplicationServices = value; 
        }

        public IDictionary<string, object> Properties => instance.Properties;

        public IFeatureCollection ServerFeatures => instance.ServerFeatures;

        public IApplicationBuilder New() => new RegulatorApplicationBuilder(instance.New());

        public RequestDelegate Build()
        {
            foreach (var regulator in ApplicationServices.GetServices<MiddlewareRegulator>())
            {
                regulator.Regulate(this);
            }
            var app = instance.Build();
            foreach (var meta in this.AsEnumerable().Reverse())
            {
                app = meta.Middleware(app);
            }
            return app;
        }

        public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            Add(new MiddlewareMetadata(GetMiddlewareType(middleware), middleware));
            return this;
        }

        private static RequestDelegate ResolveMiddleware(RequestDelegate middleware)
            => _ => Task.FromResult(middleware.Target.GetType());

        private Type GetMiddlewareType(Func<RequestDelegate, RequestDelegate> middleware)
        {
            var app = middleware(context => null);
            app = ResolveMiddleware(middleware(context => null));
            return ((Task<Type>)app.Invoke(null)).Result;
        }
    }
}
