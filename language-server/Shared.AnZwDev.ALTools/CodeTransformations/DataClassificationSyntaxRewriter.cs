using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class DataClassificationSyntaxRewriter : ALSyntaxRewriter
    {

        public string DataClassification { get; set; }

        public DataClassificationSyntaxRewriter()
        {
            this.DataClassification = null;
        }

        public override SyntaxNode VisitTable(TableSyntax node)
        {
            PropertySyntax propertySyntax = node.GetProperty("DataClassification");
            if (propertySyntax == null)
            {
                NoOfChanges++;
                node = node.AddPropertyListProperties(
                    this.CreateDataClassificationProperty(node));
            }
            else
            {
                string valueText = propertySyntax.Value.ToString();
                if ((String.IsNullOrWhiteSpace(valueText)) ||
                    (valueText.Equals("ToBeClassified", StringComparison.CurrentCultureIgnoreCase)))
                {
                    NoOfChanges++;
                    node = node.ReplaceNode(propertySyntax, this.CreateDataClassificationProperty(node));
                }
            }

            return base.VisitTable(node);
        }

        public override SyntaxNode VisitField(FieldSyntax node)
        {
            PropertyValueSyntax fieldClass = node.GetPropertyValue("FieldClass");
            if ((fieldClass == null) || (fieldClass.ToString().Equals("Normal", StringComparison.CurrentCultureIgnoreCase)))
            {
                PropertySyntax propertySyntax = node.GetProperty("DataClassification");
                if (propertySyntax == null)
                {
                    NoOfChanges++;
                    return node.AddPropertyListProperties(
                        this.CreateDataClassificationProperty(node));
                }
                else
                {
                    string valueText = propertySyntax.Value.ToString();
                    if ((String.IsNullOrWhiteSpace(valueText)) ||
                        (valueText.Equals("ToBeClassified", StringComparison.CurrentCultureIgnoreCase)))
                    {
                        NoOfChanges++;
                        return node.ReplaceNode(propertySyntax, this.CreateDataClassificationProperty(node));
                    }
                }
            } 
            else
            {
                PropertySyntax propertySyntax = node.GetProperty("DataClassification");
                if (propertySyntax != null)
                {
                    NoOfChanges++;
                    return node.WithPropertyList(
                        node.PropertyList.WithProperties(
                            node.PropertyList.Properties.Remove(propertySyntax)));
                }
            }
            return base.VisitField(node);
        }

        protected PropertySyntax CreateDataClassificationProperty(SyntaxNode node)
        {
            var propertySyntax = SyntaxFactory.Property(
                "DataClassification",
                SyntaxFactory.EnumPropertyValue(SyntaxFactory.IdentifierName(this.DataClassification)));

            /*
            SyntaxTriviaList leadingTriviaList = node.CreateChildNodeIdentTrivia();
            SyntaxTriviaList trailingTriviaList = SyntaxFactory.ParseTrailingTrivia("\r\n", 0);
            propertySyntax = propertySyntax
                .WithLeadingTrivia(leadingTriviaList)
                .WithTrailingTrivia(trailingTriviaList);
            */

            return propertySyntax;

            /*
            return SyntaxFactory.Property(
                SyntaxFactory.PropertyName(SyntaxFactory.Identifier("DataClassification"))
                    .WithTrailingTrivia(SyntaxFactory.ParseTrailingTrivia(" ")),
                SyntaxFactory.EnumPropertyValue(SyntaxFactory.IdentifierName(this.DataClassification))
                    .WithLeadingTrivia(SyntaxFactory.ParseLeadingTrivia(" ")))
                .WithLeadingTrivia(leadingTriviaList)                
                .WithTrailingTrivia(trailingTriviaList);
            */
        }


    }
}
