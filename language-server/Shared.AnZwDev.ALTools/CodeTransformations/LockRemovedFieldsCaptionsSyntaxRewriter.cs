using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeAnalysis;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class LockRemovedFieldsCaptionsSyntaxRewriter : ALCaptionsSyntaxRewriter
    {

        public LockRemovedFieldsCaptionsSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitField(FieldSyntax node)
        {
            if (this.IsFieldRemoved(node))
            {
                PropertySyntax propertySyntax = node.GetProperty("Caption");
                LabelSyntax labelSyntax = (propertySyntax?.Value as LabelPropertyValueSyntax)?.Value;
                if (labelSyntax != null)
                {
                    if (!this.IsLocked(labelSyntax.Properties))
                    {
                        LabelSyntax newLabelSyntax;
                        CommaSeparatedIdentifierEqualsLiteralListSyntax propertiesList = labelSyntax.Properties;
                        if (propertiesList == null)
                            newLabelSyntax = labelSyntax
                                .WithCommaToken(SyntaxFactory.Token(EnumExtensions.FromString("CommaToken", SyntaxKind.CommaToken)))
                                .WithProperties(this.CreateCaptionLockedProperties());
                        else
                            newLabelSyntax = labelSyntax.WithProperties(propertiesList.AddValues(this.CreateLockedProperty()));

                        NoOfChanges++;
                        return node.ReplaceNode(labelSyntax, newLabelSyntax);
                    }
                }
            }
            return base.VisitField(node);
        }

        protected CommaSeparatedIdentifierEqualsLiteralListSyntax CreateCaptionLockedProperties()
        {
            List<IdentifierEqualsLiteralSyntax> propertiesList = new List<IdentifierEqualsLiteralSyntax>();
            propertiesList.Add(this.CreateLockedProperty());
            SeparatedSyntaxList<IdentifierEqualsLiteralSyntax> separatedSyntaxList = new SeparatedSyntaxList<IdentifierEqualsLiteralSyntax>();
            separatedSyntaxList = separatedSyntaxList.AddRange(propertiesList);
            return SyntaxFactory.CommaSeparatedIdentifierEqualsLiteralList(separatedSyntaxList);
        }

        protected IdentifierEqualsLiteralSyntax CreateLockedProperty()
        {
            return SyntaxFactoryHelper.IdentifierEqualsLiteral("Locked", true);
        }

        protected bool IsLocked(CommaSeparatedIdentifierEqualsLiteralListSyntax propertiesList)
        {
            if (propertiesList != null)
                foreach (var property in propertiesList.Values)
                    if (property.Identifier.ValueText?.ToLower() == "locked")
                        return true;
            return false;
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
