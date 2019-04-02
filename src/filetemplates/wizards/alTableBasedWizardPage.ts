'use strict';

import * as vzFileTemplates from 'vz-file-templates';
import { ProjectItemWizardPage } from './projectItemWizardPage';
import { ALTableBasedWizardData } from './alTableBasedWizardData';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';

export class ALTableBasedWizardPage extends ProjectItemWizardPage {
    private _tableWizardData : ALTableBasedWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, title : string, template : vzFileTemplates.IProjectItemTemplate, settings : vzFileTemplates.IProjectItemTemplateRunSettings, data : ALTableBasedWizardData) {
        super(toolsExtensionContext, title, template, settings);
        this._tableWizardData = data;
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
        this._tableWizardData.tableList = await this._toolsExtensionContext.alLangProxy.getTableList();
        this.sendMessage({
            command : "setTables",
            data : this._tableWizardData.tableList
        });
    }

    protected async loadFields() {
        try
        {
            this._tableWizardData.fieldList = await this._toolsExtensionContext.alLangProxy.getFieldList(this._tableWizardData.selectedTable);
            this.sendMessage({
                command: "setFields",
                data : this._tableWizardData.fieldList
            });
        }
        catch (e)
        {
            
        }
    }

    protected setTable(tableName : string) {
        var fieldChanged = (this._tableWizardData.selectedTable != tableName);

        this._tableWizardData.selectedTable = tableName;
        if ((fieldChanged) || (!this._tableWizardData.fieldList) || (this._tableWizardData.fieldList.length == 0))
            this.loadFields();
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message))
            return true;

        switch (message.command) {
            case "selectTable":
                this.setTable(message.tableName);
                return true;
        }
        
        return false;
    }


    
}