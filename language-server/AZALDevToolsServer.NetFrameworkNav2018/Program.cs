/************************************************************************
 *                                                                      *
 * Legacy version of the language server maintained to support Nav 2018 *
 *                                                                      *
 ************************************************************************/
using AnZwDev.ALTools.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZALDevToolsServer.NetFrameworkNav2018
{
    class Program
    {

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            LanguageServerHost host = new LanguageServerHost(args[0]);
            await host.RunAsync();
        }

        /*
        static void Main(string[] args)
        {
            LanguageServerHost host = new LanguageServerHost(args[0]);
            host.RunAsync().Wait();
        }
        */
    }
}
