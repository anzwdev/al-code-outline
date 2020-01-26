'use strict';

import * as vscode from 'vscode';
import * as cp from 'child_process';
import * as rpc from 'vscode-jsonrpc';
import { ToolsDocumentSymbolsRequest } from './toolsDocumentSymbolsRequest';
import { ToolsDocumentSymbolsResponse } from './toolsDocumentSymbolsResponse';
import { ToolsPackageSymbolsRequest } from './toolsPackageSymbolsRequest';
import { ToolsPackageSymbolsResponse } from './toolsPackageSymbolsResponse';
import { ToolsProjectSymbolsRequest } from './toolsProjectSymbolsRequest';
import { ToolsProjectSymbolsResponse } from './toolsProjectSymbolsResponse';
import { ToolsLibrarySymbolsDetailsRequest } from './toolsLibrarySymbolsDetailsRequest';
import { ToolsLibrarySymbolsDetailsResponse } from './toolsLibrarySymbolsDetailsResponse';
import { ToolsCloseSymbolsLibraryRequest } from './toolsCloseSymbolsLibraryRequest';
import { ToolsGetSyntaxTreeRequest } from './toolsGetSyntaxTreeRequest';
import { ToolsGetSyntaxTreeResponse } from './toolsGetSyntaxTreeResponse';
import { ToolsGetSyntaxTreeSymbolResponse } from './toolsGetSyntaxTreeSymbolResponse';
import { ToolsGetSyntaxTreeSymbolsRequest } from './toolsGetSyntaxTreeSymbolRequest';
import { ToolsCloseSyntaxTreeRequest } from './toolsCloseSyntaxTreeRequest';
import { ToolsGetCodeAnalyzersRulesRequest } from './toolsGetCodeAnalyzersRulesRequest';
import { ToolsGetCodeAnalyzersRulesResponse } from './toolsGetCodeAnalyzersRulesResponse';
import { ToolsAddAppAreasRequest } from './toolsAddAppAreasRequest';
import { ToolsAddAppAreasResponse } from './toolsAddAppAreasResponse';

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
        try {
            //find binaries path
            let langServerPath : string = this._context.asAbsolutePath("bin/AZALDevToolsServer.exe");
            //start child process
            this._childProcess = cp.spawn(langServerPath, [this._alExtensionPath]);
            if (this._childProcess) {
                this._connection = rpc.createMessageConnection(
                    new rpc.StreamMessageReader(this._childProcess.stdout),
                    new rpc.StreamMessageWriter(this._childProcess.stdin));
                this._connection.listen();
            }
        }
        catch (e) {
        }
    }

    public async getALDocumentSymbols(params : ToolsDocumentSymbolsRequest) : Promise<ToolsDocumentSymbolsResponse|undefined> {
        try {
            if (!this._connection)
                return undefined;
                    
            let reqType = new rpc.RequestType<ToolsDocumentSymbolsRequest, ToolsDocumentSymbolsResponse, void, void>('al/documentsymbols');
            let val = await this._connection.sendRequest(reqType, params);
            return val;
        }
        catch (e) {
            return undefined;
        }
    }

    public async getAppPackageSymbols(params : ToolsPackageSymbolsRequest) : Promise<ToolsPackageSymbolsResponse|undefined> {
        try {
            if (!this._connection)
                return undefined;
                    
            let reqType = new rpc.RequestType<ToolsPackageSymbolsRequest, ToolsPackageSymbolsResponse, void, void>('al/packagesymbols');
            let val = await this._connection.sendRequest(reqType, params);
            return val;
        }
        catch (e) {
            return undefined;
        }
    }

    public async getProjectSymbols(params : ToolsProjectSymbolsRequest) : Promise<ToolsProjectSymbolsResponse|undefined> {
        try {
            if (!this._connection)
                return undefined;
                
            let reqType = new rpc.RequestType<ToolsProjectSymbolsRequest, ToolsProjectSymbolsResponse, void, void>('al/projectsymbols');
            let val = await this._connection.sendRequest(reqType, params);
            return val;
        }
        catch (e) {
            return undefined;
        }
    }

    public async getLibrarySymbolsDetails(params : ToolsLibrarySymbolsDetailsRequest) : Promise<ToolsLibrarySymbolsDetailsResponse|undefined> {
        try {
            if (!this._connection)
                return undefined;
                
            let reqType = new rpc.RequestType<ToolsLibrarySymbolsDetailsRequest, ToolsLibrarySymbolsDetailsResponse, void, void>('al/librarysymbolsdetails');
            let val = await this._connection.sendRequest(reqType, params);
            return val;
        }
        catch (e) {
            return undefined;
        }
    }

    public closeSymbolsLibrary(params: ToolsCloseSymbolsLibraryRequest) {
        try {
            if (!this._connection)
                return undefined;

            let reqType = new rpc.NotificationType<ToolsCloseSymbolsLibraryRequest, void>('al/closesymbolslibrary');;
            this._connection.sendNotification(reqType, params);
        }
        catch (e) {
        }
    }

    public async getSyntaxTree(params: ToolsGetSyntaxTreeRequest) : Promise<ToolsGetSyntaxTreeResponse|undefined> {
        try {
            if (!this._connection)
                return undefined;
                
            let reqType = new rpc.RequestType<ToolsGetSyntaxTreeRequest, ToolsGetSyntaxTreeResponse, void, void>('al/getsyntaxtree');
            let val = await this._connection.sendRequest(reqType, params);
            return val;
        }
        catch(e) {
            return undefined;
        }
    }

    public async getSyntaxTreeSymbol(params: ToolsGetSyntaxTreeSymbolsRequest) : Promise<ToolsGetSyntaxTreeSymbolResponse|undefined> {
        try {
            if (!this._connection)
                return undefined;
                
            let reqType = new rpc.RequestType<ToolsGetSyntaxTreeSymbolsRequest, ToolsGetSyntaxTreeSymbolResponse, void, void>('al/getsyntaxtreesymbol');
            let val = await this._connection.sendRequest(reqType, params);
            return val;
        }
        catch(e) {
            return undefined;
        }
    }

    public closeSyntaxTree(params: ToolsCloseSyntaxTreeRequest) {
        try {
            if (!this._connection)
                return undefined;

            let reqType = new rpc.NotificationType<ToolsCloseSyntaxTreeRequest, void>('al/closesyntaxtree');;
            this._connection.sendNotification(reqType, params);
        }
        catch (e) {
        }
    }

    public async getCodeAnalyzersRules(params: ToolsGetCodeAnalyzersRulesRequest) : Promise<ToolsGetCodeAnalyzersRulesResponse|undefined> {
        try {
            if (!this._connection)
                return undefined;
                
            let reqType = new rpc.RequestType<ToolsGetCodeAnalyzersRulesRequest, ToolsGetCodeAnalyzersRulesResponse, void, void>('al/getcodeanalyzersrules');
            let val = await this._connection.sendRequest(reqType, params);
            return val;
        }
        catch(e) {
            return undefined;
        }
    }

    public async addAppAreas(params: ToolsAddAppAreasRequest) : Promise<ToolsAddAppAreasResponse> {
        try {
            if (!this._connection)
                return undefined;
            
            let reqType = new rpc.RequestType<ToolsAddAppAreasRequest, ToolsAddAppAreasResponse, void, void>('al/addappareas');
            let val = await this._connection.sendRequest(reqType, params);
            return val;
        }
        catch(e) {
            return undefined;
        }
    }

    public isEnabled() : boolean {
        if (this._connection)
            return true;        
        return false;
    }

}