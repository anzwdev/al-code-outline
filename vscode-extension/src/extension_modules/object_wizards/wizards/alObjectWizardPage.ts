import * as vscode from 'vscode';
import * as path from 'path';
import { ALObjectWizardSettings } from './alObjectWizardSettings';
import { ALObjectWizardData } from './alObjectWizardData';
import { BaseWebViewEditor } from '../../../ui_core/baseWebViewEditor';
import { DevToolsExtensionContext } from '../../../devToolsExtensionContext';
import { FileBuilder } from '../fileBuilder';
import { LSObjectKind } from '../../../langserver/common_types/lsObjectKind';
import { LSPIObjectIdentifier } from '../../../langserver/project_information/symbols/lspiObjectIdentifier';
import { LSPIGetNamespaceAndUsingsResponse } from '../../../langserver/project_information/lspiGetNamespaceAndUsingsReponse';

export class ALObjectWizardPage extends BaseWebViewEditor {
    protected _settings: ALObjectWizardSettings;
    private _objectWizardData: ALObjectWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, title : string, viewType: string, settings: ALObjectWizardSettings, data: ALObjectWizardData) {
        super(toolsExtensionContext, title, viewType);
        this._settings = settings;
        this._objectWizardData = data;
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message)) {
            return true;
        }

        switch (message.command) {
            case 'idProviderChanged':
                this.onIdProviderChanged(message.data);
                return true;
            case 'finishClick':
                this.onFinish(message.data);
                return true;
            case 'cancelClick':
                this.onCancel();
                return true;
        }
        
        return false;
    }

    protected async finishWizard(data : any) : Promise<boolean> {
        return false;
    }

    protected async onIdProviderChanged(data: any) {
        if ((data) && (data.idResProviderName)) {
            this._objectWizardData.idResProviderName = data.idResProviderName;
            let objectId = await this.context.idReservationService.suggestObjectId(
                this._objectWizardData.idResProviderName, this._settings.destDirectoryUri, this._objectWizardData.idResObjectType);
            if (objectId) {
                this._objectWizardData.objectId = objectId;
                this.sendMessage({
                    command : 'setIdProvider',
                    data : {
                        idResProviderName: this._objectWizardData.idResProviderName,
                        objectId: this._objectWizardData.objectId
                    }
                });
            }
        }
    }

    protected async finishObjectIdReservation(data: ALObjectWizardData) {
        let objectId = await this.context.idReservationService.reserveObjectId(data.idResProviderName, 
            this._settings.destDirectoryUri, data.idResObjectType, data.objectId);
        if (objectId) {
            data.objectId = objectId;
        }
    }

    protected async onFinish(data : any) {
        if (await this.finishWizard(data)) {
            this.close();
        }
    }

    protected onCancel() {
        this.close();
    }

    protected async createObjectFile(objectKind : LSObjectKind, objectId : number, objectName : string, content: string) {
        let fileName : string = await this.context.crsIntegrationService.getObjectFileName(objectKind, objectId, objectName);
        let destPath = this.context.crsIntegrationService.getDestFilePath(this._settings.destDirectoryUri.fsPath, objectKind);
        if (destPath) {
            let fullPath : string | undefined = FileBuilder.generateObjectFileInDir(destPath, fileName, content);
            if (fullPath) {
                FileBuilder.showFile(fullPath);
            }
        }
    }

    protected async createObjectExtensionFile(objectKind : LSObjectKind, objectId : number, objectName : string, extendedObjectName : string, content : string) {
        let fileName : string = await this.context.crsIntegrationService.getExtObjectFileName(objectKind, objectId, objectName, extendedObjectName);
        let destPath = this.context.crsIntegrationService.getDestFilePath(this._settings.destDirectoryUri.fsPath, objectKind);
        if (destPath) {
            let fullPath : string | undefined = FileBuilder.generateObjectFileInDir(destPath, fileName, content);
            if (fullPath) {
                FileBuilder.showFile(fullPath);
            }
        }
    }

    protected async getNamespaceAndUsings(objectKind: LSObjectKind, objectName: string, objectNamespace: string | undefined, referencedObjects: LSPIObjectIdentifier[] | undefined) : Promise<LSPIGetNamespaceAndUsingsResponse | undefined> {
        //get namespaces information
        let destFilePath = this.context.crsIntegrationService.getDestFilePath(this._settings.destDirectoryUri.fsPath, objectKind);
        if (!destFilePath) {
            return undefined;
        }
        
        let destFileUri = vscode.Uri.file(path.join(destFilePath, "newFile.al")); //this file won't be saved, it is just temporary name
        let objectIdentifier: LSPIObjectIdentifier = {
                kind: objectKind,
                id: 0,
                name: objectName,
                namespace: objectNamespace
            };

        return await this.context.projectInformationService.getNamespaceAndUsings(destFileUri, objectIdentifier, referencedObjects, false);
    }

}