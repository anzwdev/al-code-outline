'use strict';

import * as path from 'path';
import { ProjectItemWizardPage } from './projectItemWizardPage';
import { ALEnumExtWizardData } from './alEnumExtWizardData';
import { ALEnumExtSyntaxBuilder } from '../syntaxbuilders/alEnumExtSyntaxBuilder';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { ALObjectWizardSettings } from './alObjectWizardSettings';

export class ALEnumExtWizardPage extends ProjectItemWizardPage {
    private _enumExtWizardData : ALEnumExtWizardData;
    
    constructor(toolsExtensionContext : DevToolsExtensionContext, settings: ALObjectWizardSettings, data : ALEnumExtWizardData) {
        super(toolsExtensionContext, "AL Enum Extension Wizard", settings);
        this._enumExtWizardData = data;
    }

    //initialize wizard
    protected onDocumentLoaded() {
        //send data to the web view
        this.sendMessage({
            command : 'setData',
            data : this._enumExtWizardData
        });

        //load base enums
        if ((this._enumExtWizardData.baseEnumList == null) || (this._enumExtWizardData.baseEnumList.length == 0))
            this.loadBaseEnums();
    }

    protected async loadBaseEnums() {
        this._enumExtWizardData.baseEnumList = await this._toolsExtensionContext.alLangProxy.getEnumList(this._settings.getDestDirectoryUri());
        this.sendMessage({
            command : "setEnums",
            data : this._enumExtWizardData.baseEnumList
        });
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'alenumextwizard', 'alenumextwizard.html');
    }

    protected getViewType() : string {
        return "azALDevTools.ALEnumExtWizard";
    }

    protected async finishWizard(data : any) : Promise<boolean> {
        //build parameters
        this._enumExtWizardData.objectId = data.objectId;
        this._enumExtWizardData.objectName = data.objectName;
        this._enumExtWizardData.baseEnum = data.baseEnum;
        this._enumExtWizardData.valueList = data.valueList;
        this._enumExtWizardData.captionList = data.captionList;

        let firstValueId = Number.parseInt(data.firstValueId);
        if (Number.isNaN(firstValueId))
            this._enumExtWizardData.firstValueId = 0;
        else
            this._enumExtWizardData.firstValueId = firstValueId;

        //build new object
        let builder : ALEnumExtSyntaxBuilder = new ALEnumExtSyntaxBuilder();
        let source = builder.buildFromEnumExtWizardData(this._settings.getDestDirectoryUri(), this._enumExtWizardData);
        this.createObjectExtensionFile('EnumExtension', this._enumExtWizardData.objectId, this._enumExtWizardData.objectName,
            this._enumExtWizardData.baseEnum, source);

        return true;
    }

} 