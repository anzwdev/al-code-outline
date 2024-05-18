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
                return node.ReplaceNode(oldCaptionPropertySyntax, this.CreateCaptionPropertyFromName(node, locked, oldCaptionPropertySyntax));
            }
            return node;
        }

        protected PropertySyntax CreateCaptionPropertyFromName(SyntaxNode node, bool locked, PropertySyntax existingProperty)
        {
            string value = node.GetNameStringValue().RemovePrefixSuffix(
                this.Project.MandatoryPrefixes, this.Project.MandatorySuffixes, this.Project.MandatoryAffixes, this.Project.AdditionalMandatoryAffixesPatterns);

            var propertySyntax = SyntaxFactoryHelper.CaptionProperty(value, null, locked);

            if (existingProperty != null)
                propertySyntax = propertySyntax.WithTriviaFrom(existingProperty);
            else
                propertySyntax = propertySyntax
                    .WithLeadingTrivia(node.CreateChildNodeIdentTrivia())
                    .WithTrailingNewLine();

            return propertySyntax;
        }

    }
}
