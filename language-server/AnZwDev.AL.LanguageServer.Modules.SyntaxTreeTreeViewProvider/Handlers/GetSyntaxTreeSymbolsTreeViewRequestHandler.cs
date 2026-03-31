using AnZwDev.AL.LanguageServer.Modules.SyntaxTreeTreeViewProvider.Contracts;
using AnZwDev.AL.Workspaces;
using AnZwDev.AL.Workspaces.AttachedData.SyntaxTreeSymbolsTreeViews;
using AnZwDev.LanguageServer;
using AnZwDev.System.Logging;
using AnZwDev.System.ServiceModel;
using StreamJsonRpc;

namespace AnZwDev.AL.LanguageServer.Modules.SyntaxTreeTreeViewProvider.Handlers
{
    public class GetSyntaxTreeTreeViewRequestHandler : RequestHandler
    {

        public GetSyntaxTreeTreeViewRequestHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("al/syntaxtreeview/gettreeview", UseSingleObjectParameterDeserialization = true)]
        public GetSyntaxTreeTreeViewResponse GetSyntaxTreeTreeView(GetSyntaxTreeTreeViewRequest parameters)
        {
            var response = new GetSyntaxTreeTreeViewResponse()
            {
                Path = parameters.Path
            };

            try
            {
                if (!String.IsNullOrWhiteSpace(parameters.Path))
                    response.RootNode = this.Services.GetService<Workspace>()?
                        .Projects.FindByPath(parameters.Path)?
                        .Files.Find(parameters.Path)?
                        .AttachedData.Get(ProjectFileAttachedSyntaxTreeTreeViewFactory.Instance).Get();
            }
            catch (Exception e)
            {
                Services.GetService<ILogger>()?.Log(e);
            }

            return response;
        }

    }
}
