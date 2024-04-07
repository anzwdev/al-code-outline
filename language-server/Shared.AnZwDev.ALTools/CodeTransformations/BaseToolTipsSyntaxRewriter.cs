using AnZwDev.ALTools.CodeAnalysis;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class BaseToolTipsSyntaxRewriter : BasePageWithSourceSyntaxRewriter
    {

        public bool SortProperties { get; set; }

        private SortPropertiesSyntaxRewriter _sortPropertiesSyntaxRewriter = new SortPropertiesSyntaxRewriter();

        public BaseToolTipsSyntaxRewriter()
        {
        }

        protected PageFieldSyntax SetPageFieldToolTip(PageFieldSyntax node, LabelInformation newToolTip)
        {
            return SetPageFieldToolTip(node, newToolTip.Value, newToolTip.Comment);
        }

        protected PageFieldSyntax SetPageFieldToolTip(PageFieldSyntax node, string newToolTip, string newToolTipComment)
        {
            PropertySyntax propertySyntax = node.GetProperty("ToolTip");
            if (propertySyntax == null)
            {
                var newPropertySyntax = SyntaxFactoryHelper.ToolTipProperty(newToolTip, newToolTipComment, false);

                /*
                SyntaxTriviaList leadingTriviaList = node.CreateChildNodeIdentTrivia();
                SyntaxTriviaList trailingTriviaList = SyntaxFactory.ParseTrailingTrivia("\r\n", 0);
                newPropertySyntax = newPropertySyntax
                    .WithLeadingTrivia(leadingTriviaList)
                    .WithTrailingTrivia(trailingTriviaList);
                */

                newPropertySyntax = newPropertySyntax
                    .WithLeadingTrivia(node.CreateChildNodeIdentTrivia())
                    .WithTrailingNewLine();

                node = node.AddPropertyListProperties(newPropertySyntax);

                if (SortProperties)
                    node = node.WithPropertyList(_sortPropertiesSyntaxRewriter.SortPropertyList(node.PropertyList, out _));

                return node;
            }
            else
            {
                bool validValue = true;
                if (propertySyntax.Value is LabelPropertyValueSyntax labelValue)
                    validValue = ((labelValue.Value?.LabelText?.Value.Value?.ToString()) != newToolTip);
                if (validValue)
                {
                    PropertySyntax newPropertySyntax = SyntaxFactoryHelper.ToolTipProperty(newToolTip, newToolTipComment, false)
                        .WithTriviaFrom(propertySyntax);
                    return node.ReplaceNode(propertySyntax, newPropertySyntax);
                }
            }
            return null;
        }

    }
}
