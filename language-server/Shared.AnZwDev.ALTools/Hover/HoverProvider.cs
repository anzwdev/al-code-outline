using AnZwDev.ALTools.ALSymbols;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Hover
{
    public class HoverProvider
    {

        public ALDevToolsServer Server { get; }

        public HoverProvider(ALDevToolsServer server)
        {
            Server = server;
        }

        public virtual string ProvideHover(SyntaxNode node, int position)
        {
            return null;
        }

    }
}
