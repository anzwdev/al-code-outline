'use strict';

import * as vscode from 'vscode';
import { BaseWebViewEditor } from '../webviews/baseWebViewEditor';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { AZSymbolsLibrary } from '../symbollibraries/azSymbolsLibrary';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';
import { PageBuilder } from '../objectbuilders/pageBuilder';
import { QueryBuilder } from '../objectbuilders/queryBuilder';
import { ReportBuilder } from '../objectbuilders/reportBuilder';
import { XmlPortBuilder } from '../objectbuilders/xmlPortBuilder';
import { PageExtBuilder } from '../objectbuilders/pageExtBuilder';
import { TableExtBuilder } from '../objectbuilders/tableExtBuilder';
import { ALSyntaxHelper } from '../allanguage/alSyntaxHelper';
import { ALObjectRunner } from '../alObjectRunner';

/** Base class for AL Object and AL Symbol browsers */
export class ALBaseSymbolsBrowser extends BaseWebViewEditor {
    protected _library : AZSymbolsLibrary;
    protected _devToolsContext : DevToolsExtensionContext;

    constructor(devToolsContext : DevToolsExtensionContext,  library : AZSymbolsLibrary) {
        let name : string = library.displayName;
        if (name == '')
            name =  'AL Object Browser';
        super(devToolsContext.vscodeExtensionContext, library.displayName);
        this._devToolsContext = devToolsContext;
        this._library = library;
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message))
            return true;

        switch (message.command) {
            case 'definition':
                this.goToDefinition(message.path);
                return true;
            case 'runinwebclient':
                this.runInWebClient(message.path);
                return true;
            case 'newcardpage':
                this.createPage(message.path, message.selpaths, "Card");
                return true;
            case 'newlistpage':
                this.createPage(message.path, message.selpaths, "List");
                return true;
            case 'newreport':
                this.createReport(message.path, message.selpaths);
                return true;
            case 'newxmlport':
                this.createXmlPort(message.path, message.selpaths);
                return true;
            case 'newquery':
                this.createQuery(message.path, message.selpaths);
                return true;
            case 'extendtable':
                this.createTableExt(message.path, message.selpaths);
                return true;
            case 'extendpage':
                this.createPageExt(message.path, message.selpaths);
                return true;
        }

        return false;
    }

    protected async getObjectsFromPath(selPaths: number[][] | undefined, kind : AZSymbolKind) : Promise<AZSymbolInformation[] | undefined> {
        if (!selPaths)
            return undefined;

        let objList = await this._library.getSymbolListByPath(selPaths, kind);
        if (!objList)
            return undefined;

        if (objList.length > 100) {
            let action: string = await vscode.window.showWarningMessage(`You are about to run this command for ${selPaths.length} objects. Do you want to continue?`, {modal: true}, 'Yes', 'No');
            if (action !== 'Yes') {
                return undefined;
            }
        }

        return objList;
    }

    protected async createPage(path : number[] | undefined, selPaths: number[][] | undefined, pageType : string) {
        let symbolList = await this.getObjectsFromPath(selPaths, AZSymbolKind.TableObject);
        if (symbolList) {
            let builder : PageBuilder = new PageBuilder();
            await builder.showWizard(symbolList, pageType);
        }
    }

    protected async createQuery(path : number[] | undefined, selPaths: number[][] | undefined) {
        let symbolList = await this.getObjectsFromPath(selPaths, AZSymbolKind.TableObject);
        if (symbolList) {
            let builder : QueryBuilder = new QueryBuilder();
            builder.showWizard(symbolList);
        }
    }

    protected async createReport(path : number[] | undefined, selPaths: number[][] | undefined) {
        let symbolList = await this.getObjectsFromPath(selPaths, AZSymbolKind.TableObject);
        if (symbolList) {
            let builder : ReportBuilder = new ReportBuilder();
            builder.showWizard(symbolList);
        }
    }

    protected async createXmlPort(path : number[] | undefined, selPaths: number[][] | undefined) {
        let symbolList = await this.getObjectsFromPath(selPaths, AZSymbolKind.TableObject);
        if (symbolList) {
            let builder : XmlPortBuilder = new XmlPortBuilder();
            builder.showWizard(symbolList);
        }
    }

    protected async createPageExt(path : number[] | undefined, selPaths: number[][] | undefined) {
        let symbolList = await this.getObjectsFromPath(selPaths, AZSymbolKind.TableObject);
        if (symbolList) {
            let builder : PageExtBuilder = new PageExtBuilder();
            builder.showWizard(symbolList);
        }
    }

    protected async createTableExt(path : number[] | undefined, selPaths: number[][] | undefined) {
        let symbolList = await this.getObjectsFromPath(selPaths, AZSymbolKind.TableObject);
        if (symbolList) {
            let builder : TableExtBuilder = new TableExtBuilder();
            builder.showWizard(symbolList);
        }
    }

    protected async goToDefinition(path : number[] | undefined) {
        let alSymbol : AZSymbolInformation | undefined = this._library.getObjectSymbolByPath(path);
        if (alSymbol) {
            //get data type name
            let typeName : string | undefined = ALSyntaxHelper.kindToVariableType(alSymbol.kind);
            if (!typeName) {
                vscode.window.showErrorMessage('This object type is not supported.');
                return;
            }

            let targetLocation : vscode.Location | undefined = await this._devToolsContext.alLangProxy.getDefinitionLocation(typeName, alSymbol.name);
    
            if (targetLocation) {
                try {
                    //open document
                    let targetDoc = await vscode.workspace.openTextDocument(targetLocation.uri);
                    let targetEditor = await vscode.window.showTextDocument(targetDoc, {
                        preview : true
                    });

                    //go to location
                    targetEditor.selection = new vscode.Selection(targetLocation.range.start, targetLocation.range.start);
                }
                catch (e) {
                    vscode.window.showErrorMessage(e.message);
                }
            }
        }
    }

    protected runInWebClient(path : number[] | undefined) {
        let alSymbol : AZSymbolInformation | undefined = this._library.getObjectSymbolByPath(path);
        if (alSymbol) {
            ALObjectRunner.runInWebClient(alSymbol);
        }
    }


}