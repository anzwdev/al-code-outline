import * as vscode from 'vscode';
import { LSPIProjectProfile } from '../../../langserver/project_information/profile/lspiProjectProfile';

export class ALObjectWizardSettings {
    destDirectoryUri: vscode.Uri;
    destFileName?: string;
    projectProfile?: LSPIProjectProfile;

    constructor(uri: vscode.Uri) {
        this.destDirectoryUri = uri;
        this.destFileName = undefined;
        this.projectProfile = undefined;
    }

}