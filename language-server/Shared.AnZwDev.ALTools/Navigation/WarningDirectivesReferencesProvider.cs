using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Navigation
{

#if BC
    public class WarningDirectivesReferencesProvider : ReferencesProvider
    {

        public WarningDirectivesReferencesProvider(ALDevToolsServer server) : base(server)
        {
        }

        public override void FindReferences(SyntaxNode node, int position, List<DocumentRange> references)
        {
            var warningDirective = node.FindParentByKind(ConvertedSyntaxKind.PragmaWarningDirectiveTrivia) as PragmaWarningDirectiveTriviaSyntax;
            if (warningDirective != null)
            {
                var errorCode = warningDirective.FindErrorCodeAtPosition(position);
                if (errorCode != null)
                {
                    var ruleId = errorCode.ToString();
                    if (!String.IsNullOrWhiteSpace(ruleId))
                        CollectRuleReferences(ruleId, references);
                }
            }
        }

        private void CollectRuleReferences(string ruleId, List<DocumentRange> references)
        {
            for (int i = 0; i < Server.Workspace.Projects.Count; i++)
                CollectRuleReferences(ruleId, Server.Workspace.Projects[i], references);
        }

        private void CollectRuleReferences(string ruleId, ALProject project, List<DocumentRange> references)
        {
            for (int i = 0; i < project.Files.Count; i++)
                CollectRuleReferences(ruleId, project.Files[i], references);
        }

        private void CollectRuleReferences(string ruleId, ALProjectFile file, List<DocumentRange> references)
        {
            if (file.Directives != null)
                for (int i = 0; i < file.Directives.Count; i++)
                    if ((file.Directives[i] is ALAppPragmaWarningDirective warningDirective) && (warningDirective.ContainsRule(ruleId)))
                    {
                        var reference = CreateRuleReference(file, warningDirective);
                        if (reference != null)
                            references.Add(reference);
                    }
        }

        private DocumentRange CreateRuleReference(ALProjectFile file, ALAppPragmaWarningDirective warningDirective)
        {
            if ((String.IsNullOrWhiteSpace(file.FullPath)) ||
                (warningDirective.Range?.start == null) ||
                (warningDirective.Range?.end == null))
                return null;
            return new DocumentRange(file.FullPath, warningDirective.Range.start, warningDirective.Range.end);
        }

    }
#endif
}
