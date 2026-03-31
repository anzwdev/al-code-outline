using AnZwDev.AL.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.SourceCode.SymbolsCompilers
{
    internal static class CodeMemberSymbolCompiler
    {

        public static (List<MethodSymbol>, List<GlobalVariableDeclarationSymbol>, List<EventSymbol>?) Compile(SyntaxList<MemberSyntax> syntaxList)
        {
            var methods = new List<MethodSymbol>();
            var variables = new List<GlobalVariableDeclarationSymbol>();
            List<EventSymbol>? events = null;

            for (int i = 0; i < syntaxList.Count; i++)
            {
                switch (syntaxList[i])
                {
                    case MethodDeclarationSyntax methodSyntax:
                        var method = MethodSymbolCompiler.Compile(methodSyntax);
                        if (method != null)
                            methods.Add(method);
                        break;
                    case GlobalVarSectionSyntax globalVarSection:
                        GlobalVariableDeclarationSymbolCompiler.Compile(globalVarSection, variables);
                        break;
                    case EventDeclarationSyntax eventSyntax:
                        var eventSymbol = EventSymbolCompiler.Compile(eventSyntax);
                        if (eventSymbol != null)
                        {
                            if (events == null)
                                events = new List<EventSymbol>();
                            events.Add(eventSymbol);
                        }
                        break;
                    case TriggerDeclarationSyntax triggerSyntax:
                        break;
                }
            }

            return (methods, variables, events);
        }

    }
}
