using AnZwDev.ALTools.ALSymbols;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    /*
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
                node = node.AddPropertyListProperties(this.CreateApplicationAreaProperty(node, _pageMembersAppArea));
                _pageAppArea = _pageMembersAppArea;

                //run processing again to remove redundant data classification
                var newNode = base.VisitTable(node);
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

    }
    */
}
