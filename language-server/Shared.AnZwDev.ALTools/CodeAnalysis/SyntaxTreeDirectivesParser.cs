using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Hover;
using AnZwDev.ALTools.Extensions;

namespace AnZwDev.ALTools.CodeAnalysis
{

#if BC

    internal class SyntaxTreeDirectivesParser
    {

        public List<string> DisabledErrors { get; set; }

        public IList<DirectiveTriviaSyntax> Directives { get; private set; }

        public void Parse(SyntaxNode syntaxNode)
        {
            this.Directives = syntaxNode.GetDirectives();
        }

        public bool GetErrorCodeStateAtPosition(int position, string errorCode)
        {
            bool state = (DisabledErrors ==  null) || (!DisabledErrors.Contains(errorCode));

            foreach (var directiveTrivia in this.Directives)
            {
                if (directiveTrivia.Span.Start > position)
                    return state;

                if ((directiveTrivia is PragmaWarningDirectiveTriviaSyntax pragmaWarningDirectiveTrivia) && (pragmaWarningDirectiveTrivia.IsActive))
                {
                    var enabled = pragmaWarningDirectiveTrivia.DisableOrRestoreKeyword.Kind.ConvertToLocalType() == ALSymbols.Internal.ConvertedSyntaxKind.RestoreKeyword;

                    var containsDirective = 
                        (pragmaWarningDirectiveTrivia.ErrorCodes.Count == 0) || 
                        (pragmaWarningDirectiveTrivia.ErrorCodes.Any(p => (errorCode.Equals(ALSyntaxHelper.DecodeName(p.ToString()), StringComparison.OrdinalIgnoreCase))));

                    if (containsDirective)
                        state = enabled;
                }
            }

            return state;
        }

    }

#endif

}
