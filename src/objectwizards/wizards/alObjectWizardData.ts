import * as vscode from 'vscode';

export class ALObjectWizardData {
    uri: vscode.Uri | undefined;
    objectId : string;
    idResProviders: string[] | undefined;
    idResProviderName: string | undefined;
    idResObjectType: string;    

    constructor() {
        this.objectId = '';
        this.idResObjectType = '';
    }

}