'use strict';

import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { ToolsProjectSymbolsRequest } from '../langserver/toolsProjectSymbolsRequest';
import { ToolsProjectSymbolsResponse } from '../langserver/toolsProjectSymbolsResponse';
import { AZSymbolInformation } from './azSymbolInformation';
import { AZSymbolKind } from './azSymbolKind';
import { ALBaseServerSideLibrary } from './alBaseServerSideLibrary';

export class ALProjectSymbolsLibrary extends ALBaseServerSideLibrary {
    protected _projectPath : string;
    protected _projectUri : vscode.Uri;
    protected _includeDependencies : boolean;

    constructor(context : DevToolsExtensionContext, newIncludeDependencies : boolean, newProjectPath : string) {
        super(context);
        this._projectPath = newProjectPath;
        this._projectUri = vscode.Uri.file(this._projectPath);
        this._includeDependencies = newIncludeDependencies;
        this.displayName = "Project Symbols"; 
    }

    protected async loadInternalAsync(forceReload : boolean) : Promise<boolean> {
        try {
            let alPackagesPath = vscode.workspace.getConfiguration('al', null).get<string>('packageCachePath');
            if (!alPackagesPath)
                alPackagesPath = ".alpackages";

            let workspaceFoldersPaths : string[] = [];
            let folders = vscode.workspace.workspaceFolders;
            if (folders) {
                for (let i=0; i<folders.length; i++) {
                    if (folders[i].uri)
                    workspaceFoldersPaths.push(folders[i].uri.fsPath);
                }
            }

            let request : ToolsProjectSymbolsRequest = new ToolsProjectSymbolsRequest(this._includeDependencies, this._projectPath, alPackagesPath, workspaceFoldersPaths);
            let response : ToolsProjectSymbolsResponse | undefined = await this._context.toolsLangServerClient.getProjectSymbols(request);
            if ((response) && (response.root))
                this.rootSymbol = AZSymbolInformation.fromAny(response.root);
            else
                this.rootSymbol = AZSymbolInformation.create(AZSymbolKind.ProjectDefinition, this.displayName);
            if (response)
                this._libraryId = response.libraryId;
        }
        catch (e) {
            let msg : string = 'Loading project symbols failed.';
            if (e.message)
                msg = msg + ' (' + e.message + ')';
            else
                msg = msg + ' (UNDEFINED ERROR)';
            vscode.window.showErrorMessage(msg);
            return false;
        }
        return true;
    }

    public getUri(): vscode.Uri | undefined {
        return this._projectUri;
    }


}