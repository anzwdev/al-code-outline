using AnZwDev.ALTools.ALSymbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.Extensions;
using System.Linq;
using System.Xml.Linq;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Core;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class RemoveRedundandAppAreasSyntaxRewriter : ALSyntaxRewriter
    {

        private UniqueStringList _pageAppArea = null;
        private UniqueStringList _pageMembersAppArea = null;
        private ValueCheckState _pageMembersAppAreaState = ValueCheckState.NotChecked;
        private bool _pageAppAreaProcessingActive = false;

        public RemoveRedundandAppAreasSyntaxRewriter()
        {
        }

        protected override SyntaxNode AfterVisitNode(SyntaxNode node)
        {
            if (this.NoOfChanges == 0)
                return null;
            return base.AfterVisitNode(node);
        }

        public override SyntaxNode VisitPage(PageSyntax node)
        {
            InitProcessingVariables(node);

            var newNode = base.VisitPage(node);

            node = newNode as PageSyntax;
            if ((node != null) && (UniqueStringList.IsNullOrEmpty(_pageAppArea)) && (!UniqueStringList.IsNullOrEmpty(_pageMembersAppArea)) && (_pageMembersAppAreaState == ValueCheckState.Equal))
            {
                NoOfChanges++;
                node = node.AddPropertyListProperties(this.CreateApplicationAreaProperty(node, _pageMembersAppArea));
                _pageAppArea = _pageMembersAppArea;

                //run processing again to remove redundant application areas
                newNode = base.VisitPage(node);
            }

            ClearProcessingVariables();

            return newNode;
        }

        private void InitProcessingVariables(SyntaxNode node)
        {
            (_, _pageAppArea) = GetApplicationArea(node);
            _pageMembersAppArea = null;
            _pageMembersAppAreaState = ValueCheckState.NotChecked;
            _pageAppAreaProcessingActive = true;
        }

        private void ClearProcessingVariables()
        {
            _pageAppArea = null;
            _pageMembersAppArea = null;
            _pageMembersAppAreaState = ValueCheckState.NotChecked;
            _pageAppAreaProcessingActive = false;
        }

        public override SyntaxNode VisitPageLabel(PageLabelSyntax node)
        {
            (var propertyList, var propertyListUpdated) = CheckAndRemovePageMemberAppArea(node, node.PropertyList);
            if (propertyListUpdated)
                node = node.WithPropertyList(propertyList);
            return base.VisitPageLabel(node);
        }

        public override SyntaxNode VisitPageField(PageFieldSyntax node)
        {
            (var propertyList, var propertyListUpdated) = CheckAndRemovePageMemberAppArea(node, node.PropertyList);
            if (propertyListUpdated)
                node = node.WithPropertyList(propertyList);
            return base.VisitPageField(node);
        }

        public override SyntaxNode VisitPageUserControl(PageUserControlSyntax node)
        {
            (var propertyList, var propertyListUpdated) = CheckAndRemovePageMemberAppArea(node, node.PropertyList);
            if (propertyListUpdated)
                node = node.WithPropertyList(propertyList);
            return base.VisitPageUserControl(node);
        }

        public override SyntaxNode VisitPagePart(PagePartSyntax node)
        {
            (var propertyList, var propertyListUpdated) = CheckAndRemovePageMemberAppArea(node, node.PropertyList);
            if (propertyListUpdated)
                node = node.WithPropertyList(propertyList);
            return base.VisitPagePart(node);
        }

        public override SyntaxNode VisitPageSystemPart(PageSystemPartSyntax node)
        {
            (var propertyList, var propertyListUpdated) = CheckAndRemovePageMemberAppArea(node, node.PropertyList);
            if (propertyListUpdated)
                node = node.WithPropertyList(propertyList);
            return base.VisitPageSystemPart(node);
        }

#if BC
        public override SyntaxNode VisitPageChartPart(PageChartPartSyntax node)
        {
            (var propertyList, var propertyListUpdated) = CheckAndRemovePageMemberAppArea(node, node.PropertyList);
            if (propertyListUpdated)
                node = node.WithPropertyList(propertyList);
            return base.VisitPageChartPart(node);
        }
#endif

        public override SyntaxNode VisitPageAction(PageActionSyntax node)
        {
            (var propertyList, var propertyListUpdated) = CheckAndRemovePageMemberAppArea(node, node.PropertyList);
            if (propertyListUpdated)
                node = node.WithPropertyList(propertyList);
            return base.VisitPageAction(node);
        }

        private (PropertyListSyntax, bool) CheckAndRemovePageMemberAppArea(SyntaxNode node, PropertyListSyntax propertyList)
        {
            if (_pageAppAreaProcessingActive)
            {
                (PropertySyntax appAreaProperty, UniqueStringList memberAppArea) = GetApplicationArea(node);

                CheckIfMemberAppAreasAreTheSame(memberAppArea);

                if (ShouldRemoveAppArea(memberAppArea))
                    return (RemoveProperty(propertyList, appAreaProperty), true);
            }

            return (propertyList, false);
        }

        private bool ShouldRemoveAppArea(UniqueStringList memberAppArea)
        {
            return
                (!UniqueStringList.IsNullOrEmpty(_pageAppArea)) &&
                (!UniqueStringList.IsNullOrEmpty(memberAppArea)) &&
                (memberAppArea.Equals(_pageAppArea));
        }

        private PropertyListSyntax RemoveProperty(PropertyListSyntax propertyList, PropertySyntax property)
        {
            NoOfChanges++;
            return propertyList.WithProperties(
                    propertyList.Properties.Remove(property));
        }

        private void CheckIfMemberAppAreasAreTheSame(UniqueStringList memberAppArea)
        {
            switch (_pageMembersAppAreaState)
            {
                case ValueCheckState.NotChecked:
                    _pageMembersAppArea = memberAppArea;
                    _pageMembersAppAreaState = ValueCheckState.Equal;
                    break;
                case ValueCheckState.Equal:
                    var equal =
                        ((memberAppArea == null) && (_pageMembersAppArea == null)) ||
                        ((memberAppArea != null) && (_pageMembersAppArea != null) && (memberAppArea.Equals(_pageMembersAppArea)));
                    if (!equal)
                    {
                        _pageMembersAppArea = null;
                        _pageMembersAppAreaState = ValueCheckState.Different;
                    }    
                    break;
            }
        }

        protected PropertySyntax CreateApplicationAreaProperty(SyntaxNode node, UniqueStringList valuesList)
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

            SeparatedSyntaxList<IdentifierNameSyntax> values = new SeparatedSyntaxList<IdentifierNameSyntax>();
            for (int i=0; i < valuesList.Count; i++)           
                values = values.Add(SyntaxFactory.IdentifierName(valuesList[i]));

            //try to convert from string to avoid issues with enum ids changed between AL compiler versions
            PropertyKind propertyKind;
            try
            {
                propertyKind = (PropertyKind)Enum.Parse(typeof(PropertyKind), "ApplicationArea", true);
            }
            catch (Exception)
            {
                propertyKind = PropertyKind.ApplicationArea;
            }

            return SyntaxFactory.Property(propertyKind, SyntaxFactory.CommaSeparatedPropertyValue(values))
                .WithLeadingTrivia(leadingTriviaList)
                .WithTrailingTrivia(trailingTriviaList);
        }


        private (PropertySyntax, UniqueStringList) GetApplicationArea(SyntaxNode node)
        {
            PropertySyntax appAreaProperty = node.GetProperty("ApplicationArea");

            if (appAreaProperty != null)
            {
                CommaSeparatedPropertyValueSyntax appAreaValue = appAreaProperty.Value as CommaSeparatedPropertyValueSyntax;
                if (appAreaValue != null)
                {
                    UniqueStringList appAreas = new UniqueStringList();
                    foreach (IdentifierNameSyntax value in appAreaValue.Values)
                        appAreas.Add(ALSyntaxHelper.DecodeName(value.Identifier.ValueText).Trim());
                    return (appAreaProperty, appAreas);
                }
            }

            return (appAreaProperty, null);
        }


    }
}
