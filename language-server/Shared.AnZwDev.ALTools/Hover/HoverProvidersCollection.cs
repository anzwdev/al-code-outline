using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeCompletion;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Hover
{
    public class HoverProvidersCollection
    {

        public ALDevToolsServer Server { get; }
        private List<HoverProvider> _hoverProviders = new List<HoverProvider>();

        public HoverProvidersCollection(ALDevToolsServer server)
        {
            Server = server;
            CreateHoverProviders();
        }

        private void CreateHoverProviders()
        {
#if BC
            _hoverProviders.Add(new WarningDirectivesHoverProvider(Server));
#endif
        }

        public string ProvideHover(bool activeDocument, string source, Position linePosition)
        {
            var syntaxTree = Server.Workspace.GetSyntaxTree(activeDocument, source);

            if (syntaxTree != null)
            {
                var position = syntaxTree.GetText().Lines.GetPosition(new LinePosition(linePosition.line, linePosition.character));
                return ProvideHover(syntaxTree, position);
            }

            return null;
        }

        private string ProvideHover(SyntaxTree syntaxTree, int position)
        {
            var node = syntaxTree.GetRoot()?.FindNodeOnLeftOfPosition(position);
            if (node != null)
            {
                for (int i = 0; i < _hoverProviders.Count; i++)
                {
                    var value = _hoverProviders[i].ProvideHover(node, position);
                    if (!String.IsNullOrWhiteSpace(value))
                        return value;
                }
            }
            return null;
        }

    }
}
