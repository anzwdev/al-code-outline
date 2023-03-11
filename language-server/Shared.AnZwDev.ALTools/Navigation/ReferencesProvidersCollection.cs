using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Hover;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Navigation
{
    public class ReferencesProvidersCollection
    {
        public ALDevToolsServer Server { get; }
        private List<ReferencesProvider> _referencesProviders = new List<ReferencesProvider>();

        public ReferencesProvidersCollection(ALDevToolsServer server)
        {
            Server = server;
            CreateReferencesProviders();
        }

        private void CreateReferencesProviders()
        {
#if BC
            _referencesProviders.Add(new WarningDirectivesReferencesProvider(Server));
#endif
        }

        public List<DocumentRange> FindReferences(bool activeDocument, string source, Position linePosition)
        {
            var syntaxTree = Server.Workspace.GetSyntaxTree(activeDocument, source);

            if (syntaxTree != null)
            {
                var position = syntaxTree.GetText().Lines.GetPosition(new LinePosition(linePosition.line, linePosition.character));
                return FindReferences(syntaxTree, position);
            }

            return null;
        }

        private List<DocumentRange> FindReferences(SyntaxTree syntaxTree, int position)
        {
            var node = syntaxTree.GetRoot()?.FindNodeOnLeftOfPosition(position);
            if (node != null)
            {
                List<DocumentRange> references = new List<DocumentRange>();
                for (int i = 0; i < _referencesProviders.Count; i++)
                    _referencesProviders[i].FindReferences(node, position, references);
                return references;
            }
            return null;
        }


    }
}
