using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3
{
    public static class MiddlewareRegulatorExtensions
    {
        public static IServiceCollection AddMiddlewareRegulator(this IServiceCollection services, Action<IRegulatorApplicationBuilder> regulateAction)
        {
            services.EnsureRegulatorApplicationBuilderFactory();
            return services.AddTransient(sp => new MiddlewareRegulator(regulateAction));
        }
            
        private static void EnsureRegulatorApplicationBuilderFactory(this IServiceCollection services)
        {
            var markDescriptor = services.FirstOrDefault(n => n.ServiceType == typeof(RegulatorApplicationBuilderFactory));
            if (markDescriptor != null) return;

            var oriDescriptor = services.LastOrDefault(n => n.ServiceType == typeof(IApplicationBuilderFactory)) ??
                throw new InvalidOperationException($"找不到{typeof(IApplicationBuilderFactory)}服務注入。");
            var instanceFactory =
                oriDescriptor.ImplementationInstance != null ? _ => oriDescriptor.ImplementationInstance :
                oriDescriptor.ImplementationFactory ?? (sp => ActivatorUtilities.CreateInstance(sp, oriDescriptor.ImplementationType));
            RegulatorApplicationBuilderFactory impFactory(IServiceProvider sp)
                => new RegulatorApplicationBuilderFactory((IApplicationBuilderFactory)instanceFactory(sp));
            var newDescriptor = ServiceDescriptor.Describe(typeof(IApplicationBuilderFactory), impFactory, oriDescriptor.Lifetime);

            services.Add(newDescriptor);
            services.AddTransient(impFactory);
        }

        public static MiddlewareMetadata Find<TMiddleware>(this IEnumerable<MiddlewareMetadata> middlewares)
            => middlewares.FirstOrDefault(n => n.Type == typeof(TMiddleware));
    }
}