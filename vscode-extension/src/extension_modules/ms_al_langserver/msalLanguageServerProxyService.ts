import * as vscode from 'vscode';
import { Version } from '../../core/version';
import { DevToolsExtensionService } from '../devToolsExtensionService';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { MSALLanguageServerConst } from './msalLanguageServerConst';

export class MSALLanguageServerProxyService extends DevToolsExtensionService {
    public extensionPath : string | undefined;
    public version : Version;
    public alEditorService: any;

    constructor(context: DevToolsExtensionContext) {
        super(context);

        this.version = new Version();
        this.alEditorService = undefined;
        this.loadExtensionProperties();
    }

    protected loadExtensionProperties() {
        let alExtension = this.getALExtension();
        if (alExtension) {
            this.extensionPath = alExtension.extensionPath;
            if (alExtension.packageJSON) {
                this.version.parse(alExtension.packageJSON.version);
            }
        }
    }

    protected getALExtension() : any {
        let alStoreExtension = vscode.extensions.getExtension(MSALLanguageServerConst.msalMarketplaceExtensionId); 
        let alFileExtension = vscode.extensions.getExtension(MSALLanguageServerConst.msalFileExtensionId);

        if ((alStoreExtension) && (alFileExtension)) {
            if (alStoreExtension.isActive) {
                return alStoreExtension;
            }
            if (alFileExtension.isActive) {
                return alFileExtension;
            }
        }

        return alStoreExtension ?? alFileExtension;
    }

    protected loadLanguageClientProperties() : boolean {
        if (!this.alEditorService) {
            let alExtension = this.getALExtension();
            if ((!alExtension) || (!alExtension.isActive)) {
                return false;
            }

            if (alExtension.exports) {
                //find editor service
                if (alExtension.exports.services) {
                    let alServices = alExtension.exports.services;
                    for (let sidx = 0; (sidx < alServices.length) && (!this.alEditorService); sidx++) {
                        if (alServices[sidx].setActiveWorkspace) {
                            this.alEditorService = alServices[sidx];
                        }
                    }
                }
            }

        }
        return true;
    }

    getCurrentALWorkspaceUri() : vscode.Uri | undefined {
        if ((!vscode.workspace.workspaceFolders) || (vscode.workspace.workspaceFolders.length === 0)) {
            return undefined;
        }

        if (vscode.workspace.workspaceFolders.length > 1) {
            this.loadLanguageClientProperties();
            if (this.alEditorService) {
                if (this.alEditorService.lastActiveWorkspacePath) {
                    return vscode.Uri.file(this.alEditorService.lastActiveWorkspacePath);
                }            
            }
        }

        return vscode.workspace.workspaceFolders[0].uri;
    }

}