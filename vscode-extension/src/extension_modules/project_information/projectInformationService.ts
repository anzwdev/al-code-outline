import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { LSProjectInformationProviderClient } from "../../langserver/project_information/lsProjectInformationProviderClient";
import { DevToolsExtensionService } from "../devToolsExtensionService";
import { LSPIProjectProfile } from '../../langserver/project_information/profile/lspiProjectProfile';
import { LSObjectKind } from '../../langserver/common_types/lsObjectKind';
import { LSPIObjectListItem } from '../../langserver/project_information/symbols/lspiObjectListItem';
import { LSPIObjectIdentifier } from '../../langserver/project_information/symbols/lspiObjectIdentifier';
import { LSPITableFieldListItem } from '../../langserver/project_information/symbols/lspiTableFieldListItem';
import { LSTableFieldClass } from '../../langserver/common_types/lsTableFieldClass';
import { LSPIGetNamespaceAndUsingsResponse } from '../../langserver/project_information/lspiGetNamespaceAndUsingsReponse';
import { LSPIGetObjectsListFilter } from '../../langserver/project_information/lspiGetObjectsListFilter';
import { LSPIMethodListItem } from '../../langserver/project_information/symbols/lspiMethodListItem';

export class ProjectInformationService extends DevToolsExtensionService {

    private _lsClient: LSProjectInformationProviderClient;

    constructor(context: DevToolsExtensionContext) {
        super(context);
        this._lsClient = new LSProjectInformationProviderClient(context.lsConnector);
    }

    private getPath(uri: vscode.Uri | undefined): string {
        return uri?.fsPath ?? "";
    }

    public async getProjectProfile(uri: vscode.Uri | undefined): Promise<LSPIProjectProfile | undefined> {
        let response = await this._lsClient.getProjectProfile({
            path: this.getPath(uri)            
        });
        return response?.profile;
    }

    public getObjectList(uri: vscode.Uri | undefined, kind: LSObjectKind): Promise<LSPIObjectListItem[] | undefined> {
        let filter: LSPIGetObjectsListFilter = { 
            kind: kind,
            excludeFullInherentPermissions: false,
            skipDependencies: false 
        };
        return this.getFilteredObjectList(uri, filter);
    }

    public async getFilteredObjectList(uri: vscode.Uri | undefined, filter: LSPIGetObjectsListFilter): Promise<LSPIObjectListItem[] | undefined> {
        let response = await this._lsClient.getObjectsList({
            path: this.getPath(uri),
            filter: filter
        });
        return response?.objects;
    }

    public async getObjectMethods(uri: vscode.Uri | undefined, identifier: LSPIObjectIdentifier, includePrivate: boolean) : Promise<LSPIMethodListItem[] | undefined> {
        let response = await this._lsClient.getObjectMethods({
            path: this.getPath(uri),
            identifier: identifier,
            includePrivate: includePrivate
        });
        return response?.methods;
    }

    
    public async getTableFieldsList(uri: vscode.Uri | undefined, tableIdentifier: LSPIObjectIdentifier, fieldClassFilter: LSTableFieldClass[] | undefined,
        includeToolTips: boolean, toolTipsSourceDependencies: string[] | undefined): Promise<LSPITableFieldListItem[] | undefined> {

        let response = await this._lsClient.getTableFieldsList({
            path: this.getPath(uri),
            tableIdentifier: tableIdentifier,
            fieldClassFilter: fieldClassFilter,
            includeToolTips: includeToolTips,
            toolTipsSourceDependencies: toolTipsSourceDependencies});
        return response?.fields;
    }

    public async getNamespaceAndUsings(uri: vscode.Uri | undefined, objectIdentifier: LSPIObjectIdentifier, 
        referencedObjects: LSPIObjectIdentifier[] | undefined, force: boolean): Promise<LSPIGetNamespaceAndUsingsResponse | undefined> {

        let alSettings = vscode.workspace.getConfiguration("al", uri);
        let rootNamespace = alSettings.get<string>("rootNamespace");       

        let response = await this._lsClient.getNamespaceAndUsings({
            objectIdentifier: objectIdentifier,
            referencedObjectsIdentifiers: referencedObjects,
            path: uri?.fsPath ?? "",
            rootNamespace: rootNamespace,
            force: force
        });

        return response;
    }


    public async getNextObjectId(uri: vscode.Uri | undefined, kind: LSObjectKind): Promise<number | undefined> {
        let response = await this._lsClient.getNextObjectId({
            path: uri?.fsPath ?? "",
            kind: kind
        });

        return response?.id;
    }

}
