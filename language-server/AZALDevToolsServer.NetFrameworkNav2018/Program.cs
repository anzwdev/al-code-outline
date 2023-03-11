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
        static void Main(string[] args)
        {
            ALDevToolsServerHost host = new ALDevToolsServerHost(args[0]);
            host.Initialize();
            host.Run();
        }
    }
}
