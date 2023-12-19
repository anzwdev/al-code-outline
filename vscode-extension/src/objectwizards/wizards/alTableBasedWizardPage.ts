import { ProjectItemWizardPage } from './projectItemWizardPage';
import { ALTableBasedWizardData } from './alTableBasedWizardData';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { ALObjectWizardSettings } from './alObjectWizardSettings';
import { ToolsSymbolInformationRequest } from '../../langserver/symbolsinformation/toolsSymbolInformationRequest';
import { ToolsGetTableFieldsListRequest } from '../../langserver/symbolsinformation/toolsGetTableFieldsListRequest';
import { TableFieldInformation } from '../../symbolsinformation/tableFieldInformation';
import { TableFieldInformationHelper } from '../../symbolsinformation/tableFieldInformationHelper';
import { TableFieldClass } from '../../symbolsinformation/tableFieldClass';

export class ALTableBasedWizardPage extends ProjectItemWizardPage {
    private _tableWizardData : ALTableBasedWizardData;
    protected _includeToolTips : boolean;
    protected _includeFlowFilters : boolean;
    protected _toolTipsSourceDependencies : string[] | undefined;

    constructor(toolsExtensionContext : DevToolsExtensionContext, title : string, settings: ALObjectWizardSettings, data : ALTableBasedWizardData) {
        super(toolsExtensionContext, title, settings, data);
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
            (this._tableWizardData.tableList == null) || (this._tableWizardData.tableList.length == 0)))
            this.loadTables();
    }

    protected async loadTables() {
        let tableList: string[] = [];
        let response = await this._toolsExtensionContext.toolsLangServerClient.getTablesList(
            new ToolsSymbolInformationRequest(this._settings.getDestDirectoryPath(), false));
        if ((response) && (response.symbols)) {
            for (let i=0; i<response.symbols.length; i++) {
                let name = response.symbols[i].name;
                if (name)
                    tableList.push(name);
            }
        }    

        this._tableWizardData.tableList = tableList;

        this.sendMessage({
            command : "setTables",
            data : this._tableWizardData.tableList
        });
    }

    private updateFieldsDescriptions(fieldList: TableFieldInformation[] | undefined) {
        if (fieldList) {
            for (let i=0; i<fieldList.length; i++) {
                this.updateFieldDescription(fieldList[i]);
            }
        }
    }

    private updateFieldDescription(field: TableFieldInformation) {
        field.uiDesc = TableFieldInformationHelper.getFieldUIDesc(field);
    }

    protected async loadFields() {
        try
        {
            //this._tableWizardData.fieldList = await this._toolsExtensionContext.alLangProxy.getFieldList(this._settings.getDestDirectoryUri(), this._tableWizardData.selectedTable);
            
            let fieldList: string[] = [];
            let response = await this._toolsExtensionContext.toolsLangServerClient.getTableFieldsList(
                new ToolsGetTableFieldsListRequest(this._settings.getDestDirectoryPath(), 
                this._tableWizardData.selectedTable!, false, false, true, true, this._includeFlowFilters, this._includeToolTips, this._toolTipsSourceDependencies));
            if ((response) && (response.symbols)) {
                for (let i=0; i<response.symbols.length; i++) {
                    let name = response.symbols[i].name;
                    if (name)
                        fieldList.push(name);
                }
                
                if (this._includeFlowFilters) {                    
                    let fields: TableFieldInformation[] = [];
                    let flowFilters: TableFieldInformation[] = [];
                    for (let i=0; i<response.symbols.length; i++)
                        if (response.symbols[i].fieldClass == TableFieldClass.FlowFilter)
                            flowFilters.push(response.symbols[i]);
                        else
                            fields.push(response.symbols[i]);
                    this._tableWizardData.fieldList = fields;
                    this._tableWizardData.flowFiltersList = flowFilters;
                } else {
                    this._tableWizardData.fieldList = response.symbols;
                    this._tableWizardData.flowFiltersList = [];
                }

                this.updateFieldsDescriptions(this._tableWizardData.fieldList);
                this.updateFieldsDescriptions(this._tableWizardData.flowFiltersList);
            }        
            //this._tableWizardData.fieldList = fieldList;
            
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

    protected setTable(tableName : string, includeFlowFilters: boolean) {
        var fieldChanged = 
            (this._tableWizardData.selectedTable != tableName) ||
            (this._includeFlowFilters != includeFlowFilters);

        this._tableWizardData.selectedTable = {
            name: tableName
        };
        this._includeFlowFilters = includeFlowFilters;
        if ((fieldChanged) || (!this._tableWizardData.fieldList) || (this._tableWizardData.fieldList.length == 0))
            this.loadFields();
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message))
            return true;

        switch (message.command) {
            case "selectTable":
                this.setTable(message.tableName, !!message.includeFlowFilters);
                return true;
        }
        
        return false;
    }
    
}