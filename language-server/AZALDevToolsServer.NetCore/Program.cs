using AnZwDev.ALTools.Server;
using Microsoft.VisualStudio.Threading;
using System;

namespace AZALDevToolsServer.NetCore
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            LanguageServerHost host = new LanguageServerHost(args[0]);
            await host.RunAsync();
        }
    }
}
