using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Logging;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.ALSymbols.SymbolReaders
{
    public class ALSyntaxTreeSymbolsReader
    {

        public ALSyntaxTreeSymbolsReader()
        {
        }

        public ALSyntaxTreeSymbol ProcessSourceFile(string path)
        {
            string sourceCode;
            try
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(path);
                sourceCode = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();

                return ProcessSourceCode(sourceCode);
            }
            catch (Exception e)
            {
                MessageLog.LogError(e);
                return new ALSyntaxTreeSymbol(ALSymbolKind.Undefined, "LangServer Error: " + e.Message);
            }
        }



        public ALSyntaxTreeSymbol ProcessSourceCode(string source)
        {
            SyntaxTree syntaxTree = SyntaxTreeExtensions.SafeParseObjectText(source);

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
            symbolInfo.range = new Range(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
                lineSpan.EndLinePosition.Line, lineSpan.EndLinePosition.Character);

            lineSpan = syntaxTree.GetLineSpan(node.Span);
            symbolInfo.selectionRange = new Range(lineSpan.StartLinePosition.Line, lineSpan.StartLinePosition.Character,
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
