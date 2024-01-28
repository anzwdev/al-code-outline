import * as vscode from 'vscode';

export class ALObjectWizardData {
    uri: vscode.Uri | undefined;
    objectId : string;
    idResProviders: string[] | undefined;
    idResProviderName: string | undefined;
    idResObjectType: string;
    objectNamespace: string | undefined;
    objectUsings: string[] | undefined;

    constructor() {
        this.objectId = '';
        this.idResObjectType = '';
    }

}