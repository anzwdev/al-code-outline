'use strict';

import * as path from 'path';
import * as vscode from 'vscode';
import { ToolsPackageSymbolsRequest } from '../langserver/toolsPackageSymbolsRequest';
import { ToolsPackageSymbolsResponse } from '../langserver/toolsPackageSymbolsResponse';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { AZSymbolsLibrary } from './azSymbolsLibrary';
import { AZSymbolInformation } from './azSymbolInformation';
import { AZSymbolKind } from './azSymbolKind';

export class ALAppSymbolsLibrary extends AZSymbolsLibrary {
    filePath : string;
    protected _context : DevToolsExtensionContext;

    constructor(context : DevToolsExtensionContext, sourceFilePath : string) {
        super();
        this._context = context;
        this.filePath = sourceFilePath;
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

} 