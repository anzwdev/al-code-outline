'use strict';

import * as vscode from 'vscode';
import { BaseWebViewEditor } from '../webviews/baseWebViewEditor';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { AZSymbolsLibrary } from '../symbollibraries/azSymbolsLibrary';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';
import { ALSymbolsBasedPageWizard } from '../objectwizards/symbolwizards/alSymbolsBasedPageWizard';
import { ALSymbolsBasedQueryWizard } from '../objectwizards/symbolwizards/alSymbolsBasedQueryWizard';
import { ALSymbolsBasedReportWizard } from '../objectwizards/symbolwizards/alSymbolsBasedReportWizard';
import { ALSymbolsBasedXmlPortWizard } from '../objectwizards/symbolwizards/alSymbolsBasedXmlPortWizard';
import { ALSymbolsBasedPageExtWizard } from '../objectwizards/symbolwizards/alSymbolsBasedPageExtWizard';
import { ALSymbolsBasedTableExtWizard } from '../objectwizards/symbolwizards/alSymbolsBasedTableExtWizard';
import { ALSyntaxHelper } from '../allanguage/alSyntaxHelper';
import { SymbolsTreeView } from '../symbolstreeview/symbolsTreeView';

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
            case 'shownewtab':
                this.showNewTab(message.path);
                break;
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

        let objList = await this._library.getSymbolsListByPathAsync(selPaths, kind);
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
            let builder : ALSymbolsBasedPageWizard = new ALSymbolsBasedPageWizard();
            await builder.showWizard(symbolList, pageType);
        }
    }

    protected async createQuery(path : number[] | undefined, selPaths: number[][] | undefined) {
        let symbolList = await this.getObjectsFromPath(selPaths, AZSymbolKind.TableObject);
        if (symbolList) {
            let builder : ALSymbolsBasedQueryWizard = new ALSymbolsBasedQueryWizard();
            builder.showWizard(symbolList);
        }
    }

    protected async createReport(path : number[] | undefined, selPaths: number[][] | undefined) {
        let symbolList = await this.getObjectsFromPath(selPaths, AZSymbolKind.TableObject);
        if (symbolList) {
            let builder : ALSymbolsBasedReportWizard = new ALSymbolsBasedReportWizard();
            builder.showWizard(symbolList);
        }
    }

    protected async createXmlPort(path : number[] | undefined, selPaths: number[][] | undefined) {
        let symbolList = await this.getObjectsFromPath(selPaths, AZSymbolKind.TableObject);
        if (symbolList) {
            let builder : ALSymbolsBasedXmlPortWizard = new ALSymbolsBasedXmlPortWizard();
            builder.showWizard(symbolList);
        }
    }

    protected async createPageExt(path : number[] | undefined, selPaths: number[][] | undefined) {
        let symbolList = await this.getObjectsFromPath(selPaths, AZSymbolKind.PageObject);
        if (symbolList) {
            let builder : ALSymbolsBasedPageExtWizard = new ALSymbolsBasedPageExtWizard();
            builder.showWizard(symbolList);
        }
    }

    protected async createTableExt(path : number[] | undefined, selPaths: number[][] | undefined) {
        let symbolList = await this.getObjectsFromPath(selPaths, AZSymbolKind.TableObject);
        if (symbolList) {
            let builder : ALSymbolsBasedTableExtWizard = new ALSymbolsBasedTableExtWizard();
            builder.showWizard(symbolList);
        }
    }

    protected async showNewTab(path: number[] | undefined) {
        let alSymbolList : AZSymbolInformation[] | undefined = await this._library.getSymbolsListByPathAsync([path], AZSymbolKind.AnyALObject);
        if ((alSymbolList) && (alSymbolList.length > 0)) {
            let symbolsTreeView = new SymbolsTreeView(this._devToolsContext,
                alSymbolList[0], alSymbolList[0].fullName, undefined);
            symbolsTreeView.show();
        }
    }

    protected async goToDefinition(path : number[] | undefined) {
        let alSymbolList : AZSymbolInformation[] | undefined = await this._library.getSymbolsListByPathAsync([path], AZSymbolKind.AnyALObject);
        if ((alSymbolList) && (alSymbolList.length > 0)) {
            let targetLocation : vscode.Location | undefined = undefined;
            let alSymbol : AZSymbolInformation = alSymbolList[0];
            //get data type name
            let typeName : string | undefined = ALSyntaxHelper.kindToVariableType(alSymbol.kind);
            if (!typeName) {
                let typeName = ALSyntaxHelper.kindToWorkspaceSymbolType(alSymbol.kind);
                if (!typeName) {
                    vscode.window.showErrorMessage('This object type is not supported.');
                    return;    
                }               
                targetLocation = await this._devToolsContext.alLangProxy.getWorkspaceSymbol(typeName, alSymbol.name);
            } else
                targetLocation = await this._devToolsContext.alLangProxy.getDefinitionLocation(typeName, alSymbol.name);
    
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
            } else {
                vscode.window.showErrorMessage('Object definition is not available.');
            }
        }
    }

    protected async runInWebClient(path : number[] | undefined) {
        let alSymbolList : AZSymbolInformation[] | undefined = await this._library.getSymbolsListByPathAsync([path], AZSymbolKind.AnyALObject);
        if ((alSymbolList) && (alSymbolList.length > 0)) {
            this._devToolsContext.objectRunner.runSymbolAsync(alSymbolList[0]);
        }
    }

    protected onPanelClosed() {
        this._library.unloadAsync();
    }

}