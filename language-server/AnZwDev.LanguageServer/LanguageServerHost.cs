using AnZwDev.System.ServiceModel;
using AnZwDev.System.Logging;
using AnZwDev.LanguageServer.MessageHandlers;
using StreamJsonRpc;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using System.Xml.Serialization;

namespace AnZwDev.LanguageServer
{

    public class LanguageServerHost
    {

        public InstanceServiceProvider Services { get; }
        protected List<LanguageServerModule> Modules { get; }
        protected JsonRpc JsonRpc { get; }
        private bool _initialized = false;

        public LanguageServerHost()
        {
            Services = new InstanceServiceProvider();
            Modules = new List<LanguageServerModule>();
            JsonRpc = new JsonRpc(Console.OpenStandardOutput(), Console.OpenStandardInput());
        }

        private void Initialize()
        {
            if (!_initialized)
            {
                RegisterMessageHandlers();
                RegisterServices();
                RegisterModules();

                InitializeModules();

                _initialized = true;
            }
        }

        private void InitializeModules()
        {
            for (int i = 0; i < Modules.Count; i++)
                Modules[i].Initialize();
        }

        protected void RegisterModule(LanguageServerModule module)
        {
            this.Modules.Add(module);
        }

        internal void RegisterRequestHandler(RequestHandler handler)
        {
            this.JsonRpc.AddLocalRpcTarget(handler);
        }

        public virtual void Stop()
        {
            this.JsonRpc.Dispose();
        }

        public async Task RunAsync()
        {
            try
            {
                Initialize();
                this.JsonRpc.StartListening();
                await JsonRpc.Completion;
            }
            catch (Exception ex)
            {
                Services.GetService<ILogger>()?.Log(ex);
            }
        }

        protected virtual void RegisterModules()
        {
        }

        protected virtual void RegisterServices()
        {
            Services.AddSingleton<LanguageServerDocumentService>(new LanguageServerDocumentService());
        }

        protected virtual void RegisterMessageHandlers()
        {
            RegisterRequestHandler(new ShutdownRequestHandler(Services));
            RegisterRequestHandler(new ExitNotificationHandler(Services, this));
        }

        /*
                //request handlers
Core            this.RegisterRequestHandler(new ShutdownRequestHandler(this));

                //document symbols
                this.RegisterRequestHandler(new DocumentSymbolsRequestHandler(this));
                this.RegisterRequestHandler(new ProjectSymbolsRequestHandler(this));
                this.RegisterRequestHandler(new GetProjectSymbolLocationRequestHandler(this));

                //symbols libraries
AppViewer       this.RegisterRequestHandler(new AppPackageSymbolsRequestHandler(this));
AppViewer       this.RegisterRequestHandler(new LibrarySymbolsDetailsRequestHandler(this));
AppViewer       this.RegisterRequestHandler(new CloseSymbolsLibraryNotificationHandler(this));
AppViewer       this.RegisterRequestHandler(new GetLibrarySymbolLocationRequestHandler(this));
AppViewer       this.RegisterRequestHandler(new GetALAppContentRequestHandler(this));

                //obsolete syntax tree analyzer for other vs code extensions
SyntaxTree      this.RegisterRequestHandler(new GetSyntaxTreeRequestHandler(this));
SyntaxTree      this.RegisterRequestHandler(new GetSyntaxTreeSymbolRequestHandler(this));
SyntaxTree      this.RegisterRequestHandler(new CloseSyntaxTreeNotificationHandler(this));

                //syntax tree analyzer
SyntaxTree      this.RegisterRequestHandler(new GetRawSyntaxTreeRequestHandler(this));
SyntaxTree      this.RegisterRequestHandler(new GetRawSyntaxTreeSymbolRequestHandler(this));
SyntaxTree      this.RegisterRequestHandler(new CloseRawSyntaxTreeNotificationHandler(this));

SyntaxTree      this.RegisterRequestHandler(new GetFullSyntaxTreeRequestHandler(this));

                //code analyzers
                this.RegisterRequestHandler(new GetCodeAnalyzersRulesRequestHandler(this));
                this.RegisterRequestHandler(new FindDuplicateCodeRequestHandler(this));

                //code transformations
                this.RegisterRequestHandler(new WorkspaceCommandRequestHandler(this));
                this.RegisterRequestHandler(new CollectWorkspaceCommandCodeActionsRequestHandler(this));

                //symbols information
                this.RegisterRequestHandler(new GetObjectsListRequestHandler(this));
                this.RegisterRequestHandler(new GetTablesListRequestHandler(this));
                this.RegisterRequestHandler(new GetTableFieldsListRequestHandler(this));
                this.RegisterRequestHandler(new GetCodeunitsListRequestHandler(this));
                this.RegisterRequestHandler(new GetCodeunitMethodsListRequestHandler(this));
                this.RegisterRequestHandler(new GetInterfacesListRequestHandler(this));
                this.RegisterRequestHandler(new GetInterfaceMethodsListRequestHandler(this));
                this.RegisterRequestHandler(new GetEnumsListRequestHandler(this));
                this.RegisterRequestHandler(new GetReportsListRequestHandler(this));
                this.RegisterRequestHandler(new GetQueriesListRequestHandler(this));
                this.RegisterRequestHandler(new GetXmlPortsListRequestHandler(this));
                this.RegisterRequestHandler(new GetPagesListRequestHandler(this));
                this.RegisterRequestHandler(new GetPageDetailsRequestHandler(this));
                this.RegisterRequestHandler(new GetPageFieldsAvailableToolTipsRequestHandler(this));
                this.RegisterRequestHandler(new GetXmlPortTableElementDetailsRequestHandler(this));
                this.RegisterRequestHandler(new GetReportDataItemDetailsRequestHandler(this));
                this.RegisterRequestHandler(new GetQueryDataItemDetailsRequestHandler(this));
                this.RegisterRequestHandler(new GetPermissionSetsRequestHandler(this));
                this.RegisterRequestHandler(new GetReportDetailsRequestHandler(this));
                this.RegisterRequestHandler(new GetDependenciesListRequestHandler(this));
                this.RegisterRequestHandler(new GetWarningDirectivesRequestHandler(this));
                this.RegisterRequestHandler(new GetNewFileRequiredInterfacesHandler(this));

                //next available object id
                this.RegisterRequestHandler(new GetNextObjectIdRequestHandler(this));

                //code completion, hover, references
                this.RegisterRequestHandler(new CodeCompletionRequestHandler(this));
                this.RegisterRequestHandler(new HoverRequestHandler(this));
                this.RegisterRequestHandler(new ReferencesRequestHandler(this));

                //standard notification handlers
Core            this.RegisterRequestHandler(new ExitNotificationHandler(this));

                //document tracking notification handlers
Workspaces      this.RegisterRequestHandler(new WorkspaceFoldersChangeNotificationHandler(this));
Workspaces      this.RegisterRequestHandler(new DocumentOpenNotificationHandler(this));
Workspaces      this.RegisterRequestHandler(new DocumentContentChangeRequestHandler(this));
Workspaces      this.RegisterRequestHandler(new DocumentCloseNotificationHandler(this));

                //this file change handlers are not used by vs code:
                //this.RegisterRequestHandler(new DocumentSaveNotificationHandler(this.ALDevToolsServer));
                //this.RegisterRequestHandler(new FileCreateNotificationHandler(this.ALDevToolsServer));
                //this.RegisterRequestHandler(new FileDeleteNotificationHandler(this.ALDevToolsServer));
                //this.RegisterRequestHandler(new FileRenameNotificationHandler(this.ALDevToolsServer));

Workspaces      this.RegisterRequestHandler(new FileSystemFileCreateNotificationHandler(this));
Workspaces      this.RegisterRequestHandler(new FileSystemFileDeleteNotificationHandler(this));
Workspaces      this.RegisterRequestHandler(new FileSystemFileChangeNotificationHandler(this));

Workspaces      this.RegisterRequestHandler(new ConfigurationChangeNotificationHandler(this));

                this.RegisterRequestHandler(new GetFileContentRequestHandler(this));

                //language handlers
                this.RegisterRequestHandler(new GetImagesRequestHandler(this));

                //other message handlers
Workspaces      this.RegisterRequestHandler(new GetProjectSettingsRequestHandler(this));

        */

    }
}
