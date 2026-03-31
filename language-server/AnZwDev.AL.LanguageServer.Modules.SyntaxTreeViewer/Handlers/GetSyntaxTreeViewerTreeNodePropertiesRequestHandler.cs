using AnZwDev.AL.LanguageServer.Modules.SyntaxTreeViewer.Contracts;
using AnZwDev.AL.Workspaces;
using AnZwDev.AL.Workspaces.AttachedData.SyntaxTreeViewersStates;
using AnZwDev.LanguageServer;
using AnZwDev.System.Logging;
using AnZwDev.System.ServiceModel;
using StreamJsonRpc;

namespace AnZwDev.AL.LanguageServer.Modules.SyntaxTreeViewer.Handlers
{
    internal class GetSyntaxTreeViewerTreeNodePropertiesRequestHandler : RequestHandler
    {

        public GetSyntaxTreeViewerTreeNodePropertiesRequestHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("al/syntaxtreeviewer/gettreenodeproperties", UseSingleObjectParameterDeserialization = true)]
        public GetSyntaxTreeViewerTreeNodePropertiesResponse GetNodeProperties(GetSyntaxTreeViewerTreeNodePropertiesRequest parameters)
        {
            var response = new GetSyntaxTreeViewerTreeNodePropertiesResponse();

            try
            {
                if ((!String.IsNullOrWhiteSpace(parameters.Path)) && (!String.IsNullOrWhiteSpace(parameters.Uid)))
                    response.Properties = this.Services.GetService<Workspace>()?
                        .Projects.FindByPath(parameters.Path)?
                        .Files.Find(parameters.Path)?
                        .AttachedData.Get(ProjectFileAttachedSyntaxTreeViewerStateFactory.Instance)
                        .GetProperties(parameters.Uid);
            }
            catch (Exception e)
            {
                Services.GetService<ILogger>()?.Log(e);
            }

            return response;
        }

    }
}
