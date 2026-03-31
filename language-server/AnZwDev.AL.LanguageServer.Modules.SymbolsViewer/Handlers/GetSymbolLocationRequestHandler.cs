using AnZwDev.AL.LanguageServer.Modules.SymbolsSourceProvider.Contracts.Location;
using AnZwDev.AL.LanguageServer.Modules.SymbolsViewer.Contracts;
using AnZwDev.AL.Symbols;
using AnZwDev.AL.Views.SymbolsTreeViews;
using AnZwDev.AL.Workspaces.Documents.SymbolsViewers;
using AnZwDev.LanguageServer;
using AnZwDev.System.ServiceModel;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.SymbolsViewer.Handlers
{
    internal class GetSymbolLocationRequestHandler : RequestHandler
    {
        public GetSymbolLocationRequestHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("al/symbolsviewer/getsymbollocation", UseSingleObjectParameterDeserialization = true)]
        public GetSymbolLocationResponse GetObjectLocation(GetSymbolLocationRequest parameters)
        {
            var document = this.Services
                .GetService<SymbolsViewerDocumentStore>()?
                .Get(parameters.DocumentUid);
            var node = document?.GetNode(parameters.ObjectUid);

            if ((document == null) || (node == null))
                return new GetSymbolLocationResponse();

            return new GetSymbolLocationResponse()
            {
                Location = FindNodeLocation(node, parameters.DirectAppFileAccess)
            };
        }

        private SPSymbolLocation? FindNodeLocation(SymbolsTreeNode node, bool directAppFileAccess)
        {
            var applicationSymbol = FindTreeNodeSource<ApplicationSymbol>(node);
            var objectSymbol = FindTreeNodeSource<ObjectSymbol>(node);
            return SPSymbolLocationFactory.BuildLocation(applicationSymbol, objectSymbol, directAppFileAccess);
        }

        private T? FindTreeNodeSource<T>(SymbolsTreeNode? node) where T : Symbol
        {
            while ((node != null) && ((node.TreeNodeSource == null) || (!(node.TreeNodeSource is T))))
                node = node.ParentSymbol;
            if (node?.TreeNodeSource == null)
                return null;
            return node.TreeNodeSource as T;
        }

    }
}
