import * as vscode from 'vscode';
import * as fs from 'fs';
import * as path from 'path';

export class UriHelper {

    public static getDirectoryUriIfFile(uri: vscode.Uri): vscode.Uri {

        let fullPath: string = uri.fsPath;
        if (fs.lstatSync(fullPath).isDirectory()) {
            return uri;
        }

        let parsedPath = path.parse(fullPath);
        return vscode.Uri.file(parsedPath.dir);
    }

}
