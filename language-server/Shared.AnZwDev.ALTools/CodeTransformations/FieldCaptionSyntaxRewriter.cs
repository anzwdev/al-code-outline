using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class FieldCaptionSyntaxRewriter : ALCaptionsSyntaxRewriter
    {

        public bool LockRemovedFields { get; set; }

        public FieldCaptionSyntaxRewriter()
        {
            this.LockRemovedFields = false;
        }

        public override SyntaxNode VisitField(FieldSyntax node) 
        {
            if (!node.HasProperty("CaptionML"))
            {
                bool lockCaption = (this.LockRemovedFields && this.IsFieldRemoved(node));

                PropertySyntax propertySyntax = node.GetProperty("Caption");
                bool changed = false;
                if (propertySyntax == null)
                {
                    changed = true;
                    node = node.AddPropertyListProperties(
                        this.CreateCaptionPropertyFromName(node, lockCaption));
                }
                else
                {
                    string valueText = propertySyntax.Value.ToString();
                    if (String.IsNullOrWhiteSpace(valueText))
                    {
                        changed = true;
                        node = node.ReplaceNode(propertySyntax, this.CreateCaptionPropertyFromName(node, lockCaption));
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

            return base.VisitField(node);
        }

        protected bool IsFieldRemoved(FieldSyntax node)
        {
            PropertyValueSyntax obsoleteStateSyntax = node.GetPropertyValue("ObsoleteState");
            if (obsoleteStateSyntax == null)
                return false;
            string value = ALSyntaxHelper.DecodeName(obsoleteStateSyntax.ToString());
            return ((value != null) && (value.Equals("Removed", StringComparison.CurrentCultureIgnoreCase)));
        }


    }
}
