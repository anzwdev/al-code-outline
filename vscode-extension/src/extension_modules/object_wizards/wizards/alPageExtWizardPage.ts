import * as path from 'path';
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { ALPageExtWizardData } from './alPageExtWizardData';
import { ALObjectWizardPage } from './alObjectWizardPage';
import { DevToolsExtensionContext } from '../../../devToolsExtensionContext';
import { LSPIObjectIdentifier } from '../../../langserver/project_information/symbols/lspiObjectIdentifier';
import { LSObjectKind } from '../../../langserver/common_types/lsObjectKind';
import { ALPageExtSyntaxBuilder } from '../syntax_builders/alPageExtSyntaxBuilder';
import { LSPIObjectListItem } from '../../../langserver/project_information/symbols/lspiObjectListItem';
import { LSPIObjectListItemHelper } from '../../../langserver/project_information/symbols/lspiObjectListItemHelper';

export class ALPageExtWizardPage extends ALObjectWizardPage {
    protected _pageExtWizardData : ALPageExtWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, settings: ALObjectWizardSettings, data : ALPageExtWizardData) {
        super(toolsExtensionContext, "AL Page Extension Wizard", "azALDevTools.ALPageExtWizard", settings, data);
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
        var basePageListItem: LSPIObjectListItem | undefined = data.basePage;
        var basePage = LSPIObjectListItemHelper.toObjectIdentifierOrUndefined(basePageListItem);
        
        //build parameters
        this._pageExtWizardData.objectId = data.objectId;
        this._pageExtWizardData.objectName = data.objectName;
        this._pageExtWizardData.basePage = basePage;
        
        await this.finishObjectIdReservation(this._pageExtWizardData);

        //get namespaces information
        let referencedObjects: LSPIObjectIdentifier[] = [];
        if (this._pageExtWizardData.basePage) {
            referencedObjects.push(this._pageExtWizardData.basePage);
        }

        let fileNamespaces = await this.getNamespaceAndUsings(LSObjectKind.PageExtension, this._pageExtWizardData.objectName, this._pageExtWizardData.objectNamespace, referencedObjects);
        if (fileNamespaces) {
            this._pageExtWizardData.objectNamespace = fileNamespaces.namespace;
            this._pageExtWizardData.objectUsings = fileNamespaces.usings;
        }

        //build new object
        let builder : ALPageExtSyntaxBuilder = new ALPageExtSyntaxBuilder();
        let source = await builder.buildFromPageExtWizardData(this._settings.destDirectoryUri,
            this._pageExtWizardData);
        this.createObjectExtensionFile(LSObjectKind.PageExtension, this._pageExtWizardData.objectId, this._pageExtWizardData.objectName, this._pageExtWizardData.basePage?.name ?? "", source);

        return true;
    }

    protected async loadPages() {
        this._pageExtWizardData.pageList = await this.context.projectInformationService.getObjectList(this._settings.destDirectoryUri, LSObjectKind.Page);

        //let resourceUri = this._settings.getDestDirectoryUri();
        //this._pageExtWizardData.pageList = await this._toolsExtensionContext.alLangProxy.getPageList(resourceUri);
        if ((this._pageExtWizardData.pageList) && (this._pageExtWizardData.pageList.length > 0)) {
            this.sendMessage({
                command : "setPages",
                data : this._pageExtWizardData.pageList
            });
        }
    }

}
