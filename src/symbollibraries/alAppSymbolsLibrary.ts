'use strict';

import * as path from 'path';
import * as vscode from 'vscode';
import { ToolsPackageSymbolsRequest } from '../langserver/toolsPackageSymbolsRequest';
import { ToolsPackageSymbolsResponse } from '../langserver/toolsPackageSymbolsResponse';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { AZSymbolInformation } from './azSymbolInformation';
import { AZSymbolKind } from './azSymbolKind';
import { ALBaseServerSideLibrary } from './alBaseServerSideLibrary';
import { NumberHelper } from '../tools/numberHelper';

export class ALAppSymbolsLibrary extends ALBaseServerSideLibrary {
    filePath : string;
    protected _fileUri : vscode.Uri;

    constructor(context : DevToolsExtensionContext, sourceFilePath : string) {
        super(context);
        this.filePath = sourceFilePath;
        this._fileUri = vscode.Uri.file(this.filePath);
        this.displayName = path.parse(sourceFilePath).base; 
    }
  
    protected async loadInternalAsync(forceReload : boolean) : Promise<boolean> {
        try {
            let request : ToolsPackageSymbolsRequest = new ToolsPackageSymbolsRequest(this.filePath);
            let response : ToolsPackageSymbolsResponse | undefined = await this._context.toolsLangServerClient.getAppPackageSymbols(request);
            if ((response) && (response.root))
                this.rootSymbol = AZSymbolInformation.fromAny(response.root);
            else
                this.rootSymbol = AZSymbolInformation.create(AZSymbolKind.Document, this.displayName);
            if (response)
                this._libraryId = NumberHelper.zeroIfNotDef(response.libraryId);
        }
        catch (e) {
            let msg : string = 'Loading symbols from file "' + this.filePath + '" failed.';
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
        return this._fileUri;
    }

} 