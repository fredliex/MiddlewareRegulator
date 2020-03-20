using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace WebApplication3
{
    internal sealed class MiddlewareTypeResolverMiddlareware
    {
        private readonly Type middlewareType;
        public MiddlewareTypeResolverMiddlareware(RequestDelegate next) => middlewareType = next.Target.GetType();
        public Task Invoke(HttpContext context) => Task.FromResult(middlewareType);
    }
}
