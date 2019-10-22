'use strict';

import * as vscode from 'vscode';
import * as path from 'path';
import { BaseWebViewEditor } from '../../webviews/baseWebViewEditor';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { ALObjectWizardSettings } from './alObjectWizardSettings';
import { ICRSExtensionPublicApi } from 'crs-al-language-extension-api';
import { CRSALLangExtHelper } from '../../crsAlLangExtHelper';
import { FileBuilder } from '../fileBuilder';

export class ProjectItemWizardPage extends BaseWebViewEditor {
    protected _toolsExtensionContext : DevToolsExtensionContext;
    protected _settings: ALObjectWizardSettings;

    constructor(toolsExtensionContext : DevToolsExtensionContext, title : string, settings: ALObjectWizardSettings) {
        super(toolsExtensionContext.vscodeExtensionContext, title);
        this._toolsExtensionContext = toolsExtensionContext;
        this._settings = settings;
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message))
            return true;

        switch (message.command) {
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
        let fullPath : string = FileBuilder.generateObjectFileInDir(destPath, fileName, content);
        if (fullPath)
            FileBuilder.showFile(fullPath);
    }

    protected async createObjectExtensionFile(objectType : string, objectId : string, objectName : string, extendedObjectName : string, content : string) {
        let fileName : string = await this.getExtObjectFileName(objectType, objectId, objectName, extendedObjectName);
        let destPath = this.getDestFilePath(this._settings.destDirectoryPath, objectType);
        let fullPath : string = FileBuilder.generateObjectFileInDir(destPath, fileName, content);
        if (fullPath)
            FileBuilder.showFile(fullPath);
    }

    protected getDestFilePath(targetPath: string | undefined, objectType: string) : string | undefined {
        //target path has been specified - do not use crs reorganize settings
        if (targetPath)
            return targetPath;
        
        let workspacePathSelected: boolean = false;
        
        //no path - select current workspace folder
        if (!targetPath) {
            targetPath = this._toolsExtensionContext.alLangProxy.getCurrentWorkspaceFolderPath();
            if (!targetPath)
                return undefined;
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
        let alPath : string = settings.get<string>('AlSubFolderName');
        if (alPath)
            targetPath = path.join(targetPath, alPath);
        targetPath = path.join(targetPath, objectType.toLowerCase());

        return targetPath;
    }

}