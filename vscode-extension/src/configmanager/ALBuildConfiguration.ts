import * as vscode from 'vscode';
import * as path from 'path';

export class ALBuildConfiguration {
    name: string;
    fileName: string;
    projectFolderUri: vscode.Uri;
    appJsonUri: vscode.Uri;
    uri: vscode.Uri;

    constructor (projectFolderUri: vscode.Uri, uri: vscode.Uri, name: string) {
        this.projectFolderUri = projectFolderUri;
        this.appJsonUri = vscode.Uri.file(path.join(projectFolderUri.fsPath, 'app.json'));
        this.uri = uri;
        this.name = name;
        this.fileName = path.parse(uri.fsPath).base;
    }

    async getContentAsync() : Promise<string | undefined> {
        let content = await vscode.workspace.fs.readFile(this.uri);
        if (content) {
            return content.toString();
        }
        return undefined;
    }

    async initFromAppJsonAsync() {
        let content = await vscode.workspace.fs.readFile(this.appJsonUri);
        if ((content === undefined) || (content.length === 0)) {
            await this.copyFromAppJsonAsync();
        }
    }

    copyFromAppJsonAsync() : Thenable<void> {
        return vscode.workspace.fs.copy(this.appJsonUri, this.uri, { overwrite: true });
    }

    copyToAppJsonAsync() : Thenable<void> {
        return vscode.workspace.fs.copy(this.uri, this.appJsonUri, { overwrite: true });
    }

    deleteAsync() : Thenable<void> {
        return vscode.workspace.fs.delete(this.uri);
    }

}