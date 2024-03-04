import * as vscode from 'vscode';
import * as path from 'path';
import { BaseWebViewEditor } from '../../webviews/baseWebViewEditor';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { ALObjectWizardSettings } from './alObjectWizardSettings';
import { CRSALLangExtHelper } from '../../crsAlLangExtHelper';
import { FileBuilder } from '../fileBuilder';
import { ToolsGetProjectSettingsRequest } from '../../langserver/toolsGetProjectSettingsRequest';
import { ToolsGetProjectSettingsResponse } from '../../langserver/toolsGetProjectSettingsResponse';
import { ALObjectWizardData } from './alObjectWizardData';
import { ICRSExtensionPublicApi } from '../../CRSExtensionPublicApiInterfaces';
import { ToolsGetNewFileRequiredInterfacesResponse } from '../../langserver/toolsGetNewFileRequiredInterfacesResponse';
import { ToolsGetNewFileRequiredInterfacesRequest } from '../../langserver/toolsGetNewFileRequiredInterfacesRequest';
import { ToolsSymbolReference } from '../../langserver/symbolsinformation/toolsSymbolReference';

export class ProjectItemWizardPage extends BaseWebViewEditor {
    protected _toolsExtensionContext : DevToolsExtensionContext;
    protected _settings: ALObjectWizardSettings;
    private _objectWizardData: ALObjectWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, title : string, settings: ALObjectWizardSettings, data: ALObjectWizardData) {
        super(toolsExtensionContext.vscodeExtensionContext, title);
        this._toolsExtensionContext = toolsExtensionContext;
        this._settings = settings;
        this._objectWizardData = data;
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message))
            return true;

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
            let objectId = await this._toolsExtensionContext.idReservationService.suggestObjectId(
                this._objectWizardData.idResProviderName, this._objectWizardData.uri, this._objectWizardData.idResObjectType);
            if (objectId) {
                this._objectWizardData.objectId = objectId.toString();
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
        let objectId: number | undefined = Number.parseInt(data.objectId);
        if (!Number.isNaN(objectId)) {
            objectId = await this._toolsExtensionContext.idReservationService.reserveObjectId(
                data.idResProviderName, data.uri, data.idResObjectType, objectId);
            if (objectId)
                data.objectId = objectId.toString();
        }
    }

    protected async onFinish(data : any) {
        if (await this.finishWizard(data))
            this.close();
    }

    protected onCancel() {
        this.close();
    }

    protected async getObjectFileName(objectType : string, objectId : string, objectName : string) : Promise<string> {
        let crsExtensionApi : ICRSExtensionPublicApi | undefined = await CRSALLangExtHelper.GetCrsAlLangExt();
        if (crsExtensionApi)
            return crsExtensionApi.ObjectNamesApi.GetObjectFileName(objectType, objectId, objectName);
        else
            return objectName + ".al";
    }

    protected async getExtObjectFileName(objectType : string, objectId : string, objectName : string, extendedObjectName : string) : Promise<string> {
        let crsExtensionApi : ICRSExtensionPublicApi | undefined = await CRSALLangExtHelper.GetCrsAlLangExt();
        if (crsExtensionApi)
            return crsExtensionApi.ObjectNamesApi.GetObjectExtensionFileName(
                objectType, objectId, objectName, "", extendedObjectName);
        else
            return objectName + ".al";
    }

    protected async createObjectFile(objectType : string, objectId : string, objectName : string, content: string) {
        let fileName : string = await this.getObjectFileName(objectType, objectId, objectName);
        let destPath = this.getDestFilePath(this._settings.destDirectoryPath, objectType);
        if (destPath) {
            let fullPath : string | undefined = FileBuilder.generateObjectFileInDir(destPath, fileName, content);
            if (fullPath)
                FileBuilder.showFile(fullPath);
        }
    }

    protected async createObjectExtensionFile(objectType : string, objectId : string, objectName : string, extendedObjectName : string, content : string) {
        let fileName : string = await this.getExtObjectFileName(objectType, objectId, objectName, extendedObjectName);
        let destPath = this.getDestFilePath(this._settings.destDirectoryPath, objectType);
        if (destPath) {
            let fullPath : string | undefined = FileBuilder.generateObjectFileInDir(destPath, fileName, content);
            if (fullPath) {
                FileBuilder.showFile(fullPath);
            }
        }
    }

    protected async getNamespacesInformation(objectType: string, referencedObjects: ToolsSymbolReference[] | undefined) : Promise<ToolsGetNewFileRequiredInterfacesResponse | undefined> {
        //get namespaces information
        let destFilePath = this.getDestFilePath(this._settings.destDirectoryPath, objectType);
        if (!destFilePath) {
            return undefined;
        }
        destFilePath = path.join(destFilePath, "newFile.al");   //this file won't be saved, it is just temporary name

        let alSettings = vscode.workspace.getConfiguration("al", vscode.Uri.file(destFilePath));
        let rootNamespace = alSettings.get<string>("rootNamespace");       

        return await this._toolsExtensionContext.toolsLangServerClient.getNewFileRequiredInterfaces(
            new ToolsGetNewFileRequiredInterfacesRequest(true, destFilePath, rootNamespace, referencedObjects));
    }

    protected getDestFilePath(targetPath: string | undefined, objectType: string) : string | undefined {
        //target path has been specified - do not use crs reorganize settings
        if (targetPath) {
            return targetPath;
        }
        
        let workspacePathSelected: boolean = false;
        
        //no path - select current workspace folder
        if (!targetPath) {
            targetPath = this._toolsExtensionContext.alLangProxy.getCurrentWorkspaceFolderPath();
            if (!targetPath) {
                return undefined;
            }
            workspacePathSelected = true;
        }
          
        //get crs settings        
        let settings = vscode.workspace.getConfiguration('CRS', vscode.Uri.file(targetPath));
        let saveFileAction = settings.get<string>('OnSaveAlFileAction');        
        if ((!saveFileAction) || (saveFileAction.toLowerCase() != 'reorganize'))
            return targetPath;

        //reorganize is active - find destination path
        if (!workspacePathSelected)
            targetPath = this._toolsExtensionContext.alLangProxy.getCurrentWorkspaceFolderPath();
        if (!targetPath)
            return undefined;

        let alPath = settings.get<string>('AlSubFolderName');
        if (alPath)
            targetPath = path.join(targetPath, alPath);
        targetPath = path.join(targetPath, objectType.toLowerCase());

        return targetPath;
    }

    protected async getProjectSettings() : Promise<ToolsGetProjectSettingsResponse | undefined> {
        let uri = this._settings.getDestDirectoryUri();
        if (!uri) {
            let folders = vscode.workspace.workspaceFolders;
            if ((folders) && (folders.length > 0))
                uri = folders[0].uri;
        }

        //get project settings
        if (uri)
            return await this._toolsExtensionContext.toolsLangServerClient.getProjectSettings(new ToolsGetProjectSettingsRequest(uri.fsPath));

        return undefined;
    }

}