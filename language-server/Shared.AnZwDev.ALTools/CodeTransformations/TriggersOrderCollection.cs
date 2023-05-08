using AnZwDev.ALTools.ALSymbols.Internal;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    internal class TriggersOrderCollection
    {

        private readonly Dictionary<ConvertedSyntaxKind, TriggersOrder> _nodeTriggers = new Dictionary<ConvertedSyntaxKind, TriggersOrder>();

        public TriggersOrderCollection()
        {
            SetDefault();
        }

        public void Add(TriggersOrder triggersOrder)
        {
            if (_nodeTriggers.ContainsKey(triggersOrder.Kind))
                _nodeTriggers[triggersOrder.Kind] = triggersOrder;
            else
                _nodeTriggers.Add(triggersOrder.Kind, triggersOrder);
        }

        public void AddRange(IEnumerable<TriggersOrder> triggersOrders)
        {
            foreach (var trigger in triggersOrders)
                Add(trigger);
        }

        public bool TryCompare(ConvertedSyntaxKind kind, string nameX, string nameY, out int result)
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
            this.Clear();
            this.Add(new TriggersOrder(ConvertedSyntaxKind.CodeunitObject, new string[]{
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
            this.Add(new TriggersOrder(ConvertedSyntaxKind.TableObject, new string[]{
                "OnInsert",
                "OnModify",
                "OnDelete",
                "OnRename"
            }));
            this.Add(new TriggersOrder(ConvertedSyntaxKind.TableExtensionObject, new string[]{
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
            this.Add(new TriggersOrder(ConvertedSyntaxKind.Field, new string[]{
                "OnValidate",
                "OnLookup"
            }));
            this.Add(new TriggersOrder(ConvertedSyntaxKind.FieldModification, new string[]{
                "OnBeforeValidate",
                "OnAfterValidate"
            }));
            this.Add(new TriggersOrder(ConvertedSyntaxKind.PageObject, new string[]{
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
            this.Add(new TriggersOrder(ConvertedSyntaxKind.RequestPage, new string[]{
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
            this.Add(new TriggersOrder(ConvertedSyntaxKind.RequestPageExtension, new string[]{
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
            this.Add(new TriggersOrder(ConvertedSyntaxKind.PageExtensionObject, new string[]{
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
            this.Add(new TriggersOrder(ConvertedSyntaxKind.PageField, new string[]
            {
                "OnValidate",
                "OnLookup",
                "OnAfterLookup",
                "OnDrillDown",
                "OnAssistEdit",
                "OnControlAddIn"
            }));
            this.Add(new TriggersOrder(ConvertedSyntaxKind.ControlModifyChange, new string[]
            {
                "OnBeforeValidate",
                "OnAfterValidate",
                "OnLookup",
                "OnDrillDown",
                "OnAssistEdit",
                "OnAfterAfterLookup"
            }));
            this.Add(new TriggersOrder(ConvertedSyntaxKind.PageAction, new string[]{
                "OnAction"
            }));
            this.Add(new TriggersOrder(ConvertedSyntaxKind.ActionModifyChange, new string[]{
                "OnBeforeAction",
                "OnAfterAction"
            }));
            this.Add(new TriggersOrder(ConvertedSyntaxKind.ReportObject, new string[]{
                "OnInitReport",
                "OnPreReport",
                "OnPostReport"
            }));
            this.Add(new TriggersOrder(ConvertedSyntaxKind.ReportExtension, new string[]{
                "OnPreReport",
                "OnPostReport"
            }));
            this.Add(new TriggersOrder(ConvertedSyntaxKind.ReportDataItem, new string[]{
                "OnPreDataItem",
                "OnAfterGetRecord",
                "OnPostDataItem"
            }));
            this.Add(new TriggersOrder(ConvertedSyntaxKind.ReportExtensionDataSetModify, new string[]{
                "OnBeforePreDataItem",
                "OnAfterPreDataItem",
                "OnBeforeAfterGetRecord",
                "OnAfterAfterGetRecord",
                "OnBeforePostDataItem",
                "OnAfterPostDataItem"
            }));
            this.Add(new TriggersOrder(ConvertedSyntaxKind.XmlPortObject, new string[]{
                "OnInitXmlPort",
                "OnPreXmlPort",
                "OnPostXmlPort"
            }));
            this.Add(new TriggersOrder(ConvertedSyntaxKind.XmlPortTableElement, new string[]{
                "OnAfterInitRecord",
                "OnBeforeInsertRecord",
                "OnAfterInsertRecord",
                "OnBeforeModifyRecord",
                "OnAfterModifyRecord",
                "OnPreXmlItem",
                "OnAfterGetRecord"
            }));
            this.Add(new TriggersOrder(ConvertedSyntaxKind.XmlPortFieldElement, new string[]{
                "OnAfterAssignField",
                "OnBeforePassField"
            }));
            this.Add(new TriggersOrder(ConvertedSyntaxKind.XmlPortTextElement, new string[]{
                "OnAfterAssignVariable",
                "OnBeforePassVariable"
            }));
            this.Add(new TriggersOrder(ConvertedSyntaxKind.XmlPortFieldAttribute, new string[]{
                "OnAfterAssignField",
                "OnBeforePassField"
            }));
            this.Add(new TriggersOrder(ConvertedSyntaxKind.XmlPortTextAttribute, new string[]{
                "OnAfterAssignVariable",
                "OnBeforePassVariable"
            }));
            this.Add(new TriggersOrder(ConvertedSyntaxKind.QueryObject, new string[]{
                "OnBeforeOpen"
            }));
        }

    }
}
