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
    public partial class PlatformApplicationBuilder : IApplicationBuilder
    {
        private readonly IApplicationBuilder instance;
        private readonly MiddlewareMetadataCollection metadatas;
        private readonly Func<RequestDelegate, RequestDelegate> typeResolverMiddleware;

        public PlatformApplicationBuilder(IApplicationBuilder instance)
        {
            this.instance = instance;
            metadatas = new MiddlewareMetadataCollection(this);

            // init probeDelegate
            this.UseMiddleware<MiddlewareTypeResolverMiddlareware>();
            typeResolverMiddleware = metadatas[0].Middleware;
            metadatas.Clear();
        }

        public IServiceProvider ApplicationServices
        { 
            get => instance.ApplicationServices; 
            set => instance.ApplicationServices = value; 
        }

        public IDictionary<string, object> Properties => instance.Properties;

        public IFeatureCollection ServerFeatures => instance.ServerFeatures;

        public IApplicationBuilder New() => new PlatformApplicationBuilder(instance.New());

        public RequestDelegate Build()
        {
            foreach (var regulator in ApplicationServices.GetServices<MiddlewareRegulator>())
            {
                regulator.Regulate(this, metadatas);
            }
            var app = instance.Build();
            foreach (var meta in metadatas.AsEnumerable().Reverse())
            {
                app = meta.Middleware(app);
            }
            return app;
        }

        public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            metadatas.Add(metadatas.CreateMetadata(middleware));
            return this;
        }

        internal Type GetMiddlewareType(Func<RequestDelegate, RequestDelegate> middleware)
        {
            if (typeResolverMiddleware == null) return null;
            var app = middleware(context => null);
            app = typeResolverMiddleware(middleware(context => null));
            return ((Task<Type>)app.Invoke(null)).Result;
        }
    }
}
