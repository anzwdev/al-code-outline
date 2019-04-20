'use strict';

import * as path from 'path';
import { AZSymbolsLibrary } from '../symbolLibraries/azSymbolsLibrary';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { ALBaseSymbolsBrowser } from './alBaseSymbolsBrowser';
import { ALObjectsBrowser } from './alObjectsBrowser';

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
            data : this._library.objectListRootSymbol
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

    protected onObjectSelected(path : number[] | undefined) {       
        this._selectedObject = this._library.getObjectSymbolByPath(path);
        if (this._selectedObject)
            this.sendMessage({
                command: 'setSelObjData',
                data: this._selectedObject
            });
    }

    protected showListView() {
        this._devToolsContext.setUseSymbolsBrowser(false);
        this._panel.dispose();
        let objectsBrowser : ALObjectsBrowser = new ALObjectsBrowser(this._devToolsContext, this._library);
        objectsBrowser.show();
    }

} 
