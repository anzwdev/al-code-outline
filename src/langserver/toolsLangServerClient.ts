'use strict';

import * as vscode from 'vscode';
import * as cp from 'child_process';
import * as rpc from 'vscode-jsonrpc';
import { ToolsDocumentSymbolsRequest } from './toolsDocumentSymbolsRequest';
import { ToolsDocumentSymbolsResponse } from './toolsDocumentSymbolsResponse';
import { ToolsPackageSymbolsRequest } from './toolsPackageSymbolsRequest';
import { ToolsPackageSymbolsResponse } from './toolsPackageSymbolsResponse';

export class ToolsLangServerClient implements vscode.Disposable {
    _context : vscode.ExtensionContext;
    _childProcess : cp.ChildProcess | undefined;
    _connection : rpc.MessageConnection | undefined;
    _alExtensionPath : string;

    constructor(context : vscode.ExtensionContext, alExtensionPath : string) {
        this._context = context;
        this._childProcess = undefined;
        this._connection = undefined;
        this._alExtensionPath = alExtensionPath;
        this.initialize();
    }

    dispose() {
        if (this._connection) {
            this._connection.dispose();
            this._connection = undefined;
        }
    }

    protected initialize() {
        //find binaries path
        let langServerPath : string = this._context.asAbsolutePath("bin/AZALDevToolsServer.exe");
        //start child process
        this._childProcess = cp.spawn(langServerPath, [this._alExtensionPath]);
        this._connection = rpc.createMessageConnection(
            new rpc.StreamMessageReader(this._childProcess.stdout),
            new rpc.StreamMessageWriter(this._childProcess.stdin));
        this._connection.listen();
    }

    public async GetALDocumentSymbols(params : ToolsDocumentSymbolsRequest) : Promise<ToolsDocumentSymbolsResponse|undefined> {
        if (!this._connection)
            return undefined;
                
        let reqType = new rpc.RequestType<ToolsDocumentSymbolsRequest, ToolsDocumentSymbolsResponse, void, void>('al/documentsymbols');
        let val = await this._connection.sendRequest(reqType, params);
        return val;
    }

    public async GetAppPackageSymbols(params : ToolsPackageSymbolsRequest) : Promise<ToolsPackageSymbolsResponse|undefined> {
        if (!this._connection)
            return undefined;
                
        let reqType = new rpc.RequestType<ToolsPackageSymbolsRequest, ToolsPackageSymbolsResponse, void, void>('al/packagesymbols');
        let val = await this._connection.sendRequest(reqType, params);
        return val;
    }


}