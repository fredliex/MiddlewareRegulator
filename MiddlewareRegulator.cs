using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace WebApplication3
{
    public class MiddlewareRegulator
    {
        private readonly Action<IRegulatorApplicationBuilder> regulateAction;

        public MiddlewareRegulator(Action<IRegulatorApplicationBuilder> regulateAction)
            => this.regulateAction = regulateAction;

        public void Regulate(IRegulatorApplicationBuilder regulatorApplicationBuilder)
            => regulateAction(regulatorApplicationBuilder);
    }
}
