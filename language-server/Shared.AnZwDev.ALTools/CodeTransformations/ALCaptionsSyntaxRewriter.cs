using AnZwDev.ALTools.CodeAnalysis;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class ALCaptionsSyntaxRewriter : ALSyntaxRewriter
    {

        public bool SortProperties { get; set; }

        protected SortPropertiesSyntaxRewriter SortPropertiesSyntaxRewriter { get; }

        public ALCaptionsSyntaxRewriter()
        {
            SortProperties = false;
            SortPropertiesSyntaxRewriter = new SortPropertiesSyntaxRewriter();
        }

        protected T UpdateCaptionFromName<T>(T node, PropertySyntax oldCaptionPropertySyntax, bool locked) where T : SyntaxNode
        {
            string valueText = oldCaptionPropertySyntax.Value.ToString();
            if (String.IsNullOrWhiteSpace(valueText))
            {
                NoOfChanges++;
                return node.ReplaceNode(oldCaptionPropertySyntax, this.CreateCaptionPropertyFromName(node, locked));
            }
            return node;
        }

        protected PropertySyntax CreateCaptionPropertyFromName(SyntaxNode node, bool locked)
        {
            string value = node.GetNameStringValue().RemovePrefixSuffix(
                this.Project.MandatoryPrefixes, this.Project.MandatorySuffixes, this.Project.MandatoryAffixes, this.Project.AdditionalMandatoryAffixesPatterns);

            var propertySyntax = SyntaxFactoryHelper.CaptionProperty(value, null, locked);

            /*
            SyntaxTriviaList leadingTriviaList = node.CreateChildNodeIdentTrivia();
            SyntaxTriviaList trailingTriviaList = SyntaxFactory.ParseTrailingTrivia("\r\n", 0);
            propertySyntax = propertySyntax
                .WithLeadingTrivia(leadingTriviaList)
                .WithTrailingTrivia(trailingTriviaList);
            */

            return propertySyntax;
        }

    }
}
