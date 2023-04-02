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

        private string _tableDataClassification = null;
        private string _fieldsDataClassification = null;
        private ValueCheckState _fieldsDataClassificationCheckState = ValueCheckState.NotChecked;
        private bool _tableProcessingActive = false;

        public RemoveRedundantDataClassificationSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitTable(TableSyntax node)
        {
            InitProcessingVariables(node);

            var newNode = base.VisitTable(node);
            node = newNode as TableSyntax;

            if ((node != null) && (String.IsNullOrWhiteSpace(_tableDataClassification)) && (!String.IsNullOrWhiteSpace(_fieldsDataClassification)) && (_fieldsDataClassificationCheckState == ValueCheckState.Equal))
            {
                NoOfChanges++;
                node = node.AddPropertyListProperties(this.CreateDataClassificationProperty(node, _fieldsDataClassification));
                _tableDataClassification = _fieldsDataClassification;

                //run processing again to remove redundant data classification
                newNode = base.VisitTable(node);
            }

            ClearProcessingVariables();

            return newNode;
        }

        private void InitProcessingVariables(SyntaxNode node)
        {
            _tableDataClassification = GetDataClassificationValue(node);
            _fieldsDataClassification = null;
            _fieldsDataClassificationCheckState = ValueCheckState.NotChecked;
            _tableProcessingActive = true;
        }

        private void ClearProcessingVariables()
        {
            _tableDataClassification = null;
            _fieldsDataClassification = null;
            _fieldsDataClassificationCheckState = ValueCheckState.NotChecked;
            _tableProcessingActive = false;
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
