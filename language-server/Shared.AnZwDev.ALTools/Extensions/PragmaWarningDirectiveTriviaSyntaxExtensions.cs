using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Extensions
{

#if BC

    public static class PragmaWarningDirectiveTriviaSyntaxExtensions
    {

        public static CodeExpressionSyntax FindErrorCodeAtPosition(this PragmaWarningDirectiveTriviaSyntax warningDirective, int position)
        {
            if (warningDirective.ErrorCodes != null)
                for (int i = 0; i < warningDirective.ErrorCodes.Count; i++)
                    if ((warningDirective.ErrorCodes[i].Span.Start <= position) && (warningDirective.ErrorCodes[i].Span.End >= position))
                        return warningDirective.ErrorCodes[i];
            return null;
        }

    }

#endif

}
