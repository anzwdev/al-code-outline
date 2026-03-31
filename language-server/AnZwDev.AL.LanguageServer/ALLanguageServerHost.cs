using AnZwDev.AL.LanguageServer.Modules.ProjectInformationProvider;
using AnZwDev.AL.LanguageServer.Modules.SymbolsSourceProvider;
using AnZwDev.AL.LanguageServer.Modules.SymbolsViewer;
using AnZwDev.AL.LanguageServer.Modules.SyntaxTreeSymbolsTreeViewProvider;
using AnZwDev.AL.LanguageServer.Modules.SyntaxTreeTreeViewProvider;
using AnZwDev.AL.LanguageServer.Modules.SyntaxTreeViewer;
using AnZwDev.AL.LanguageServer.Modules.WorkspaceChangeTracking;
using AnZwDev.AL.Workspaces;
using AnZwDev.LanguageServer;
using AnZwDev.System.IO;
using AnZwDev.System.Logging;
using AnZwDev.System.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AnZwDev.AL.LanguageServer
{
    public class ALLanguageServerHost : LanguageServerHost
    {

        public Workspace Workspace { get; }
        
        private readonly string _logFilePath;

        public ALLanguageServerHost()
        {
            Workspace = new Workspace(Services);

            _logFilePath = PathUtils.Combine(Assembly.GetExecutingAssembly(), "LanguageServer.log");
        }

        protected override void RegisterModules()
        {
            base.RegisterModules();

            RegisterModule(new WorkspaceChangeTrackingModule(this));
            RegisterModule(new ProjectInformationProviderModule(this));

            //viewers
            RegisterModule(new SymbolsViewerModule(this));
            RegisterModule(new SyntaxTreeViewerModule(this));

            //simple views or data content providers
            RegisterModule(new SyntaxTreeSymbolsTreeViewProviderModule(this));
            RegisterModule(new SyntaxTreeTreeViewProviderModule(this));
            RegisterModule(new SymbolsSourceProviderModule(this));
        }

        protected override void RegisterServices()
        {
            base.RegisterServices();

            Services.AddSingleton<ILogger>(new FlatFileLogger(_logFilePath));
            Services.AddSingleton(Workspace);
        }

    }
}
