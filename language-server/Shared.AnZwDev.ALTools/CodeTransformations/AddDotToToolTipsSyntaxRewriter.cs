using AnZwDev.ALTools.ALSymbols;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class AddDotToToolTipsSyntaxRewriter : ALSyntaxRewriter
    {

        public override SyntaxNode VisitLabelPropertyValue(LabelPropertyValueSyntax node)
        {
            var propertyName = (node.Parent as PropertySyntax)?.Name?.Identifier.ValueText;
            if ((!String.IsNullOrEmpty(propertyName)) && (propertyName.Equals("ToolTip", StringComparison.OrdinalIgnoreCase)))
            {
                var labelText = node.Value?.LabelText?.Value.Text;
                if (labelText != null)
                {
                    labelText = ALSyntaxHelper.DecodeString(labelText.Trim()).Trim();
                    if (!labelText.EndsWith("."))
                    {
                        labelText = labelText + ".";
                        var newLabel = node.Value.WithLabelText(SyntaxFactory.StringLiteralValue(SyntaxFactory.Literal(labelText)));
                        node = node.WithValue(newLabel);
                        NoOfChanges++;
                    }
                }
            }

            return base.VisitLabelPropertyValue(node);
        }

    }
}
