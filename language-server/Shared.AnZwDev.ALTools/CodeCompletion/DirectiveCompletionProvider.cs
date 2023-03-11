using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeCompletion
{

#if BC

    public class DirectiveCompletionProvider : CodeCompletionProvider
    {

        public DirectiveCompletionProvider(ALDevToolsServer server) : base(server, "directives")
        {
        }

        public override void CollectCompletionItems(ALProject project, SyntaxTree syntaxTree, SyntaxNode syntaxNode, int position, List<CodeCompletionItem> completionItems)
        {
            var directiveSyntax = syntaxTree.GetRoot()?
                .GetFirstDirective(p => (p.FullSpan.Start <= position) && (p.FullSpan.End >= position));
            if ((directiveSyntax != null) &&
                (directiveSyntax is PragmaWarningDirectiveTriviaSyntax warningSyntax) && 
                (warningSyntax.DisableOrRestoreKeyword != null) && 
                (warningSyntax.DisableOrRestoreKeyword.Span.End < position))
                AddRules(project, completionItems);
        }

        private void AddRules(ALProject project, List<CodeCompletionItem> completionItems)
        {
            AddRules("${CodeCop}", completionItems);
            AddRules("${AppSourceCop}", completionItems);
            AddRules("${PerTenantExtensionCop}", completionItems);
            AddRules("${UICop}", completionItems);
        }

        private void AddRules(string analyzerName, List<CodeCompletionItem> completionItems)
        {
            var rulesList = this.Server.CodeAnalyzersLibraries.GetCodeAnalyzersLibrary(analyzerName);
            if (rulesList?.Rules != null)
                for (int i = 0; i < rulesList.Rules.Count; i++)
                    completionItems.Add(CreateCodeCompletionItem(rulesList.Rules[i].id, rulesList.Rules[i].title));
        }

        private CodeCompletionItem CreateCodeCompletionItem(string ruleId, string ruleDescription)
        {
            var item = new CodeCompletionItem(ruleId, CompletionItemKind.Value);
            item.detail = ruleDescription;
            return item;
        }

    }

#endif

}
