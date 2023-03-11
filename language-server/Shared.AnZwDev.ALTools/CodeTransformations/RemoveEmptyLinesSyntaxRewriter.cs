using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class RemoveEmptyLinesSyntaxRewriter : ALSyntaxRewriter
    {

        public RemoveEmptyLinesSyntaxRewriter()
        {
        }

        #region All object types

        public override SyntaxNode VisitPropertyList(PropertyListSyntax node)
        {
            node = node.WithProperties(SetEmptyLinesBetweenNodes(node.Properties, 0));
            return base.VisitPropertyList(node);
        }

        #endregion

        #region Tables and table extensions

        public override SyntaxNode VisitFieldList(FieldListSyntax node)
        {
            node = node.WithFields(SetEmptyLinesBetweenNodes(node.Fields, 0));
            return base.VisitFieldList(node);
        }

        public override SyntaxNode VisitKeyList(KeyListSyntax node)
        {
            node = node.WithKeys(SetEmptyLinesBetweenNodes(node.Keys, 0));
            return base.VisitKeyList(node);
        }

        public override SyntaxNode VisitFieldGroupList(FieldGroupListSyntax node)
        {
            node = node.WithFieldGroups(SetEmptyLinesBetweenNodes(node.FieldGroups, 0));
            return base.VisitFieldGroupList(node);
        }

        public override SyntaxNode VisitFieldExtensionList(FieldExtensionListSyntax node)
        {
            node = node.WithFields(SetEmptyLinesBetweenNodes(node.Fields, 0));
            return base.VisitFieldExtensionList(node);
        }

        #endregion

        #region Pages and page extensions

        public override SyntaxNode VisitPageGroup(PageGroupSyntax node)
        {
            node = node.WithControls(SetEmptyLinesBetweenNodes(node.Controls, 0));
            return base.VisitPageGroup(node);
        }

        public override SyntaxNode VisitPageActionGroup(PageActionGroupSyntax node)
        {
            node = node.WithActions(SetEmptyLinesBetweenNodes(node.Actions, 0));
            return base.VisitPageActionGroup(node);
        }

        public override SyntaxNode VisitPageActionList(PageActionListSyntax node)
        {
            node = node.WithAreas(SetEmptyLinesBetweenNodes(node.Areas, 0));
            return base.VisitPageActionList(node);
        }

        public override SyntaxNode VisitPageExtensionActionList(PageExtensionActionListSyntax node)
        {
            node = node.WithChanges(SetEmptyLinesBetweenNodes(node.Changes, 0));
            return base.VisitPageExtensionActionList(node);
        }

        #endregion

        #region XmlPorts

        public override SyntaxNode VisitXmlPortTableElement(XmlPortTableElementSyntax node)
        {
            node = node.WithSchema(SetEmptyLinesBetweenNodes(node.Schema, 0));
            return base.VisitXmlPortTableElement(node);
        }

        public override SyntaxNode VisitXmlPortTextElement(XmlPortTextElementSyntax node)
        {
            node = node.WithSchema(SetEmptyLinesBetweenNodes(node.Schema, 0));
            return base.VisitXmlPortTextElement(node);
        }

        public override SyntaxNode VisitXmlPortSchema(XmlPortSchemaSyntax node)
        {
            node = node.WithXmlPortSchema(SetEmptyLinesBetweenNodes(node.XmlPortSchema, 0));
            return base.VisitXmlPortSchema(node);
        }

        #endregion

        #region Queries

        public override SyntaxNode VisitQueryDataItem(QueryDataItemSyntax node)
        {
            node = node.WithElements(SetEmptyLinesBetweenNodes(node.Elements, 0));
            return base.VisitQueryDataItem(node);
        }

        #endregion

        #region Reports

        public override SyntaxNode VisitReportDataItem(ReportDataItemSyntax node)
        {
            node = node.WithElements(SetEmptyLinesBetweenNodes(node.Elements, 0));
            return base.VisitReportDataItem(node);
        }

        #endregion

        #region Nodes list processing

        protected SyntaxList<T> SetEmptyLinesBetweenNodes<T>(SyntaxList<T> nodesList, int noOfEmptyLines) where T : SyntaxNode
        {
            if ((nodesList == null) || (nodesList.Count <= 1))
                return nodesList;

            List<T> newNodes = null;
            for (int nodeIndex = 1; nodeIndex < nodesList.Count; nodeIndex++)
            {
                SyntaxNode node1 = (newNodes == null) ? nodesList[nodeIndex - 1] : newNodes[nodeIndex - 1];
                SyntaxNode node2 = (newNodes == null) ? nodesList[nodeIndex] : newNodes[nodeIndex];

                SyntaxTriviaList triviaList1 = node1.GetTrailingTrivia();
                SyntaxTriviaList triviaList2 = node2.GetLeadingTrivia();
                IEnumerable<SyntaxTrivia> mergedTrivias = MergedTriviaEnumerable(triviaList1, triviaList2);

                if (UpdateRequired(mergedTrivias, noOfEmptyLines))
                {
                    //prepare nodes list
                    if (newNodes == null)
                    {
                        newNodes = new List<T>();
                        newNodes.AddRange(nodesList);
                    }

                    //process
                    List<SyntaxTrivia> node1TriviaList = mergedTrivias.FirstLineOnly(true);
                    List<SyntaxTrivia> node2TriviaList = MergedReversedTriviaEnumerable(triviaList1, triviaList2).FirstLineOnly(false);
                    //for (int emptyLineIndex = 0; emptyLineIndex < noOfEmptyLines; emptyLineIndex++)
                    //    node2TriviaList.Add(_newLineTrivia);
                    node2TriviaList.Reverse();

                    //update
                    newNodes[nodeIndex - 1] = (T)node1.WithTrailingTrivia(SyntaxFactory.TriviaList(node1TriviaList));
                    newNodes[nodeIndex] = (T)node2.WithLeadingTrivia(SyntaxFactory.TriviaList(node2TriviaList));
                }
            }

            if (newNodes != null)
                return SyntaxFactory.List<T>(newNodes);

            return nodesList;
        }

        protected bool UpdateRequired(IEnumerable<SyntaxTrivia> triviaList, int requiredNoOfLines)
        {
            bool firstLine = true;
            int noOfLines = -1;
            foreach (SyntaxTrivia trivia in triviaList)
            {
                ConvertedSyntaxKind kind = trivia.Kind.ConvertToLocalType();
                switch (kind)
                {
                    case ConvertedSyntaxKind.WhiteSpaceTrivia:
                        break;
                    case ConvertedSyntaxKind.EndOfLineTrivia:
                        noOfLines++;
                        firstLine = false;
                        break;
                    default:
                        if (!firstLine)
                            return false;
                        break;
                }
            }
            return (!firstLine) && (noOfLines != requiredNoOfLines);
        }


        protected IEnumerable<SyntaxTrivia> MergedTriviaEnumerable(SyntaxTriviaList list1, SyntaxTriviaList list2)
        {
            if (list1 != null)
                for (int i = 0; i < list1.Count; i++)
                    yield return list1[i];
            if (list2 != null)
                for (int i = 0; i < list2.Count; i++)
                    yield return list2[i];
        }

        protected IEnumerable<SyntaxTrivia> MergedReversedTriviaEnumerable(SyntaxTriviaList list1, SyntaxTriviaList list2)
        {
            if (list2 != null)
                for (int i = list2.Count - 1; i >= 0; i--)
                    yield return list2[i];
            if (list1 != null)
                for (int i = list1.Count - 1; i >= 0; i--)
                    yield return list1[i];
        }

        #endregion

    }
}
