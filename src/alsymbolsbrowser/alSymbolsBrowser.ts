import * as vscode from 'vscode';
import * as path from 'path';
import { AZSymbolsLibrary } from '../symbollibraries/azSymbolsLibrary';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';
import { ALObjectBrowserItem } from './alObjectBrowserItem';
import { BaseWebViewEditor } from '../webviews/baseWebViewEditor';
import { ALSymbolsBasedPageWizard } from '../objectwizards/symbolwizards/alSymbolsBasedPageWizard';
import { ALSymbolsBasedQueryWizard } from '../objectwizards/symbolwizards/alSymbolsBasedQueryWizard';
import { ALSymbolsBasedReportWizard } from '../objectwizards/symbolwizards/alSymbolsBasedReportWizard';
import { ALSymbolsBasedXmlPortWizard } from '../objectwizards/symbolwizards/alSymbolsBasedXmlPortWizard';
import { ALSymbolsBasedPageExtWizard } from '../objectwizards/symbolwizards/alSymbolsBasedPageExtWizard';
import { ALSymbolsBasedReportExtWizard } from '../objectwizards/symbolwizards/alSymbolsBasedReportExtWizard';
import { ALSymbolsBasedTableExtWizard } from '../objectwizards/symbolwizards/alSymbolsBasedTableExtWizard';
import { ALSyntaxHelper } from '../allanguage/alSyntaxHelper';
import { SymbolsTreeView } from '../symbolstreeview/symbolsTreeView';
import { TextEditorHelper } from '../tools/textEditorHelper';
import { StringHelper } from '../tools/stringHelper';
import { AppFileTextContentProvider } from '../editorextensions/appFileTextContentProvider';
import { ToolsGetProjectSymbolLocationRequest } from '../langserver/toolsGetProjectSymbolLocationRequest';
import { ALSymbolSourceLocation } from '../symbollibraries/alSymbolSourceLocation';

/**
 * AL Symbols Browser
 * allows to browse symbols in a tree structure like in the Class Browser in Visual Studio
 * and in a list view like in old Dynamics Nav object browser
 */
export class ALSymbolsBrowser extends BaseWebViewEditor {
    protected _library : AZSymbolsLibrary;
    protected _devToolsContext : DevToolsExtensionContext;
    protected _selectedObject : AZSymbolInformation | undefined;
    protected _showObjectIds : boolean;
    protected _treeViewMode : boolean;
    protected _itemsList: ALObjectBrowserItem[];
    protected _showLibraries: boolean;

    constructor(devToolsContext : DevToolsExtensionContext,  library : AZSymbolsLibrary) {
        super(devToolsContext.vscodeExtensionContext, library.displayName);
        this._devToolsContext = devToolsContext;
        this._library = library;
        this._treeViewMode = devToolsContext.getUseSymbolsBrowser();        
        //tree view properties
        this._selectedObject = undefined;
        this._showObjectIds = false;
        //list view properties
        this._itemsList = [];
        this._showLibraries = false;
    }

    protected getHtmlContentPath() : string {
        if (this._treeViewMode)
            return path.join('htmlresources', 'alsymbolsbrowser', 'symbolsbrowser.html');
        return path.join('htmlresources', 'objectbrowser', 'objectbrowser.html');        
    }

    protected getViewType() : string {
        return 'azALDevTools.ALSymbolsBrowser';
    }

    protected async onDocumentLoaded() {
        //load library
        await this._library.loadAsync(false);

        if (this._treeViewMode) {
            //send data to the tree web view
            this.updateTreeObjects();
        } else {
            //send data to the list web view
            this.updateListObjects();
        }
    }

    protected updateTreeObjects() {        
        this.sendMessage({
            command : 'setData',
            data : this._library.rootSymbol
        });
    }

    protected updateListObjects() {
        this._itemsList = [];
        this._showLibraries = false;
        if (this._library.rootSymbol)
            this.collectListSymbols(this._library.rootSymbol, '');

        //send data to the web view
        this.sendMessage({
            command: 'setData',
            data: this._itemsList,
            showLibraries: this._showLibraries
        });
    }

    protected collectListSymbols(symbol: AZSymbolInformation, libraryName: string) {
        if (symbol.isALObject()) {
            this._itemsList.push(new ALObjectBrowserItem(symbol.kind, symbol.id, symbol.name, libraryName, symbol.getPath()))
        } else if (symbol.childSymbols) {
            if (symbol.kind == AZSymbolKind.Package) {
                libraryName = symbol.name;
                if ((!this._showLibraries) && (this._itemsList.length > 0))
                    this._showLibraries = true;
            }

            for (let i=0; i<symbol.childSymbols.length; i++) {
                symbol.childSymbols[i].parent = symbol;
                this.collectListSymbols(symbol.childSymbols[i], libraryName);
            }
        }
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message))
            return true;

        switch (message.command) {
            case 'definition':
                this.goToDefinition(message.path);
                return true;
            case 'localdefinition':
                    this.goToLocalDefinition(message.path);
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
            case 'extendreport':
                this.createReportExt(message.path, message.selpaths);
                return true;
            case 'copysel':
                this.copySelected(message.path, message.selpaths);
                return true;
            case 'showlist':
                this.switchViewMode(false);
                break;
            case 'showTreeView':
                this.switchViewMode(true);
                break;
            case 'objselected':
                this.onObjectSelected(message.path);
                return true;
            case 'currRowChanged':
                this.updatePivotObjCommand(message.path);
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
            let action = await vscode.window.showWarningMessage(`You are about to run this command for ${selPaths.length} objects. Do you want to continue?`, {modal: true}, 'Yes', 'No');
            if (action !== 'Yes') {
                return undefined;
            }
        }

        return objList;
    }

    protected async copySelected(path : number[] | undefined, selPaths: number[][] | undefined) {
        let eol = StringHelper.getDefaultEndOfLine(undefined);
        let symbolList = await this.getObjectsFromPath(selPaths, AZSymbolKind.AnyALObject);
        if (symbolList) {
            let objectsText = 'Type\tId\tName';
            for (let i=0; i<symbolList.length; i++) {
                symbolList[i]
                objectsText += (eol + 
                    symbolList[i].getObjectTypeName() + '\t' + 
                    symbolList[i].id.toString() + '\t' + 
                    symbolList[i].name);
            }
            vscode.env.clipboard.writeText(objectsText);
        }
    }

    protected async createPage(path : number[] | undefined, selPaths: number[][] | undefined, pageType : string) {
        let symbolList = await this.getObjectsFromPath(selPaths, AZSymbolKind.TableObject);
        if (symbolList) {
            let builder : ALSymbolsBasedPageWizard = new ALSymbolsBasedPageWizard(this._devToolsContext);
            await builder.showWizard(symbolList, pageType);
        }
    }

    protected async createQuery(path : number[] | undefined, selPaths: number[][] | undefined) {
        let symbolList = await this.getObjectsFromPath(selPaths, AZSymbolKind.TableObject);
        if (symbolList) {
            let builder : ALSymbolsBasedQueryWizard = new ALSymbolsBasedQueryWizard(this._devToolsContext);
            builder.showWizard(symbolList);
        }
    }

    protected async createReport(path : number[] | undefined, selPaths: number[][] | undefined) {
        let symbolList = await this.getObjectsFromPath(selPaths, AZSymbolKind.TableObject);
        if (symbolList) {
            let builder : ALSymbolsBasedReportWizard = new ALSymbolsBasedReportWizard(this._devToolsContext);
            builder.showWizard(symbolList);
        }
    }

    protected async createXmlPort(path : number[] | undefined, selPaths: number[][] | undefined) {
        let symbolList = await this.getObjectsFromPath(selPaths, AZSymbolKind.TableObject);
        if (symbolList) {
            let builder : ALSymbolsBasedXmlPortWizard = new ALSymbolsBasedXmlPortWizard(this._devToolsContext);
            builder.showWizard(symbolList);
        }
    }

    protected async createPageExt(path : number[] | undefined, selPaths: number[][] | undefined) {
        let symbolList = await this.getObjectsFromPath(selPaths, AZSymbolKind.PageObject);
        if (symbolList) {
            let builder : ALSymbolsBasedPageExtWizard = new ALSymbolsBasedPageExtWizard(this._devToolsContext);
            builder.showWizard(symbolList);
        }
    }

    protected async createReportExt(path : number[] | undefined, selPaths: number[][] | undefined) {
        let symbolList = await this.getObjectsFromPath(selPaths, AZSymbolKind.ReportObject);
        if (symbolList) {
            let builder : ALSymbolsBasedReportExtWizard = new ALSymbolsBasedReportExtWizard(this._devToolsContext);
            builder.showWizard(symbolList);
        }
    }

    protected async createTableExt(path : number[] | undefined, selPaths: number[][] | undefined) {
        let symbolList = await this.getObjectsFromPath(selPaths, AZSymbolKind.TableObject);
        if (symbolList) {
            let builder : ALSymbolsBasedTableExtWizard = new ALSymbolsBasedTableExtWizard(this._devToolsContext);
            builder.showWizard(symbolList);
        }
    }

    protected async showNewTab(path: number[] | undefined) {
        if (!path)
            return;
        let alSymbolList : AZSymbolInformation[] | undefined = await this._library.getSymbolsListByPathAsync([path], AZSymbolKind.AnyALObject);
        if ((alSymbolList) && (alSymbolList.length > 0)) {
            let symbolsTreeView = new SymbolsTreeView(this._devToolsContext, 'lib://' + alSymbolList[0].fullName, undefined);
            symbolsTreeView.setSymbols(alSymbolList[0], alSymbolList[0].fullName);
            symbolsTreeView.show();
        }
    }

    protected async goToDefinition(path : number[] | undefined) {
        if (!path)
            return;
        let alSymbolList : AZSymbolInformation[] | undefined = await this._library.getSymbolsListByPathAsync([path], AZSymbolKind.AnyALObject);
        if ((alSymbolList) && (alSymbolList.length > 0)) {
            let preview = !vscode.workspace.getConfiguration('alOutline').get('openDefinitionInNewTab');
            let targetLocation : vscode.Location | undefined = undefined;
            let alSymbol : AZSymbolInformation = alSymbolList[0];
            
            //try to find symbol location
            let workspaceFolder: vscode.WorkspaceFolder | undefined = undefined;
            let libraryUri = this._library.getUri();
            if (libraryUri)
                workspaceFolder = vscode.workspace.getWorkspaceFolder(libraryUri);
            if ((!workspaceFolder) && (vscode.workspace.workspaceFolders) && (vscode.workspace.workspaceFolders.length > 0))
                workspaceFolder = vscode.workspace.workspaceFolders[0];

            //get location from extension language server
            if (workspaceFolder) {
                let projectSymbolResponse = await this._devToolsContext.toolsLangServerClient.getProjectSymbolLocation(
                    new ToolsGetProjectSymbolLocationRequest(workspaceFolder.uri.fsPath, alSymbol.kind.toString(), alSymbol.name));
                if ((projectSymbolResponse) && (projectSymbolResponse.location)) {
                    if (this.openALSymbolSourceLocation(projectSymbolResponse.location, workspaceFolder.name))
                        return;
                }
            }

            //use Microsoft AL Language Server to open definition
            let typeName : string | undefined = ALSyntaxHelper.kindToVariableType(alSymbol.kind);
            if (!typeName) {
                vscode.window.showErrorMessage('This object type is not supported.');
                return;    
            }
            
            targetLocation = await this._devToolsContext.alLangProxy.getDefinitionLocation(typeName, alSymbol.name);
    
            if (targetLocation) {
                TextEditorHelper.openEditor(targetLocation.uri, true, preview, targetLocation.range.start);
            } else {
                vscode.window.showErrorMessage('Object definition is not available.');
            }
        }
    }
    
    protected async goToLocalDefinition(path: number[] | undefined) {
        if (!path)
            return;
        let location = await this._library.getSymbolLocationByPath(path);
        if (!location)
            return;

        this.openALSymbolSourceLocation(location, '');

        /*
        let preview = !vscode.workspace.getConfiguration('alOutline').get('openDefinitionInNewTab');            
        let position: vscode.Position | undefined = undefined;
        if (location.range)
            position = new vscode.Position(location.range.start.line, location.range.start.character);

        if (location.schema == 'file')
            TextEditorHelper.openEditor(vscode.Uri.file(location.sourcePath), true, preview, position);
        else
            TextEditorHelper.openEditor(vscode.Uri.parse(AppFileTextContentProvider.scheme + ':' + location.sourcePath), true, preview, position);
        */
    }

    protected openALSymbolSourceLocation(location: ALSymbolSourceLocation, workspaceFolderName: string): boolean {
        if ((!location.schema) || (!location.sourcePath))
            return false;
        
        let preview = !vscode.workspace.getConfiguration('alOutline').get('openDefinitionInNewTab');            
        let position: vscode.Position | undefined = undefined;
        if (location.range)
            position = new vscode.Position(location.range.start.line, location.range.start.character);

        if (location.sourcePath) {
            if (location.schema == 'file') {
                TextEditorHelper.openEditor(vscode.Uri.file(location.sourcePath), true, preview, position);
                return true;
            } else if (location.schema == 'alapp') {
                TextEditorHelper.openEditor(vscode.Uri.parse(AppFileTextContentProvider.scheme + ':' + location.sourcePath), true, preview, position);
                return true;
            } else if (location.schema == 'al-preview') {
                let alPreviewUri = vscode.Uri.parse('al-preview://allang/' + workspaceFolderName + '/' + location.sourcePath);
                TextEditorHelper.openEditor(alPreviewUri, true, preview, position);
                return true;
            }
        }
        return false;
    }

    protected async runInWebClient(path : number[] | undefined) {
        if (!path)
            return;
        let alSymbolList : AZSymbolInformation[] | undefined = await this._library.getSymbolsListByPathAsync([path], AZSymbolKind.AnyALObject);
        if ((alSymbolList) && (alSymbolList.length > 0)) {
            this._devToolsContext.objectRunner.runSymbolAsync(alSymbolList[0]);
        }
    }

    protected onPanelClosed() {
        this._library.unloadAsync();
    }

    protected async updatePivotObjCommand(symbolPath: number[] | undefined) {
        let rootSymbol : AZSymbolInformation = AZSymbolInformation.create(AZSymbolKind.Document, 'Symbol');
        if ((symbolPath) && (symbolPath.length > 0)) {
            let pathList: number[][] = [symbolPath];
            let symbolList : AZSymbolInformation[] | undefined = await this._library.getSymbolsListByPathAsync(pathList, AZSymbolKind.AnyALObject);
            if ((symbolList) && (symbolList.length > 0))        
                rootSymbol.addChildItem(symbolList[0]);
        }
        this._devToolsContext.activeDocumentSymbols.setRootSymbol(rootSymbol);
    }


    protected async onObjectSelected(path : number[] | undefined) {       
        if (!path)
            return;
        let symbolList : AZSymbolInformation[] | undefined = await this._library.getSymbolsListByPathAsync([path], AZSymbolKind.AnyALObject);
        if ((symbolList) && (symbolList.length > 0))
            this._selectedObject = symbolList[0];
        else        
            this._selectedObject = undefined;
        if (this._selectedObject)
            this.sendMessage({
                command: 'setSelObjData',
                data: this._selectedObject
            });
    }

    protected switchViewMode(newTreeViewMode: boolean) {
        this._treeViewMode = newTreeViewMode;
        this._devToolsContext.setUseSymbolsBrowser(this._treeViewMode);
        this.resetViewView();
    }

} 
