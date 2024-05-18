using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class RemoveRedundantDataClassificationSyntaxRewriter : ALSyntaxRewriter
    {

        public bool SortProperties { get; set; }

        private SortPropertiesSyntaxRewriter _sortPropertiesSyntaxRewriter = new SortPropertiesSyntaxRewriter();
        private string _tableDataClassification = null;

        public RemoveRedundantDataClassificationSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitTable(TableSyntax node)
        {
            _tableDataClassification = GetDataClassificationValue(node);
            if (String.IsNullOrEmpty(_tableDataClassification))
            {
                var fieldsDataClassification = GetFieldsDataClassificationValue(node.Fields?.Fields);
                if (!String.IsNullOrEmpty(fieldsDataClassification))
                {
                    NoOfChanges++;
                    node = node.AddPropertyListProperties(
                        this.CreateDataClassificationProperty(node, fieldsDataClassification));

                    if (SortProperties)
                        node = node.WithPropertyList(_sortPropertiesSyntaxRewriter.SortPropertyList(node.PropertyList, out _));

                    _tableDataClassification = fieldsDataClassification;
                }
            }
            var tableNode = base.VisitTable(node);
            _tableDataClassification = null;
            return tableNode;
        }

        public override SyntaxNode VisitTableExtension(TableExtensionSyntax node)
        {
            _tableDataClassification = null;
            return base.VisitTableExtension(node);
        }

        public override SyntaxNode VisitField(FieldSyntax node)
        {
            PropertySyntax propertySyntax = node.GetProperty("DataClassification");
            if (propertySyntax != null)
            {
                string valueText = propertySyntax.Value.ToString();
                if ((!String.IsNullOrWhiteSpace(valueText)) && (!String.IsNullOrWhiteSpace(_tableDataClassification)) && (_tableDataClassification.Equals(valueText, StringComparison.OrdinalIgnoreCase)))
                {
                    NoOfChanges++;
                    return node.WithPropertyList(
                        node.PropertyList.WithProperties(
                            node.PropertyList.Properties.Remove(propertySyntax)));
                }
            }
            return base.VisitField(node);
        }

        protected string GetFieldsDataClassificationValue(IEnumerable<FieldSyntax> fields)
        {
            if (fields == null)
                return null;

            string foundDataClassification = null;
            foreach (var field in fields)
            {
                var fieldDataClassification = GetDataClassificationValue(field);
                
                if (String.IsNullOrWhiteSpace(fieldDataClassification))
                    return null;

                if ((foundDataClassification != null) && (!foundDataClassification.Equals(fieldDataClassification, StringComparison.OrdinalIgnoreCase)))
                    return null;

                foundDataClassification = fieldDataClassification;
            }
            return foundDataClassification;
        }

        protected string GetDataClassificationValue(SyntaxNode node)
        {
            PropertySyntax dataClassificationProperty = node.GetProperty("DataClassification");
            if (dataClassificationProperty != null)
                return ALSyntaxHelper.DecodeName(dataClassificationProperty.Value?.ToString());
            return null;
        }

        protected PropertySyntax CreateDataClassificationProperty(SyntaxNode node, string value)
        {
            //calculate indent
            int indentLength = 4;
            string indent;
            SyntaxTriviaList leadingTrivia = node.GetLeadingTrivia();
            if (leadingTrivia != null)
            {
                indent = leadingTrivia.ToString();
                int newLinePos = indent.LastIndexOf("/n");
                if (newLinePos >= 0)
                    indent = indent.Substring(newLinePos + 1);
                indentLength += indent.Length;
            }
            indent = "".PadLeft(indentLength);

            SyntaxTriviaList leadingTriviaList = SyntaxFactory.ParseLeadingTrivia(indent, 0);
            SyntaxTriviaList trailingTriviaList = SyntaxFactory.ParseTrailingTrivia("\r\n", 0);

            return SyntaxFactory.Property(
                "DataClassification",
                SyntaxFactory.EnumPropertyValue(SyntaxFactory.IdentifierName(value)))
                .WithLeadingTrivia(leadingTriviaList)
                .WithTrailingTrivia(trailingTriviaList);
        }

    }
}
