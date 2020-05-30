'use strict';

import * as path from 'path';
import { ProjectItemWizardPage } from './projectItemWizardPage';
import { ALEnumWizardData } from './alEnumWizardData';
import { ALEnumSyntaxBuilder } from '../syntaxbuilders/alEnumSyntaxBuilder';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { ALObjectWizardSettings } from './alObjectWizardSettings';

export class ALEnumWizardPage extends ProjectItemWizardPage {
    private _enumWizardData : ALEnumWizardData;
    
    constructor(toolsExtensionContext : DevToolsExtensionContext, settings: ALObjectWizardSettings, data : ALEnumWizardData) {
        super(toolsExtensionContext, "AL Enum Wizard", settings);
        this._enumWizardData = data;
    }

    //initialize wizard
    protected onDocumentLoaded() {
        //send data to the web view
        this.sendMessage({
            command : 'setData',
            data : this._enumWizardData
        });
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'alenumwizard', 'alenumwizard.html');
    }

    protected getViewType() : string {
        return "azALDevTools.ALEnumWizard";
    }

    protected async finishWizard(data : any) : Promise<boolean> {
        //build parameters
        this._enumWizardData.objectId = data.objectId;
        this._enumWizardData.objectName = data.objectName;
        this._enumWizardData.valueList = data.valueList;
        this._enumWizardData.captionList = data.captionList;
        this._enumWizardData.extensible = data.extensible;
    
        //build new object
        var builder : ALEnumSyntaxBuilder = new ALEnumSyntaxBuilder();
        var source = builder.buildFromEnumWizardData(this._settings.getDestDirectoryUri(), this._enumWizardData);
        this.createObjectFile('Enum', this._enumWizardData.objectId, this._enumWizardData.objectName, source);

        return true;
    }

} 