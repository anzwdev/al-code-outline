using System;
using StreamJsonRpc;
using AnZwDev.ALTools.Logging;
using AnZwDev.ALTools.Server.Handlers;
using AnZwDev.ALTools.Server.Handlers.ChangeTracking;
using AnZwDev.ALTools.Server.Handlers.SymbolsInformation;
using AnZwDev.ALTools.Server.Handlers.LanguageInformation;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server
{
    public class LanguageServerHost
    {

        public ALDevToolsServer ALDevToolsServer { get; }
        public JsonRpc JsonRpc { get; private set; }

        public LanguageServerHost(string extensionPath)
        {
            this.ALDevToolsServer = new ALDevToolsServer(extensionPath);
            this.JsonRpc = new JsonRpc(Console.OpenStandardOutput(), Console.OpenStandardInput());
            MessageLog.Writer = new MessageLogWriterImpl();
            InitializeMessageHandlers();
        }

        protected void InitializeMessageHandlers()
        {
            //request handlers
            this.RegisterRequestHandler(new ShutdownRequestHandler(this));

            //document symbols
            this.RegisterRequestHandler(new DocumentSymbolsRequestHandler(this));

            //symbols libraries
            this.RegisterRequestHandler(new AppPackageSymbolsRequestHandler(this));
            this.RegisterRequestHandler(new ProjectSymbolsRequestHandler(this));
            this.RegisterRequestHandler(new LibrarySymbolsDetailsRequestHandler(this));
            this.RegisterRequestHandler(new CloseSymbolsLibraryNotificationHandler(this));

            this.RegisterRequestHandler(new GetLibrarySymbolLocationRequestHandler(this));
            this.RegisterRequestHandler(new GetProjectSymbolLocationRequestHandler(this));

            this.RegisterRequestHandler(new GetALAppContentRequestHandler(this));

            //obsolete syntax tree analyzer for other vs code extensions
            this.RegisterRequestHandler(new GetSyntaxTreeRequestHandler(this));
            this.RegisterRequestHandler(new GetSyntaxTreeSymbolRequestHandler(this));
            this.RegisterRequestHandler(new CloseSyntaxTreeNotificationHandler(this));

            //syntax tree analyzer
            this.RegisterRequestHandler(new GetRawSyntaxTreeRequestHandler(this));
            this.RegisterRequestHandler(new GetRawSyntaxTreeSymbolRequestHandler(this));
            this.RegisterRequestHandler(new CloseRawSyntaxTreeNotificationHandler(this));

            this.RegisterRequestHandler(new GetFullSyntaxTreeRequestHandler(this));

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
            this.RegisterRequestHandler(new ExitNotificationHandler(this));

            //document tracking notification handlers
            this.RegisterRequestHandler(new WorkspaceFoldersChangeNotificationHandler(this));
            this.RegisterRequestHandler(new DocumentOpenNotificationHandler(this));
            this.RegisterRequestHandler(new DocumentContentChangeRequestHandler(this));
            this.RegisterRequestHandler(new DocumentCloseNotificationHandler(this));

            //this file change handlers are not used by vs code:
            //this.RegisterRequestHandler(new DocumentSaveNotificationHandler(this.ALDevToolsServer));
            //this.RegisterRequestHandler(new FileCreateNotificationHandler(this.ALDevToolsServer));
            //this.RegisterRequestHandler(new FileDeleteNotificationHandler(this.ALDevToolsServer));
            //this.RegisterRequestHandler(new FileRenameNotificationHandler(this.ALDevToolsServer));

            this.RegisterRequestHandler(new FileSystemFileCreateNotificationHandler(this));
            this.RegisterRequestHandler(new FileSystemFileDeleteNotificationHandler(this));
            this.RegisterRequestHandler(new FileSystemFileChangeNotificationHandler(this));

            this.RegisterRequestHandler(new ConfigurationChangeNotificationHandler(this));

            this.RegisterRequestHandler(new GetFileContentRequestHandler(this));

            //language handlers
            this.RegisterRequestHandler(new GetImagesRequestHandler(this));

            //other message handlers
            this.RegisterRequestHandler(new GetProjectSettingsRequestHandler(this));
        }

        protected void RegisterRequestHandler(RequestHandler handler)
        {
            this.JsonRpc.AddLocalRpcTarget(handler);
        }

        public void Stop()
        {
            this.JsonRpc.Dispose();
        }

        public async Task RunAsync()
        {
            try
            {
                this.JsonRpc.StartListening();
                await JsonRpc.Completion;
            }
            catch (Exception ex)
            {
                MessageLog.LogError(ex);
            }
        }

    }
}
