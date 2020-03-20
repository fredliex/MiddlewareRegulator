using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WebApplication3
{
    /// <summary>Middleware 中繼資料</summary>
    [DebuggerDisplay("Type = {Type.FullName}")]
    public sealed class MiddlewareMetadata
    {
        /// <summary>委派</summary>
        public Func<RequestDelegate, RequestDelegate> Middleware { get; }

        /// <summary>Middleware 型別</summary>
        public Type Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MiddlewareMetadata"/> class.
        /// </summary>
        /// <param name="middleware">Middleware委派</param>
        internal MiddlewareMetadata(Type type, Func<RequestDelegate, RequestDelegate> middleware)
        {
            Middleware = middleware;
            Type = type;
        }
    }
}
