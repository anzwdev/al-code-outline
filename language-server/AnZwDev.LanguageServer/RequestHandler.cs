using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.LanguageServer
{
    public class RequestHandler
    {

        public IServiceProvider Services { get; }

        public RequestHandler(IServiceProvider services)
        {
            Services = services;
        }

    }
}
