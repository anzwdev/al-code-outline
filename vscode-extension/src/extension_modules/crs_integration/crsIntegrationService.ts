import * as vscode from 'vscode';
import * as path from 'path';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { DevToolsExtensionService } from "../devToolsExtensionService";
import { ICRSExtensionPublicApi } from "./crsPublicInterfaces";
import { CrsIntegrationConst } from './crsIntegrationConst';
import { CrsSettings } from './crsSettings';
import { CrsOnSaveAlFileAction } from './crsOnSaveAlFileAction';
import { LSObjectKind } from '../../langserver/common_types/lsObjectKind';
import { CrsTypeConverters } from './crsTypeConverters';

export class CrsIntegrationService  extends DevToolsExtensionService{
    
    private _crsALLangExtApi : ICRSExtensionPublicApi | undefined;

    constructor(context: DevToolsExtensionContext) {
        super(context);

        this._crsALLangExtApi = undefined;
    }

    public async GetCrsAlLangExt() {
        if (!this._crsALLangExtApi) {
            let crsExtension = vscode.extensions.getExtension(CrsIntegrationConst.crsExtensionId);
            if (crsExtension)
            {
                if (crsExtension.isActive) {
                    this._crsALLangExtApi = crsExtension.exports;
                } else {
                    this._crsALLangExtApi = await crsExtension.activate();
                }
            }
        }
        return this._crsALLangExtApi;
    }

    public async getObjectFileName(objectKind : LSObjectKind, objectId : number, objectName : string) : Promise<string> {
        let api = await this.GetCrsAlLangExt();
        if (api) {
            let objectIdString = objectId > 0 ? objectId.toString() : "";
            let objectType = CrsTypeConverters.objectKindToObjectTypeName(objectKind);
            return api.ObjectNamesApi.GetObjectFileName(objectType, objectIdString, objectName);
        }
        return objectName + ".al";
    }

    public async getExtObjectFileName(objectKind : LSObjectKind, objectId : number, objectName : string, extendedObjectName : string) : Promise<string> {
        let api = await this.GetCrsAlLangExt();
        if (api) {
            let objectIdString = objectId > 0 ? objectId.toString() : "";
            let objectType = CrsTypeConverters.objectKindToObjectTypeName(objectKind);
            return api.ObjectNamesApi.GetObjectExtensionFileName(objectType, objectIdString, objectName, "", extendedObjectName);
        }
        return objectName + ".al";
    }

    public getDestFilePath(targetPath: string | undefined, objectKind: LSObjectKind) : string | undefined {
        //target path has been specified - do not use crs reorganize settings
        if (targetPath) {
            return targetPath;
        }
        
        let workspacePathSelected: boolean = false;
        
        //no path - select current workspace folder
        if (!targetPath) {
            targetPath = this.context.msalLanguageServerProxyService.getCurrentALWorkspaceUri()?.fsPath;
            if (!targetPath) {
                return undefined;
            }
            workspacePathSelected = true;
        }
          
        //get crs settings
        let settings = new CrsSettings(vscode.Uri.file(targetPath));
        let saveFileAction = settings.getOnSaveAlFileAction();
        if (saveFileAction !== CrsOnSaveAlFileAction.reorganize) {
            return targetPath;
        }

        //reorganize is active - find destination path
        if (!workspacePathSelected) {
            targetPath = this.context.msalLanguageServerProxyService.getCurrentALWorkspaceUri()?.fsPath;
        }
        if (!targetPath) {
            return undefined;
        }

        let alPath = settings.getAlSubFolderName();
        if (alPath) {
            targetPath = path.join(targetPath, alPath);
        }
        let objectType = CrsTypeConverters.objectKindToObjectTypeName(objectKind);
        targetPath = path.join(targetPath, objectType);

        return targetPath;
    }

}