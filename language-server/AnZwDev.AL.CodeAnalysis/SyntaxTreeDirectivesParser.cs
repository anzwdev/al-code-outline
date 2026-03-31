using AnZwDev.AL.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.CodeAnalysis
{
    public class SyntaxTreeDirectivesParser
    {
        public List<string>? DisabledErrors { get; set; }

        public IList<DirectiveTriviaSyntax>? Directives { get; private set; }

        public void Parse(SyntaxNode syntaxNode)
        {
            this.Directives = syntaxNode.GetDirectives();
        }

        public bool GetErrorCodeStateAtPosition(int position, string errorCode)
        {
            bool state = (DisabledErrors == null) || (!DisabledErrors.Contains(errorCode));
            
            if (Directives == null)
                return state;

            foreach (var directiveTrivia in this.Directives)
            {
                if (directiveTrivia.Span.Start > position)
                    return state;

                if ((directiveTrivia is PragmaWarningDirectiveTriviaSyntax pragmaWarningDirectiveTrivia) && (pragmaWarningDirectiveTrivia.IsActive))
                {
                    var enabled = pragmaWarningDirectiveTrivia.DisableOrRestoreKeyword.Kind == SyntaxKind.RestoreKeyword;

                    var containsDirective =
                        (pragmaWarningDirectiveTrivia.ErrorCodes.Count == 0) ||
                        (pragmaWarningDirectiveTrivia.ErrorCodes.Any(p => (errorCode.Equals(ALLiteralParser.ParseName(p.ToString()), StringComparison.OrdinalIgnoreCase))));

                    if (containsDirective)
                        state = enabled;
                }
            }

            return state;
        }
    }
}
