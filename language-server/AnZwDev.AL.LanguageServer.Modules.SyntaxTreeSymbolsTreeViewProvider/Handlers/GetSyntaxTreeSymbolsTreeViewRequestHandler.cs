using AnZwDev.AL.LanguageServer.Modules.SyntaxTreeSymbolsTreeViewProvider.Contracts;
using AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews;
using AnZwDev.AL.Workspaces;
using AnZwDev.AL.Workspaces.AttachedData.SyntaxTreeSymbolsTreeViews;
using AnZwDev.AL.Workspaces.CodeAnalysis;
using AnZwDev.LanguageServer;
using AnZwDev.System.Logging;
using AnZwDev.System.ServiceModel;
using StreamJsonRpc;

namespace AnZwDev.AL.LanguageServer.Modules.SyntaxTreeSymbolsTreeViewProvider.Handlers
{
    public class GetSyntaxTreeSymbolsTreeViewRequestHandler : RequestHandler
    {

        public GetSyntaxTreeSymbolsTreeViewRequestHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("al/syntaxtreesymbolsview/gettreeview", UseSingleObjectParameterDeserialization = true)]
        public GetSyntaxTreeSymbolsTreeViewResponse GetSyntaxTreeSymbolsTreeVView(GetSyntaxTreeSymbolsTreeViewRequest parameters)
        {
            var response = new GetSyntaxTreeSymbolsTreeViewResponse()
            {
                Path = parameters.Path
            };

            try
            {
                if (!String.IsNullOrWhiteSpace(parameters.Path))
                    response.RootNode = this.Services.GetService<Workspace>()?
                        .Projects.FindByPath(parameters.Path)?
                        .Files.Find(parameters.Path)?
                        .AttachedData.Get(ProjectFileAttachedSyntaxTreeSymbolsTreeViewFactory.Instance).Get();
                else if (!String.IsNullOrWhiteSpace(parameters.Content))
                {
                    SyntaxTreeSymbolsTreeViewBuilder _builder = new SyntaxTreeSymbolsTreeViewBuilder();
                    response.RootNode = _builder.CreateView(parameters.Content);
                }
            }
            catch (Exception e)
            {
                Services.GetService<ILogger>()?.Log(e);
            }

            return response;
        }

    }
}
