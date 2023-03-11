using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.Extensions;

namespace AnZwDev.ALTools.CodeTransformations
{

#if BC

    public class EnumCaptionSyntaxRewriter : ALCaptionsSyntaxRewriter
    {

        public EnumCaptionSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitEnumValue(EnumValueSyntax node)
        {
            if (!node.HasProperty("CaptionML"))
            {
                PropertySyntax propertySyntax = node.GetProperty("Caption");
                bool changed = false;
                if (propertySyntax == null)
                {
                    changed = true;
                    node = node.AddPropertyListProperties(
                        this.CreateCaptionPropertyFromName(node, false));
                }
                else
                {
                    string valueText = propertySyntax.Value.ToString();
                    if (String.IsNullOrWhiteSpace(valueText))
                    {
                        changed = true;
                        node = node.ReplaceNode(propertySyntax, this.CreateCaptionPropertyFromName(node, false));
                    }
                }

                if (changed)
                {
                    NoOfChanges++;
                    if (SortProperties)
                        node = node.WithPropertyList(SortPropertiesSyntaxRewriter.SortPropertyList(node.PropertyList, out _));
                    return node;
                }

            }

            return base.VisitEnumValue(node);
        }
    }

#endif

}
