import * as vscode from 'vscode';
import * as path from 'path';
import { SymbolsViewerConst } from './symbolsViewerConst';
import { SymbolsViewerViewMode } from './symbolsViewerViewMode';
import { BaseWebViewEditor } from '../../ui_core/baseWebViewEditor';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { SymbolsViewerLoader } from './symbolsViewerLoader';
import { LSSVSymbvolsViewerNode } from '../../langserver/symbols_viewer/lssvSymbolsViewerNode';
import { SymbolsViewerDocument } from './symbolsViewerDocument';
import { LSSyntaxNodeKindHelper } from '../../langserver/common_types/lsSyntaxNodeKindHelper';
import { LSSyntaxNodeKind } from '../../langserver/common_types/lsSyntaxNodeKind';
import { LSSVSymbvolsViewerNodeHelper } from '../../langserver/symbols_viewer/lssvSymbolsViewerNodeHelper';
import { SymbolsViewerCodeOutlineTreeLoader } from './symbolsViewerCodeOutlineTreeLoader';
import { LSSPLocation } from '../../langserver/app_symbol_source_provider/lsspLocation';
import { TextEditorHelper } from '../../ui_core/textEditorHelper';
import { AppFileTextContentProviderConst } from '../app_symbol_source_provider/appFileTextContentProviderConst';
import { DevToolsExtensionSettings } from '../extension_settings/devToolsExtensionSettings';

/**
 * AL Symbols Browser
 * allows to browse symbols in a tree structure like in the Class Browser in Visual Studio
 * and in a list view like in old Dynamics Nav object browser
 */
export class ALSymbolsBrowser extends BaseWebViewEditor {
    protected _loader: SymbolsViewerLoader;
    protected _showObjectIds : boolean;
    protected _viewMode : SymbolsViewerViewMode;
    protected _showLibraries: boolean;
    protected _document?: SymbolsViewerDocument;

    protected _symbolsTreeRoot?: LSSVSymbvolsViewerNode;
    protected _objectsList: LSSVSymbvolsViewerNode[];
    protected _symbolsByUid: Map<number, LSSVSymbvolsViewerNode>;
    protected _selectedObjectRoot?: LSSVSymbvolsViewerNode;
    protected _codeOutlineTreeLoader: SymbolsViewerCodeOutlineTreeLoader;

    constructor(context : DevToolsExtensionContext, loader: SymbolsViewerLoader, viewType: string) {
        super(context, loader.getName(), viewType);

        this._document = undefined;
        this._loader = loader;
        this._viewMode = this.context.getGlobalStateSetting<SymbolsViewerViewMode>(SymbolsViewerConst.stateViewMode, SymbolsViewerViewMode.List);
        this._codeOutlineTreeLoader = new SymbolsViewerCodeOutlineTreeLoader(loader.getName());

        //tree view properties
        this._symbolsTreeRoot = undefined;
        this._selectedObjectRoot = undefined;
        this._showObjectIds = false;

        //list view properties
        this._objectsList = [];
        this._showLibraries = false;
        this._symbolsByUid = new Map<number, LSSVSymbvolsViewerNode>();
    }

    protected getHtmlContentPath() : string {
        switch (this._viewMode) {
            case SymbolsViewerViewMode.Tree:
                return path.join('htmlresources', 'alsymbolsbrowser', 'symbolsbrowser.html');                
            case SymbolsViewerViewMode.List:
            default:
                return path.join('htmlresources', 'objectbrowser', 'objectbrowser.html');
        }
    }

    protected async onDocumentLoaded() {
        await this.loadSymbolsTree();
        this.updateView();
    }

    protected async loadSymbolsTree() {
        this._document = await this._loader.load();
        this._symbolsTreeRoot = this._document?.root;
        if (this._symbolsTreeRoot) {
            LSSVSymbvolsViewerNodeHelper.updateIcon(this._symbolsTreeRoot);
        }

        this._symbolsByUid.clear();
        if (this._symbolsTreeRoot) {
            this.buildSymbolsByUidMap(this._symbolsTreeRoot);
        }

        this.buildObjectsList();

        this._selectedObjectRoot = undefined;
    }

    protected buildSymbolsByUidMap(symbol: LSSVSymbvolsViewerNode) {
        if (symbol.uid !== undefined) {
            this._symbolsByUid.set(symbol.uid, symbol);
        }
        if (symbol.childSymbols) {
            for (let i=0; i<symbol.childSymbols.length; i++) {
                this.buildSymbolsByUidMap(symbol.childSymbols[i]);
            }
        }
    }

    protected getSymbolsByUids(uids: number[] | undefined) : LSSVSymbvolsViewerNode[] | undefined {
        if ((!uids) || (uids.length === 0)) {
            return undefined;
        }

        let symbols: LSSVSymbvolsViewerNode[] = [];
        for (let i=0; i<uids.length; i++) {
            let symbol = this._symbolsByUid.get(uids[i]);
            if (symbol) {
                symbols.push(symbol);
            }
        }
        return symbols;
    }

    protected buildObjectsList() {
        this._objectsList = [];
        this._showLibraries = false;
        if (this._symbolsTreeRoot) {
            this.collectObjectListSymbols(this._symbolsTreeRoot, '');
        }
    }

    protected collectObjectListSymbols(symbol: LSSVSymbvolsViewerNode, libraryName: string | undefined) {
        if (LSSyntaxNodeKindHelper.isALObject(symbol.kind)) {
            symbol.library = libraryName;
            this._objectsList.push(symbol);
        } else if (symbol.childSymbols) {
            if (symbol.kind === LSSyntaxNodeKind.Package) {
                libraryName = symbol.name;
                if ((!this._showLibraries) && (this._objectsList.length > 0)) {
                    this._showLibraries = true;
                }
            }

            for (let i=0; i<symbol.childSymbols.length; i++) {
                this.collectObjectListSymbols(symbol.childSymbols[i], libraryName);
            }
        }
    }

    protected updateView() {
        switch (this._viewMode) {
            case SymbolsViewerViewMode.Tree:
                this.updateTreeView();
                break;
            case SymbolsViewerViewMode.List:
            default:
                this.updateListView();
                break;
        }
    }

    protected updateTreeView() {        
        this.sendMessage({
            command : 'setData',
            data : this._symbolsTreeRoot
        });
    }

    protected updateListView() {

        //send data to the web view
        this.sendMessage({
            command: 'setData',
            data: this._objectsList,
            showLibraries: this._showLibraries
        });
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message)) {
            return true;
        }
       
        switch (message.command) {
            case 'definition':
                this.goToDefinition(message.uid, false);
                return true;
            case 'localdefinition':
                this.goToDefinition(message.uid, true);
                return true;
            /*
            case 'shownewtab':
                this.showNewTab(message.uid);
                break;
            case 'runinwebclient':
                this.runInWebClient(message.uid);                
                return true;
            case 'copysel':
                this.copySelected(message.uid, message.seluids);
                return true;
                */
            case 'showlist':
                this.switchViewMode(SymbolsViewerViewMode.List);
                break;
            case 'showTreeView':
                this.switchViewMode(SymbolsViewerViewMode.Tree);
                break;
            case 'objselected':
                this.onObjectSelected(message.uid);
                return true;
            case 'currRowChanged':                
                this.onObjectSelected(message.uid);
                return true;
            }

        return false;
    }

    protected async goToDefinition(uid : number | undefined, directAppFileAccess: boolean) {
        if ((!this._document) || (!uid)) {
            return;
        }

        let symbol = this._symbolsByUid.get(uid);
        if (!symbol) {
            return;
        }

        let getLocationResponse = await this._loader.lsClient.getSymbolLocation({ 
            documentUid : this._document.uid, 
            objectUid: uid,
            directAppFileAccess: directAppFileAccess });
        
        if (getLocationResponse?.location) {
            this.openALSymbolSourceLocation(getLocationResponse.location);
        }
    }

    protected openALSymbolSourceLocation(location: LSSPLocation): boolean {
        if ((!location.schema) || (!location.sourcePath)) {
            return false;
        }
        
        let workspaceFolder: vscode.WorkspaceFolder | undefined = undefined;
        if (location.containerPath) {
            workspaceFolder = vscode.workspace.getWorkspaceFolder(vscode.Uri.file(location.containerPath));
        }
        if ((!workspaceFolder) && (vscode.workspace.workspaceFolders) && (vscode.workspace.workspaceFolders.length > 0)) {
            workspaceFolder = vscode.workspace.workspaceFolders[0];           
        }

        let settings = new DevToolsExtensionSettings(workspaceFolder?.uri);
        let preview = !settings.getOpenDefinitionInNewTab();
        let position: vscode.Position | undefined = undefined;
        if (location.range?.start) {
            position = new vscode.Position(location.range.start.line, location.range.start.character);
        }

        if (location.sourcePath) {
            if (location.schema === 'file') {
                TextEditorHelper.openEditor(vscode.Uri.file(location.sourcePath), true, preview, position);
                return true;
            } else if (location.schema === 'alapp') {
                TextEditorHelper.openEditor(vscode.Uri.parse(AppFileTextContentProviderConst.scheme + ':' + location.sourcePath), true, preview, position);
                return true;
            } else if (location.schema === 'al-preview') {
                let workspaceFolderName = workspaceFolder ? workspaceFolder.name : 'unknown';
                let alPreviewUri = vscode.Uri.parse('al-preview://allang/' + workspaceFolderName + '/' + encodeURIComponent(location.sourcePath));
                TextEditorHelper.openEditor(alPreviewUri, true, preview, position);
                return true;
            }
        }
        return false;
    }

    /*
    protected async copySelected(uids : number[] | undefined) {
        let eol = StringHelper.getDefaultEndOfLine(undefined);       
        let symbolList = this.getSymbolsByUids(uids);
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
        */

    /*
    protected async showNewTab(uid: number | undefined) {
        if (!uid) {
            return;
        }
        let alSymbolList : AZSymbolInformation[] | undefined = await this._library.getSymbolsListByPathAsync([path], AZSymbolKind.AnyALObject);
        if ((alSymbolList) && (alSymbolList.length > 0)) {
            let symbolsTreeView = new SymbolsTreeView(this._devToolsContext, 'lib://' + alSymbolList[0].fullName, undefined);
            symbolsTreeView.setSymbols(alSymbolList[0], alSymbolList[0].fullName);
            symbolsTreeView.show();
        }
    }
    
    protected async runInWebClient(path : number[] | undefined) {
        if (!path)
            return;
        let alSymbolList : AZSymbolInformation[] | undefined = await this._library.getSymbolsListByPathAsync([path], AZSymbolKind.AnyALObject);
        if ((alSymbolList) && (alSymbolList.length > 0)) {
            this._devToolsContext.objectRunner.runSymbolAsync(alSymbolList[0]);
        }
    }
    */

    protected onPanelClosed() {
        if ((this._document) && (this._document.uid !== undefined) && (this._document.uid !== -1)) {
            this._loader.lsClient.CloseDocument({documentUid: this._document.uid});
        }
    }

    protected async onObjectSelected(uid : number | undefined) {       
        if ((!this._document) || (this._document.uid === undefined) || (!uid)) {
            return;
        }
        
        let getObjectResponse = await this._loader.lsClient.GetObject({ documentUid : this._document.uid, objectUid: uid });
        this._selectedObjectRoot = getObjectResponse?.root;
        if (this._selectedObjectRoot) {
            LSSVSymbvolsViewerNodeHelper.updateIcon(this._selectedObjectRoot);
        }

        this.updateSelectedObjectView();        
    }

    protected updateSelectedObjectView() {
        if (this._viewMode === SymbolsViewerViewMode.Tree) {
            this.updateSelectedObjectTreeView();
        }

        this.updateCodeOutlineView();
    }

    protected updateSelectedObjectTreeView() {
        this.sendMessage({
            command: 'setSelObjData',
            data: this._selectedObjectRoot
        });
    }

    protected updateCodeOutlineView() {
        this._codeOutlineTreeLoader.setSelectedSymbol(this._selectedObjectRoot);
        this.context.codeOutlineViewService.updateCurrentTreeLoader(this._codeOutlineTreeLoader);
    }

    protected switchViewMode(newViewMode: SymbolsViewerViewMode) {
        this._viewMode = newViewMode;
        this.context.setGlobalStateSetting<SymbolsViewerViewMode>(SymbolsViewerConst.stateViewMode, this._viewMode);
        this.resetViewView();
    }
    
} 
