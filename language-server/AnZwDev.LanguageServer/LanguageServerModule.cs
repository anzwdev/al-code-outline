using AnZwDev.System.ServiceModel;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.LanguageServer
{
    public class LanguageServerModule
    {

        public InstanceServiceProvider Services { get; }
        public LanguageServerHost LanguageServerHost { get; }

        public LanguageServerModule(LanguageServerHost languageServerHost) : 
            this(languageServerHost, languageServerHost.Services)
        {
        }

        public LanguageServerModule(LanguageServerHost languageServerHost, InstanceServiceProvider services)
        {
            LanguageServerHost = languageServerHost;
            Services = services;
        }

        internal void Initialize()
        {
            RegisterHandlers();
            RegisterServices();
        }

        protected virtual void RegisterHandlers()
        {
        }

        protected virtual void RegisterServices()
        {
        }

        protected void RegisterRequestHandler(RequestHandler handler)
        {
            LanguageServerHost.RegisterRequestHandler(handler);
        }

    }
}
