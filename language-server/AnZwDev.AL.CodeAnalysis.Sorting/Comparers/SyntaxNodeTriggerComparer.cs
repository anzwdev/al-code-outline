using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.CodeAnalysis.Sorting.Comparers
{
    public class SyntaxNodeTriggerComparer
    {

        private readonly Dictionary<SyntaxKind, TriggerComparer> _nodeTriggers = new Dictionary<SyntaxKind, TriggerComparer>();

        public SyntaxNodeTriggerComparer()
        {
            SetDefault();
        }

        public void Add(TriggerComparer triggersOrder)
        {
            if (_nodeTriggers.ContainsKey(triggersOrder.Kind))
                _nodeTriggers[triggersOrder.Kind] = triggersOrder;
            else
                _nodeTriggers.Add(triggersOrder.Kind, triggersOrder);
        }

        public void AddRange(IEnumerable<TriggerComparer> triggersOrders)
        {
            foreach (var trigger in triggersOrders)
                Add(trigger);
        }

        public bool TryCompare(SyntaxKind kind, string nameX, string nameY, out int result)
        {
            if (_nodeTriggers.ContainsKey(kind))
            {
                result = _nodeTriggers[kind].Compare(nameX, nameY);
                return true;
            }
            result = 0;
            return false;
        }

        public void Clear()
        {
            _nodeTriggers.Clear();
        }

        public void SetDefault()
        {
            Clear();
            Add(new TriggerComparer(SyntaxKind.CodeunitObject, new string[]{
                "OnRun",
                "OnCheckPreconditionsPerDatabase",
                "OnCheckPreconditionsPerCompany",
                "OnUpgradePerDatabase",
                "OnUpgradePerCompany",
                "OnValidateUpgradePerDatabase",
                "OnValidateUpgradePerCompany",
                "OnInstallAppPerDatabase",
                "OnInstallAppPerCompany",
                "OnBeforeTestRun",
                "OnAfterTestRun"
            }));
            Add(new TriggerComparer(SyntaxKind.TableObject, new string[]{
                "OnInsert",
                "OnModify",
                "OnDelete",
                "OnRename"
            }));
            Add(new TriggerComparer(SyntaxKind.TableExtensionObject, new string[]{
                "OnBeforeInsert",
                "OnInsert",
                "OnAfterInsert",
                "OnBeforeModify",
                "OnModify",
                "OnAfterModify",
                "OnBeforeDelete",
                "OnDelete",
                "OnAfterDelete",
                "OnBeforeRename",
                "OnRename",
                "OnAfterRename"
            }));
            Add(new TriggerComparer(SyntaxKind.Field, new string[]{
                "OnValidate",
                "OnLookup"
            }));
            Add(new TriggerComparer(SyntaxKind.FieldModification, new string[]{
                "OnBeforeValidate",
                "OnAfterValidate"
            }));
            Add(new TriggerComparer(SyntaxKind.PageObject, new string[]{
                "OnInit",
                "OnOpenPage",
                "OnClosePage",
                "OnFindRecord",
                "OnNextRecord",
                "OnAfterGetRecord",
                "OnNewRecord",
                "OnInsertRecord",
                "OnModifyRecord",
                "OnDeleteRecord",
                "OnQueryClosePage",
                "OnAfterGetCurrRecord",
                "OnPageBackgroundTaskCompleted",
                "OnPageBackgroundTaskError"
            }));
            Add(new TriggerComparer(SyntaxKind.RequestPage, new string[]{
                "OnInit",
                "OnOpenPage",
                "OnClosePage",
                "OnFindRecord",
                "OnNextRecord",
                "OnAfterGetRecord",
                "OnNewRecord",
                "OnInsertRecord",
                "OnModifyRecord",
                "OnDeleteRecord",
                "OnQueryClosePage",
                "OnAfterGetCurrRecord"
            }));
            Add(new TriggerComparer(SyntaxKind.RequestPageExtension, new string[]{
                "OnOpenPage",
                "OnClosePage",
                "OnAfterGetRecord",
                "OnNewRecord",
                "OnInsertRecord",
                "OnModifyRecord",
                "OnDeleteRecord",
                "OnQueryClosePage",
                "OnAfterGetCurrRecord"
            }));
            Add(new TriggerComparer(SyntaxKind.PageExtensionObject, new string[]{
                "OnOpenPage",
                "OnClosePage",
                "OnAfterGetRecord",
                "OnNewRecord",
                "OnInsertRecord",
                "OnModifyRecord",
                "OnDeleteRecord",
                "OnQueryClosePage",
                "OnAfterGetCurrRecord",
                "OnPageBackgroundTaskCompleted",
                "OnPageBackgroundTaskError"
            }));
            Add(new TriggerComparer(SyntaxKind.PageField, new string[]
            {
                "OnValidate",
                "OnLookup",
                "OnAfterLookup",
                "OnDrillDown",
                "OnAssistEdit",
                "OnControlAddIn"
            }));
            Add(new TriggerComparer(SyntaxKind.ControlModifyChange, new string[]
            {
                "OnBeforeValidate",
                "OnAfterValidate",
                "OnLookup",
                "OnDrillDown",
                "OnAssistEdit",
                "OnAfterAfterLookup"
            }));
            Add(new TriggerComparer(SyntaxKind.PageAction, new string[]{
                "OnAction"
            }));
            Add(new TriggerComparer(SyntaxKind.ActionModifyChange, new string[]{
                "OnBeforeAction",
                "OnAfterAction"
            }));
            Add(new TriggerComparer(SyntaxKind.ReportObject, new string[]{
                "OnInitReport", 
                "OnPreReport",
                "OnPostReport"
            }));
            Add(new TriggerComparer(SyntaxKind.ReportExtension, new string[]{
                "OnPreReport",
                "OnPostReport"
            }));
            Add(new TriggerComparer(SyntaxKind.ReportDataItem, new string[]{
                "OnPreDataItem",
                "OnAfterGetRecord",
                "OnPostDataItem"
            }));
            Add(new TriggerComparer(SyntaxKind.ReportExtensionDataSetModify, new string[]{
                "OnBeforePreDataItem",
                "OnAfterPreDataItem",
                "OnBeforeAfterGetRecord",
                "OnAfterAfterGetRecord",
                "OnBeforePostDataItem",
                "OnAfterPostDataItem"
            }));
            Add(new TriggerComparer(SyntaxKind.XmlPortObject, new string[]{
                "OnInitXmlPort",
                "OnPreXmlPort",
                "OnPostXmlPort"
            }));
            Add(new TriggerComparer(SyntaxKind.XmlPortTableElement, new string[]{
                "OnAfterInitRecord",
                "OnBeforeInsertRecord",
                "OnAfterInsertRecord",
                "OnBeforeModifyRecord",
                "OnAfterModifyRecord",
                "OnPreXmlItem",
                "OnAfterGetRecord"
            }));
            Add(new TriggerComparer(SyntaxKind.XmlPortFieldElement, new string[]{
                "OnAfterAssignField",
                "OnBeforePassField"
            }));
            Add(new TriggerComparer(SyntaxKind.XmlPortTextElement, new string[]{
                "OnAfterAssignVariable",
                "OnBeforePassVariable"
            }));
            Add(new TriggerComparer(SyntaxKind.XmlPortFieldAttribute, new string[]{
                "OnAfterAssignField",
                "OnBeforePassField"
            }));
            Add(new TriggerComparer(SyntaxKind.XmlPortTextAttribute, new string[]{
                "OnAfterAssignVariable",
                "OnBeforePassVariable"
            }));
            Add(new TriggerComparer(SyntaxKind.QueryObject, new string[]{
                "OnBeforeOpen"
            }));
        }

    }
}
