using AnZwDev.AL.Syntax;
using AnZwDev.AL.Symbols;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;

namespace AnZwDev.AL.CodeAnalysis.Sorting.Comparers
{
    public class MethodSortInfoComparer<T> : IComparer<MethodSortInfo<T>> where T : MemberSyntax
    {
        private static int UndefinedPriority = -1;

        private Dictionary<MemberKind, int> _typePriority;
        private IComparer<string> _stringComparer = new SyntaxNodeNameComparer();
        private SyntaxNodeTriggerComparer _syntaxNodeTriggerComparer;
        private SyntaxKind _parentKind;
        private bool _sortProcedures;
        private SortProceduresTriggerSortMode _triggerSortMode;
        private SortProceduresGlobalVariablesSortMode _globalVariablesSortMode;

        public MethodSortInfoComparer(SortProceduresTriggerSortMode triggerSortMode, bool sortProcedures, SyntaxNodeTriggerComparer syntaxNodeTriggerComparer, SyntaxKind parentKind, SortProceduresGlobalVariablesSortMode globalVariablesSortMode)
        {
            _triggerSortMode = triggerSortMode;
            _sortProcedures = sortProcedures;
            _parentKind = parentKind;
            _syntaxNodeTriggerComparer = syntaxNodeTriggerComparer;
            _globalVariablesSortMode = globalVariablesSortMode;
            _typePriority = new Dictionary<MemberKind, int>();

            InitTypePriority();
        }

        private void InitTypePriority()
        {
            MemberKind[] types = {
                MemberKind.TestDeclaration,
                MemberKind.ConfirmHandlerDeclaration,
                MemberKind.FilterPageHandlerDeclaration,
                MemberKind.HyperlinkHandlerDeclaration,
                MemberKind.MessageHandlerDeclaration,
                MemberKind.ModalPageHandlerDeclaration,
                MemberKind.PageHandlerDeclaration,
                //MemberKind.RecallNotificationHandler, // is missing
                MemberKind.ReportHandlerDeclaration,
                MemberKind.RequestPageHandlerDeclaration,
                MemberKind.SendNotificationHandlerDeclaration,
                MemberKind.SessionSettingsHandlerDeclaration,
                MemberKind.StrMenuHandlerDeclaration,
                MemberKind.MethodDeclaration,
                MemberKind.InternalMethodDeclaration,
                MemberKind.ProtectedMethodDeclaration,
                MemberKind.LocalMethodDeclaration,
                MemberKind.EventSubscriberDeclaration,
                MemberKind.EventDeclaration,
                MemberKind.BusinessEventDeclaration,
                MemberKind.ExternalBusinessEventDeclaration,
                MemberKind.IntegrationEventDeclaration,
                MemberKind.InternalEventDeclaration
            };

            var priorityValue = 0;
            if (_globalVariablesSortMode == SortProceduresGlobalVariablesSortMode.First)
            {
                _typePriority.Add(MemberKind.GlobalVarSection, priorityValue);
                priorityValue++;
            }

            _typePriority.Add(MemberKind.TriggerDeclaration, priorityValue);
            priorityValue++;

            if (_globalVariablesSortMode == SortProceduresGlobalVariablesSortMode.AfterTriggers)
            {
                _typePriority.Add(MemberKind.GlobalVarSection, priorityValue);
                priorityValue++;
            }

            for (int i = 0; i < types.Length; i++)
            {
                _typePriority.Add(types[i], priorityValue);
                if (_sortProcedures)
                    priorityValue++;
            }
            if (!_sortProcedures)
                priorityValue++;

            if (_globalVariablesSortMode == SortProceduresGlobalVariablesSortMode.Last)
            {
                _typePriority.Add(MemberKind.GlobalVarSection, priorityValue);
                priorityValue++;
            }
        }

        protected int GetTypePriority(MemberKind kind, MemberKind otherSymbolKind)
        {
            if (kind == MemberKind.TriggerDeclaration && _triggerSortMode == SortProceduresTriggerSortMode.None && otherSymbolKind != MemberKind.GlobalVarSection)
                return UndefinedPriority;
            if (_typePriority.ContainsKey(kind))
                return _typePriority[kind];
            return UndefinedPriority;
        }

        private int CompareTriggersByNaturalOrder(MethodSortInfo<T> x, MethodSortInfo<T> y)
        {
            if (_syntaxNodeTriggerComparer.TryCompare(_parentKind, x.Name, y.Name, out int result))
                return result;
            return x.Index - y.Index;
        }

        public int Compare(MethodSortInfo<T>? x, MethodSortInfo<T>? y)
        {
            if (x == null)
            {
                if (y == null)
                    return 0;
                return -1;
            }
            if (y == null)
                return 1;

            if (_triggerSortMode != SortProceduresTriggerSortMode.None || _sortProcedures)
            {

                //sort triggers
                if (x.Kind == MemberKind.TriggerDeclaration && y.Kind == MemberKind.TriggerDeclaration && _triggerSortMode == SortProceduresTriggerSortMode.NaturalOrder)
                    return CompareTriggersByNaturalOrder(x, y);

                //check type
                int xTypePriority = GetTypePriority(x.Kind, y.Kind);
                int yTypePriority = GetTypePriority(y.Kind, x.Kind);
                if (xTypePriority != yTypePriority)
                    return xTypePriority - yTypePriority;

                //check name
                if (CanSortByName(x.Kind) && CanSortByName(y.Kind))
                {
                    int val = _stringComparer.Compare(x.Name, y.Name);
                    if (val != 0)
                        return val;
                }
            }

            //check old index
            return x.Index - y.Index;
        }

        private bool CanSortByName(MemberKind symbolKind)
        {
            if (symbolKind == MemberKind.TriggerDeclaration)
                return _triggerSortMode == SortProceduresTriggerSortMode.Name;
            return _sortProcedures;
        }

    }
}
