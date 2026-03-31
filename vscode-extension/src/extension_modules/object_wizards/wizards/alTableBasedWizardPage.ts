import { ALObjectWizardPage } from './alObjectWizardPage';
import { ALTableBasedWizardData } from './alTableBasedWizardData';
import { ALObjectWizardSettings } from './alObjectWizardSettings';
import { DevToolsExtensionContext } from '../../../devToolsExtensionContext';
import { LSObjectKind } from '../../../langserver/common_types/lsObjectKind';
import { LSTableFieldClass } from '../../../langserver/common_types/lsTableFieldClass';
import { LSPIObjectIdentifier } from '../../../langserver/project_information/symbols/lspiObjectIdentifier';
import { LSPIObjectListItem } from '../../../langserver/project_information/symbols/lspiObjectListItem';
import { LSPIObjectListItemHelper } from '../../../langserver/project_information/symbols/lspiObjectListItemHelper';

export class ALTableBasedWizardPage extends ALObjectWizardPage {
    private _tableWizardData : ALTableBasedWizardData;
    protected _includeToolTips : boolean;
    protected _includeFlowFilters : boolean;
    protected _toolTipsSourceDependencies : string[] | undefined;

    constructor(toolsExtensionContext : DevToolsExtensionContext, title : string, viewType: string, settings: ALObjectWizardSettings, data : ALTableBasedWizardData) {
        super(toolsExtensionContext, title, viewType, settings, data);
        this._tableWizardData = data;
        this._includeToolTips = false;
        this._includeFlowFilters = false;
        this._toolTipsSourceDependencies = undefined;
    }

    //initialize wizard
    protected onDocumentLoaded() {
        //send data to the web view
        this.sendMessage({
            command : 'setData',
            data : this._tableWizardData
        });

        //load tables
        if ((!this._tableWizardData.fixedTable) && (
            (!this._tableWizardData.tableList) || (this._tableWizardData.tableList.length === 0))) {
                this.loadTables();
            }
    }

    protected async loadTables() {
        this._tableWizardData.tableList = await this.context.projectInformationService.getObjectList(this._settings.destDirectoryUri, LSObjectKind.Table);

        this.sendMessage({
            command : "setTables",
            data : this._tableWizardData.tableList
        });
    }

    protected async loadFields() {
        try
        {
            this._tableWizardData.fieldList = [];
            this._tableWizardData.flowFiltersList = [];

            if (this._tableWizardData.selectedTable) {
                
                //get field class filter
                let fieldClassFilter: LSTableFieldClass[]  = [];
                fieldClassFilter.push(LSTableFieldClass.Normal);
                fieldClassFilter.push(LSTableFieldClass.FlowField);
                if (this._includeFlowFilters) {
                    fieldClassFilter.push(LSTableFieldClass.FlowFilter);
                }

                let fieldList = await this.context.projectInformationService.getTableFieldsList(
                    this._settings.destDirectoryUri, 
                    this._tableWizardData.selectedTable, 
                    fieldClassFilter,
                    this._includeToolTips,
                    this._toolTipsSourceDependencies);

                if (this._includeFlowFilters) {
                    this._tableWizardData.fieldList = [];
                    if (fieldList) {
                        for (let i=0; i<fieldList.length; i++) {
                            if (fieldList[i].class === LSTableFieldClass.FlowFilter) {
                                this._tableWizardData.flowFiltersList.push(fieldList[i]);
                            } else {
                                this._tableWizardData.fieldList.push(fieldList[i]);
                            }
                        }
                    }
                } else {
                    this._tableWizardData.fieldList = fieldList;
                }
            }
            
            this.sendMessage({
                command: "setFields",
                data : {
                    fieldList: this._tableWizardData.fieldList,
                    flowFilterList: this._tableWizardData.flowFiltersList
                }
            });
        }
        catch (e)
        {
        }
    }

    protected setTable(table: LSPIObjectIdentifier | undefined, includeFlowFilters: boolean) {
        var fieldChanged = 
            (this._tableWizardData.selectedTable?.id !== table?.id) ||
            (this._tableWizardData.selectedTable?.name !== table?.name) ||
            (this._tableWizardData.selectedTable?.namespace !== table?.namespace) ||
            (this._includeFlowFilters !== includeFlowFilters);

        this._tableWizardData.selectedTable = table;
        this._includeFlowFilters = includeFlowFilters;
        if ((fieldChanged) || (!this._tableWizardData.fieldList) || (this._tableWizardData.fieldList.length === 0)) {
            this.loadFields();
        }
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message)) {
            return true;
        }

        switch (message.command) {
            case "selectTable":
                if (message.selectedTable) {
                    let selectedTableListItem: LSPIObjectListItem | undefined = message.selectedTable;
                    let selectedTable = LSPIObjectListItemHelper.toObjectIdentifierOrUndefined(selectedTableListItem);
                    this.setTable(selectedTable, !!message.includeFlowFilters);
                }
                return true;
        }
        
        return false;
    }
    
}