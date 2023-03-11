using AnZwDev.ALTools.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZALDevToolsServer.NetFramework
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
