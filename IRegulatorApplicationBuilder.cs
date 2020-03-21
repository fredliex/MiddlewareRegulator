using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3
{
    public interface IRegulatorApplicationBuilder : IList<MiddlewareMetadata>, IApplicationBuilder
    {
    }
}
