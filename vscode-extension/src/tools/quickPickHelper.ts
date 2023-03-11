import * as vscode from 'vscode';
import { WorkspaceFolderQuickPickItem } from './workspaceFolderQuickPickItem';

export class QuickPickHelper {

    static async pickWorkspaceFolder(selectAllIfOneFolder: boolean) : Promise<WorkspaceFolderQuickPickItem | undefined> {
        let folders = vscode.workspace.workspaceFolders;
        
        if ((folders) && (folders.length > 0)) {
            if (folders.length == 1) {
                if (selectAllIfOneFolder)
                    return new WorkspaceFolderQuickPickItem(undefined);
                else
                    return new WorkspaceFolderQuickPickItem(folders[0]);
            } else {
                let items: WorkspaceFolderQuickPickItem[] = [];
                items.push(new WorkspaceFolderQuickPickItem(undefined));
                for (let i=0; i<folders.length;i++)
                    items.push(new WorkspaceFolderQuickPickItem(folders[i]));

                let selectedFolder = await vscode.window.showQuickPick(items, {
                    placeHolder: "Select workspace folder"
                });
                return selectedFolder;
            }
        }

        return undefined;
    }



}