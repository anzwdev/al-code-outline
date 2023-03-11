using AnZwDev.ALTools.ALSymbols;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Navigation
{
    public class ReferencesProvider
    {

        public ALDevToolsServer Server { get; }

        public ReferencesProvider(ALDevToolsServer server)
        {
            Server = server;
        }

        public virtual void FindReferences(SyntaxNode node, int position, List<DocumentRange> references)
        {
        }

    }
}
