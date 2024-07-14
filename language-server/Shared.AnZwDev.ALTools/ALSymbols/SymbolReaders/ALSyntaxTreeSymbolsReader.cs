using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Logging;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.SymbolReaders
{
    public class ALSyntaxTreeSymbolsReader : ALBaseSyntaxTreeSymbolsReader
    {

        public ALSyntaxTreeSymbolsReader()
        {
        }

        public override ALSyntaxTreeSymbol ProcessSourceCode(string source, ALProject project)
        {
            SyntaxTree syntaxTree = SyntaxTreeExtensions.SafeParseObjectText(source, project);

            if (syntaxTree != null)
            {
                SyntaxNode node = syntaxTree.GetRoot();
                if (node != null)
                    return ProcessSyntaxTreeNode(syntaxTree, node);
            }
            return null;
        }

        protected ALSyntaxTreeSymbol ProcessSyntaxTreeNode(SyntaxTree syntaxTree, SyntaxNode node)
        {
            ALSyntaxTreeSymbol symbolInfo = new ALSyntaxTreeSymbol();
            symbolInfo.kind = ALSymbolKind.SyntaxTreeNode;
            symbolInfo.name = node.Kind.ToString();
            symbolInfo.fullName = symbolInfo.name + " " + node.FullSpan.ToString();
            symbolInfo.syntaxTreeNode = node;
            symbolInfo.type = node.GetType().Name;

            if (node.ContainsDiagnostics)
                symbolInfo.containsDiagnostics = true;

            var lineSpan = syntaxTree.GetLineSpan(node.FullSpan);
            symbolInfo.range = new TextRange(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
                lineSpan.EndLinePosition.Line, lineSpan.EndLinePosition.Character);

            lineSpan = syntaxTree.GetLineSpan(node.Span);
            symbolInfo.selectionRange = new TextRange(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
                lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character);

            IEnumerable<SyntaxNode> list = node.ChildNodes();
            if (list != null)
            {
                foreach (SyntaxNode childNode in list)
                {
                    symbolInfo.AddChildSymbol(ProcessSyntaxTreeNode(syntaxTree, childNode));
                }
            }

            return symbolInfo;
        }

    }
}
