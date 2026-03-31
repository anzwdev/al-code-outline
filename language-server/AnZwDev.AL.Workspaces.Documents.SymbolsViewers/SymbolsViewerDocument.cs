using AnZwDev.AL.Syntax;
using AnZwDev.AL.Symbols.Providers.AppPackages;
using AnZwDev.AL.Views.SymbolsTreeViews;
using System.Data;
using System.Xml.Linq;

namespace AnZwDev.AL.Workspaces.Documents.SymbolsViewers
{
    public class SymbolsViewerDocument : WorkspaceDocument
    {

        public SymbolsViewerSymbolsLoader SymbolsLoader { get; }
        public SymbolsTreeNode? FullViewRootNode { get; private set; }
        public SymbolsTreeNode? ObjectHeadersViewRootNode { get; private set; }
        private Dictionary<int, SymbolsTreeNode> _objectNodesByUid = new Dictionary<int, SymbolsTreeNode>();

        public SymbolsViewerDocument(Workspace workspace, SymbolsViewerSymbolsLoader symbolsLoader) : base(workspace)
        {
            SymbolsLoader = symbolsLoader;
        }

        public void Load()
        {
            Clear();

            FullViewRootNode = SymbolsLoader.Load();

            if (FullViewRootNode != null)
                ObjectHeadersViewRootNode = CreateObjectsHeadersTree(FullViewRootNode);
        }

        public void Clear()
        {
            _objectNodesByUid.Clear();
            FullViewRootNode = null;
            ObjectHeadersViewRootNode = null;
        }

        public SymbolsTreeNode? GetNode(int uid)
        {
            if (_objectNodesByUid.ContainsKey(uid))
                return _objectNodesByUid[uid];
            return null;
        }

        private SymbolsTreeNode CreateObjectsHeadersTree(SymbolsTreeNode fullTreeNode)
        {
            var node = new SymbolsTreeNode()
            {
                Uid = fullTreeNode.Uid,
                Id = fullTreeNode.Id,
                Name = fullTreeNode.Name,
                NamespaceName = fullTreeNode.NamespaceName,
                Usings = fullTreeNode.Usings,
                FullName = fullTreeNode.FullName,
                Kind = fullTreeNode.Kind,
                Subtype = fullTreeNode.Subtype,
                Access = fullTreeNode.Access,
                Extends = fullTreeNode.Extends,
                Source = fullTreeNode.Source,
                ChildSymbols = null,
                TreeNodeSource = fullTreeNode.TreeNodeSource
            };

            if (fullTreeNode.Kind.IsObjectTypeKind())
                _objectNodesByUid.Add(fullTreeNode.Uid, fullTreeNode);
            else if (fullTreeNode.ChildSymbols != null)
            {
                node.ChildSymbols = new List<SymbolsTreeNode>(fullTreeNode.ChildSymbols.Count);
                for (int i = 0; i < fullTreeNode.ChildSymbols.Count; i++)
                    node.AddChildSymbol(CreateObjectsHeadersTree(fullTreeNode.ChildSymbols[i]));
            }

            return node;
        }

    }
}
