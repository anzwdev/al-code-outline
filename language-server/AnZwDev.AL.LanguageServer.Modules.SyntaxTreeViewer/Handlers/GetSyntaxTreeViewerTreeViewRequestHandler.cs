using AnZwDev.AL.LanguageServer.Modules.SyntaxTreeViewer.Contracts;
using AnZwDev.AL.Workspaces;
using AnZwDev.AL.Workspaces.AttachedData.SyntaxTreeViewersStates;
using AnZwDev.LanguageServer;
using AnZwDev.System.Logging;
using AnZwDev.System.ServiceModel;
using StreamJsonRpc;

namespace AnZwDev.AL.LanguageServer.Modules.SyntaxTreeViewer.Handlers
{
    public class GetSyntaxTreeViewerTreeViewRequestHandler : RequestHandler
    {

        public GetSyntaxTreeViewerTreeViewRequestHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("al/syntaxtreeviewer/gettreeview", UseSingleObjectParameterDeserialization = true)]
        public GetSyntaxTreeViewerTreeViewResponse GetSyntaxTreeTreeView(GetSyntaxTreeViewerTreeViewRequest parameters)
        {
            var response = new GetSyntaxTreeViewerTreeViewResponse();

            try
            {
                if (!String.IsNullOrWhiteSpace(parameters.Path))
                    response.RootNode = this.Services.GetService<Workspace>()?
                        .Projects.FindByPath(parameters.Path)?
                        .Files.Find(parameters.Path)?
                        .AttachedData.Get(ProjectFileAttachedSyntaxTreeViewerStateFactory.Instance)
                        .Get(parameters.ViewMode);
            }
            catch (Exception e)
            {
                Services.GetService<ILogger>()?.Log(e);
            }

            return response;
        }

    }
}
