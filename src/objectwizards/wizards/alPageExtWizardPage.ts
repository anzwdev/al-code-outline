import * as path from 'path';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { ALPageExtWizardData } from './alPageExtWizardData';
import { ALPageExtSyntaxBuilder } from '../syntaxbuilders/alPageExtSyntaxBuilder';
import { ProjectItemWizardPage } from './projectItemWizardPage';

export class ALPageExtWizardPage extends ProjectItemWizardPage {
    protected _pageExtWizardData : ALPageExtWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, settings: ALObjectWizardSettings, data : ALPageExtWizardData) {
        super(toolsExtensionContext, "AL Page Extension Wizard", settings);
        this._pageExtWizardData = data;
    }

    //initialize wizard
    protected onDocumentLoaded() {
        //send data to the web view
        this.sendMessage({
            command : 'setData',
            data : this._pageExtWizardData
        });
        this.loadPages();
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'alpageextwizard', 'alpageextwizard.html');
    }

    protected getViewType() : string {
        return "azALDevTools.ALPageExtWizard";
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message)) {
            return true;
        }

        switch (message.command) {
            case 'loadPages':
                this.loadPages();
                return true;
        }
        
        return false;
    }

    protected async finishWizard(data : any) : Promise<boolean> {
        //build parameters
        this._pageExtWizardData.objectId = data.objectId;
        this._pageExtWizardData.objectName = data.objectName;
        this._pageExtWizardData.basePage = data.basePage;
        
        //build new object
        let builder : ALPageExtSyntaxBuilder = new ALPageExtSyntaxBuilder();
        let source = await builder.buildFromPageExtWizardData(this._settings.getDestDirectoryUri(),
            this._pageExtWizardData);
        this.createObjectExtensionFile('PageExtension', this._pageExtWizardData.objectId, this._pageExtWizardData.objectName, this._pageExtWizardData.basePage, source);

        return true;
    }

    protected async loadPages() {
        let resourceUri = this._settings.getDestDirectoryUri();
        this._pageExtWizardData.pageList = await this._toolsExtensionContext.alLangProxy.getPageList(resourceUri);
        if ((this._pageExtWizardData.pageList) && (this._pageExtWizardData.pageList.length > 0)) {
            this.sendMessage({
                command : "setPages",
                data : this._pageExtWizardData.pageList
            });
        }
    }

}
