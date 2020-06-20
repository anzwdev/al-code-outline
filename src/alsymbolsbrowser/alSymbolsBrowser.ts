'use strict';

import * as path from 'path';
import { AZSymbolsLibrary } from '../symbollibraries/azSymbolsLibrary';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { ALBaseSymbolsBrowser } from './alBaseSymbolsBrowser';
import { ALObjectsBrowser } from './alObjectsBrowser';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';

/**
 * AL Symbols Browser
 * allows to browse symbols in a tree structure like in the Class Browser in Visual Studio
 */
export class ALSymbolsBrowser extends ALBaseSymbolsBrowser {
    protected _selectedObject : AZSymbolInformation | undefined;
    protected _showObjectIds : boolean;

    constructor(devToolsContext : DevToolsExtensionContext,  library : AZSymbolsLibrary) {
        super(devToolsContext, library);
        this._selectedObject = undefined;
        this._showObjectIds = false;
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'alsymbolsbrowser', 'symbolsbrowser.html');
    }

    protected getViewType() : string {
        return 'azALDevTools.ALSymbolsBrowser';
    }

    protected async onDocumentLoaded() {
        //load library
        await this._library.loadAsync(false);

        //send data to the web view
        this.updateObjects();
    }

    protected updateObjects() {        
        this.sendMessage({
            command : 'setData',
            data : this._library.rootSymbol
        });
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message))
            return true;

        switch (message.command) {
            case 'showlist':
                this.showListView();
                break;
            case 'objselected':
                this.onObjectSelected(message.path);
                return true;
        }

        return false;
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

    protected showListView() {
        this._devToolsContext.setUseSymbolsBrowser(false);
        if (this._panel)
            this._panel.dispose();
        let objectsBrowser : ALObjectsBrowser = new ALObjectsBrowser(this._devToolsContext, this._library);
        objectsBrowser.show();
    }

} 
