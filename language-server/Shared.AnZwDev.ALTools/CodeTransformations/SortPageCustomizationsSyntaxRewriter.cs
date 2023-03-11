using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class SortPageCustomizationsSyntaxRewriter : ALSyntaxRewriter
    {

        public SortPageCustomizationsSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitProfile(ProfileSyntax node)
        {
            PropertySyntax property = node.PropertyList.GetPropertyEntry("Customizations");
            if ((property != null) && (this.NodeInSpan(property)) && (!property.ContainsDiagnostics))
            {
                PropertySyntax newProperty = property.SortCommaSeparatedPropertyValue(out bool sorted);
                if (sorted)
                    NoOfChanges++;
                node = node.WithPropertyList(
                    node.PropertyList.WithProperties(
                        node.PropertyList.Properties.Replace(property, newProperty)));
            }
            return base.VisitProfile(node);
        }

    }
}
