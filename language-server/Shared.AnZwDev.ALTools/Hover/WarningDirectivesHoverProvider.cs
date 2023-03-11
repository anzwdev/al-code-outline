using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Hover
{

#if BC

    public class WarningDirectivesHoverProvider : HoverProvider
    {

        public WarningDirectivesHoverProvider(ALDevToolsServer server) : base(server)
        {
        }

        public override string ProvideHover(SyntaxNode node, int position)
        {
            var warningDirective = node.FindParentByKind(ConvertedSyntaxKind.PragmaWarningDirectiveTrivia) as PragmaWarningDirectiveTriviaSyntax;
            if (warningDirective != null)
            {
                var errorCode = warningDirective.FindErrorCodeAtPosition(position);
                if (errorCode != null)
                {
                    var ruleId = errorCode.ToString();
                    if (!String.IsNullOrWhiteSpace(ruleId))
                    {
                        Server.CodeAnalyzersLibraries.LoadCodeAnalyzers(Server.Workspace);
                        var rule = Server.CodeAnalyzersLibraries.FindCachedRule(ruleId);
                        if (rule != null)
                            return "`" + rule.id + "`: " + rule.title;
                        return "`" + rule.id + "`";
                    }
                }
            }
            return null;
        }

    }

#endif
}
