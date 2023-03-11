using AnZwDev.ALTools.Server;
using System;

namespace AZALDevToolsServer.NetCore
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
