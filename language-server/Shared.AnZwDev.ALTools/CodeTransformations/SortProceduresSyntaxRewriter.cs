using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Core;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    internal class SortProceduresSyntaxRewriter : ALSyntaxRewriter
    {

        #region Member sort info

        protected class MethodSortInfo<T> where T : SyntaxNode
        {
            public string Name { get; set; }
            public ALSymbolKind Kind { get; set; }
            public int Index { get; set; }
            public T Node { get; set; }

            public MethodSortInfo(T node, int index)
            {
                this.Node = node;
                this.Index = index;
                this.Name = node.GetNameStringValue();
                this.Kind = GetKind(node);
            }

            private ALSymbolKind GetKind(SyntaxNode node)
            {
                ConvertedSyntaxKind kind = node.Kind.ConvertToLocalType();
                switch (kind)
                {
                    case ConvertedSyntaxKind.TriggerDeclaration:
                        return ALSymbolKind.TriggerDeclaration;
                    case ConvertedSyntaxKind.MethodDeclaration:
                        MethodDeclarationSyntax methodNode = node as MethodDeclarationSyntax;
                        foreach (MemberAttributeSyntax att in methodNode.Attributes)
                        {
                            ALSymbolKind alKind = ALSyntaxHelper.MemberAttributeToMethodKind(att.GetNameStringValue());
                            if (alKind != ALSymbolKind.Undefined)
                                return alKind;
                        }
#if BC
                        if (methodNode.AccessModifier != null)
                        {
                            switch (methodNode.AccessModifier.Kind.ConvertToLocalType())
                            {
                                case ConvertedSyntaxKind.ProtectedKeyword:
                                    return ALSymbolKind.ProtectedMethodDeclaration;
                                case ConvertedSyntaxKind.LocalKeyword:
                                    return ALSymbolKind.LocalMethodDeclaration;
                                case ConvertedSyntaxKind.InternalKeyword:
                                    return ALSymbolKind.InternalMethodDeclaration;
                            }
                        }
#endif
                        return ALSymbolKind.MethodDeclaration;
                }
                return ALSymbolKind.Undefined;
            }

            public static List<MethodSortInfo<T>> FromSyntaxList(SyntaxList<T> nodeList)
            {
                List<MethodSortInfo<T>> list = new List<MethodSortInfo<T>>();
                for (int i = 0; i < nodeList.Count; i++)
                {
                    list.Add(new MethodSortInfo<T>(nodeList[i], i));
                }
                return list;
            }

            public static List<MethodSortInfo<T>> FromNodesList(List<T> nodeList)
            {
                List<MethodSortInfo<T>> list = new List<MethodSortInfo<T>>();
                for (int i = 0; i < nodeList.Count; i++)
                {
                    list.Add(new MethodSortInfo<T>(nodeList[i], i));
                }
                return list;
            }


            public static SyntaxList<T> ToSyntaxList(List<MethodSortInfo<T>> sortInfoList)
            {
                List<T> nodeList = new List<T>();
                for (int i = 0; i < sortInfoList.Count; i++)
                {
                    nodeList.Add(sortInfoList[i].Node);
                }
                return SyntaxFactory.List<T>(nodeList);
            }

            public static List<T> ToNodesList(List<MethodSortInfo<T>> sortInfoList)
            {
                List<T> nodeList = new List<T>();
                for (int i = 0; i < sortInfoList.Count; i++)
                {
                    nodeList.Add(sortInfoList[i].Node);
                }
                return nodeList;
            }

        }

        #endregion

        #region Method info comparer

        protected class MethodSortInfoComparer<T> : IComparer<MethodSortInfo<T>> where T : SyntaxNode
        {            
            private static Dictionary<ALSymbolKind, int> _typePriority;
            private static IComparer<string> _stringComparer = new SyntaxNodeNameComparer();
            private static int UndefinedPriority = -1;

            private TriggersOrderCollection _triggerNaturalOrder;
            private ConvertedSyntaxKind _parentKind;
            private bool _sortProcedures;
            private SortProceduresTriggerSortMode _triggerSortMode;

            public MethodSortInfoComparer(SortProceduresTriggerSortMode triggerSortMode, bool sortProcedures, TriggersOrderCollection triggersNaturalOrder, ConvertedSyntaxKind parentKind)
            {
                _triggerSortMode = triggerSortMode;
                _sortProcedures = sortProcedures;
                _parentKind = parentKind;
                _triggerNaturalOrder = triggersNaturalOrder;
                InitTypePriority();
            }

            private void InitTypePriority()
            {
                if (_typePriority == null)
                {
                    ALSymbolKind[] types = {
                        ALSymbolKind.TriggerDeclaration,
                        ALSymbolKind.TestDeclaration,
                        ALSymbolKind.ConfirmHandlerDeclaration,
                        ALSymbolKind.FilterPageHandlerDeclaration,
                        ALSymbolKind.HyperlinkHandlerDeclaration,
                        ALSymbolKind.MessageHandlerDeclaration,
                        ALSymbolKind.ModalPageHandlerDeclaration,
                        ALSymbolKind.PageHandlerDeclaration,
                        //ALSymbolKind.RecallNotificationHandler, // is missing
                        ALSymbolKind.ReportHandlerDeclaration,
                        ALSymbolKind.RequestPageHandlerDeclaration,
                        ALSymbolKind.SendNotificationHandlerDeclaration,
                        ALSymbolKind.SessionSettingsHandlerDeclaration,
                        ALSymbolKind.StrMenuHandlerDeclaration,
                        ALSymbolKind.MethodDeclaration,
                        ALSymbolKind.InternalMethodDeclaration,
                        ALSymbolKind.ProtectedMethodDeclaration,
                        ALSymbolKind.LocalMethodDeclaration,
                        ALSymbolKind.EventSubscriberDeclaration,
                        ALSymbolKind.EventDeclaration,
                        ALSymbolKind.BusinessEventDeclaration,
                        ALSymbolKind.IntegrationEventDeclaration
                    };
                    _typePriority = new Dictionary<ALSymbolKind, int>();

                    for (int i = 0; i < types.Length; i++)
                    {
                        if (_sortProcedures)
                            _typePriority.Add(types[i], i);
                        else if (types[i] == ALSymbolKind.TriggerDeclaration)
                            _typePriority.Add(types[i], 0);
                        else
                            _typePriority.Add(types[i], 1);
                    }
                }
            }

            protected int GetTypePriority(ALSymbolKind kind)
            {
                if ((kind == ALSymbolKind.TriggerDeclaration) && (_triggerSortMode == SortProceduresTriggerSortMode.None))
                    return UndefinedPriority;
                if (_typePriority.ContainsKey(kind))
                    return _typePriority[kind];
                return UndefinedPriority;
            }

            private int CompareTriggersByNaturalOrder(MethodSortInfo<T> x, MethodSortInfo<T> y)
            {
                if (_triggerNaturalOrder.TryCompare(_parentKind, x.Name, y.Name, out int result))
                    return result;
                return x.Index - y.Index;
            }

            public int Compare(MethodSortInfo<T> x, MethodSortInfo<T> y)
            {
                if ((_triggerSortMode != SortProceduresTriggerSortMode.None) || (_sortProcedures))
                {
                    //sort triggers
                    if ((x.Kind == ALSymbolKind.TriggerDeclaration) && (y.Kind == ALSymbolKind.TriggerDeclaration) && (_triggerSortMode == SortProceduresTriggerSortMode.NaturalOrder))
                        return CompareTriggersByNaturalOrder(x, y);

                    //check type
                    int xTypePriority = this.GetTypePriority(x.Kind);
                    int yTypePriority = this.GetTypePriority(y.Kind);
                    if (xTypePriority != yTypePriority)
                        return xTypePriority - yTypePriority;

                    //check name
                    if ((CanSortByName(x.Kind)) && (CanSortByName(y.Kind)))
                    {
                        int val = _stringComparer.Compare(x.Name, y.Name);
                        if (val != 0)
                            return val;
                    }
                }

                //check old index
                return x.Index - y.Index;
            }

            private bool CanSortByName(ALSymbolKind symbolKind)
            {
                if (symbolKind == ALSymbolKind.TriggerDeclaration)
                    return (_triggerSortMode == SortProceduresTriggerSortMode.Name);
                return _sortProcedures;
            }

        }

        #endregion

        public bool SortSingleNodeRegions { get; set; } = false;
        public SortProceduresTriggerSortMode TriggerSortMode { get; set; } = SortProceduresTriggerSortMode.None;
        public bool SortProcedures {  get; set; } = true;
        internal TriggersOrderCollection TriggersOrder { get; } = new TriggersOrderCollection();

        public SortProceduresSyntaxRewriter()
        {
        }

#region Visit objects

        public override SyntaxNode VisitTable(TableSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Members != null) && (node.Members.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newMembers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Members, node.CloseBraceToken);
                node = node.WithMembers(newMembers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitTable(node);
        }

        public override SyntaxNode VisitPage(PageSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Members != null) && (node.Members.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newMembers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Members, node.CloseBraceToken);
                node = node.WithMembers(newMembers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitPage(node);
        }

        public override SyntaxNode VisitReport(ReportSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Members != null) && (node.Members.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newMembers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Members, node.CloseBraceToken);
                node = node.WithMembers(newMembers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitReport(node);
        }

        public override SyntaxNode VisitXmlPort(XmlPortSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Members != null) && (node.Members.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newMembers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Members, node.CloseBraceToken);
                node = node.WithMembers(newMembers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitXmlPort(node);
        }

        public override SyntaxNode VisitCodeunit(CodeunitSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Members != null) && (node.Members.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newMembers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Members, node.CloseBraceToken);
                node = node.WithMembers(newMembers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitCodeunit(node);
        }

        public override SyntaxNode VisitQuery(QuerySyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Members != null) && (node.Members.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newMembers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Members, node.CloseBraceToken);
                node = node.WithMembers(newMembers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitQuery(node);
        }

        public override SyntaxNode VisitTableExtension(TableExtensionSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Members != null) && (node.Members.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newMembers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Members, node.CloseBraceToken);
                node = node.WithMembers(newMembers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitTableExtension(node);
        }

        public override SyntaxNode VisitPageExtension(PageExtensionSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Members != null) && (node.Members.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newMembers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Members, node.CloseBraceToken);
                node = node.WithMembers(newMembers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitPageExtension(node);
        }

#if BC
        public override SyntaxNode VisitInterface(InterfaceSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Members != null) && (node.Members.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newMembers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Members, node.CloseBraceToken);
                node = node.WithMembers(newMembers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitInterface(node);
        }
#endif

        public override SyntaxNode VisitPageCustomization(PageCustomizationSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Members != null) && (node.Members.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newMembers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Members, node.CloseBraceToken);
                node = node.WithMembers(newMembers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitPageCustomization(node);
        }

        public override SyntaxNode VisitProfile(ProfileSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Members != null) && (node.Members.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newMembers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Members, node.CloseBraceToken);
                node = node.WithMembers(newMembers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitProfile(node);
        }

        public override SyntaxNode VisitRequestPage(RequestPageSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Members != null) && (node.Members.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newMembers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Members, node.CloseBraceToken);
                node = node.WithMembers(newMembers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitRequestPage(node);
        }

#if BC
        public override SyntaxNode VisitRequestPageExtension(RequestPageExtensionSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Members != null) && (node.Members.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newMembers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Members, node.CloseBraceToken);
                node = node.WithMembers(newMembers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitRequestPageExtension(node);
        }

        public override SyntaxNode VisitReportExtension(ReportExtensionSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Members != null) && (node.Members.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newMembers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Members, node.CloseBraceToken);
                node = node.WithMembers(newMembers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitReportExtension(node);
        }
#endif

#endregion

        #region Visit nodes with triggers

        public override SyntaxNode VisitField(FieldSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Triggers != null) && (node.Triggers.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newTriggers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Triggers, node.CloseBraceToken);
                node = node.WithTriggers(newTriggers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitField(node);
        }

        public override SyntaxNode VisitPageField(PageFieldSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Triggers != null) && (node.Triggers.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newTriggers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Triggers, node.CloseBraceToken);
                node = node.WithTriggers(newTriggers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitPageField(node);
        }

        public override SyntaxNode VisitPageAction(PageActionSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Triggers != null) && (node.Triggers.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newTriggers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Triggers, node.CloseBraceToken);
                node = node.WithTriggers(newTriggers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitPageAction(node);
        }

        public override SyntaxNode VisitXmlPortFieldAttribute(XmlPortFieldAttributeSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Triggers != null) && (node.Triggers.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newTriggers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Triggers, node.CloseBraceToken);
                node = node.WithTriggers(newTriggers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitXmlPortFieldAttribute(node);
        }

        public override SyntaxNode VisitXmlPortFieldElement(XmlPortFieldElementSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Triggers != null) && (node.Triggers.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newTriggers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Triggers, node.CloseBraceToken);
                node = node.WithTriggers(newTriggers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitXmlPortFieldElement(node);
        }

        public override SyntaxNode VisitXmlPortTableElement(XmlPortTableElementSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Triggers != null) && (node.Triggers.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newTriggers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Triggers, node.CloseBraceToken);
                node = node.WithTriggers(newTriggers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitXmlPortTableElement(node);
        }

        public override SyntaxNode VisitXmlPortTextAttribute(XmlPortTextAttributeSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Triggers != null) && (node.Triggers.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newTriggers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Triggers, node.CloseBraceToken);
                node = node.WithTriggers(newTriggers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitXmlPortTextAttribute(node);
        }

        public override SyntaxNode VisitXmlPortTextElement(XmlPortTextElementSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Triggers != null) && (node.Triggers.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newTriggers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Triggers, node.CloseBraceToken);
                node = node.WithTriggers(newTriggers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitXmlPortTextElement(node);
        }

        public override SyntaxNode VisitControlModifyChange(ControlModifyChangeSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Triggers != null) && (node.Triggers.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newTriggers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Triggers, node.CloseBraceToken);
                node = node.WithTriggers(newTriggers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitControlModifyChange(node);
        }

        public override SyntaxNode VisitActionModifyChange(ActionModifyChangeSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Triggers != null) && (node.Triggers.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newTriggers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Triggers, node.CloseBraceToken);
                node = node.WithTriggers(newTriggers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitActionModifyChange(node);
        }

        public override SyntaxNode VisitFieldModification(FieldModificationSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Triggers != null) && (node.Triggers.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newTriggers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Triggers, node.CloseBraceToken);
                node = node.WithTriggers(newTriggers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitFieldModification(node);
        }

        public override SyntaxNode VisitReportDataItem(ReportDataItemSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Triggers != null) && (node.Triggers.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newTriggers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Triggers, node.CloseBraceToken);
                node = node.WithTriggers(newTriggers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitReportDataItem(node);
        }

#if BC
        public override SyntaxNode VisitReportExtensionDataSetModify(ReportExtensionDataSetModifySyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Triggers != null) && (node.Triggers.Count > 0) && (!node.ContainsDiagnostics))
            {
                (var newTriggers, var newClosingToken, var closingTokenModified) = this.Sort(node, node.Triggers, node.CloseBraceToken);
                node = node.WithTriggers(newTriggers);
                if (closingTokenModified)
                    node = node.WithCloseBraceToken(newClosingToken);
            }
            return base.VisitReportExtensionDataSetModify(node);
        }
#endif

#endregion

        private (SyntaxList<T>, SyntaxToken, bool) Sort<T>(SyntaxNode parent, SyntaxList<T> members, SyntaxToken closingToken) where T: SyntaxNode
        {
            if (members.Count <= 1)
                return (members, closingToken, false);

            //build list with regions
            SyntaxNodesGroupsTree<T> nodesGroupsTree = new SyntaxNodesGroupsTree<T>();
            (_, var newClosingToken, var closingTokenModified) = nodesGroupsTree.AddNodes(members, SortSingleNodeRegions, closingToken);
            
            //somethis went wrong - do not sort
            if (nodesGroupsTree.Root == null)
                return (members, closingToken, false);

            MethodSortInfoComparer<T> comparer = new MethodSortInfoComparer<T>(TriggerSortMode, SortProcedures, TriggersOrder, parent.Kind.ConvertToLocalType());

            //does not have any child groups
            if (!nodesGroupsTree.Root.HasChildGroups)
            {
                List<MethodSortInfo<T>> list = MethodSortInfo<T>.FromSyntaxList(members);
                if (!list.IsOrdered(comparer))
                    this.NoOfChanges++;
                list.Sort(comparer);
                return (MethodSortInfo<T>.ToSyntaxList(list), newClosingToken.Value, closingTokenModified);
            }

            //has child groups - sort them separately
            List<SyntaxNodesGroup<T>> allGroups = nodesGroupsTree.GetAllGroups();
            foreach (SyntaxNodesGroup<T> nodesGroup in allGroups)
            {
                if (nodesGroup.SyntaxNodes.Count > 1)
                {
                    List<MethodSortInfo<T>> list = MethodSortInfo<T>.FromNodesList(nodesGroup.SyntaxNodes);
                    if (!list.IsOrdered(comparer))
                        this.NoOfChanges++;
                    list.Sort(comparer);
                    nodesGroup.SyntaxNodes = MethodSortInfo<T>.ToNodesList(list);
                }
            }

            //return sorted nodes
            return (nodesGroupsTree.CreateSyntaxList(), newClosingToken.Value, closingTokenModified);
        }

    }
}
