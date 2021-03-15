import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { TableFieldInformation } from '../../symbolsinformation/tableFieldInformation';
import { TableFieldQuickPickItem } from './tableFieldQuickPickItem';

export class TableFieldsSelector {
    protected _sortNameImage: string;
    protected _sortIdImage: string;    
    protected _sortNameText: string;
    protected _sortIdText: string;
    protected _toolsExtensionContext : DevToolsExtensionContext;
    protected _sortBy: string;
    protected _quickPick: vscode.QuickPick<TableFieldQuickPickItem>;
    protected _selectedItems: TableFieldQuickPickItem[];

    constructor(context : DevToolsExtensionContext) {
        this._sortNameImage = 'ico-sorttext.svg';
        this._sortIdImage = 'ico-sortnumeric.svg';
        this._sortNameText = 'Sort by Name';
        this._sortIdText = 'Sort by Id';
        this._toolsExtensionContext = context;
        this._sortBy = 'name';
        this._selectedItems = [];
        this._quickPick = vscode.window.createQuickPick<TableFieldQuickPickItem>();
        this.initQuickPick();
    }

    selectFields(title: string, fieldsList: TableFieldInformation[]): Promise<TableFieldInformation[] | undefined> {
        let items: TableFieldQuickPickItem[] = [];
        for (let i=0; i<fieldsList.length; i++) {
            items.push(new TableFieldQuickPickItem(fieldsList[i]));
        }

        this._quickPick.title = title;
        this._quickPick.items = items;

        return new Promise<any>((resolve, reject) => {
            try {

                this._quickPick.show();
                this._quickPick.onDidAccept(() => {
                    let data: TableFieldInformation[] = [];
                    for (let i=0; i<this._quickPick.selectedItems.length; i++) {
                        data.push(this._quickPick.selectedItems[i].fieldInformation);
                    }
                    resolve(data);
                    this._quickPick.hide();
                });
                this._quickPick.onDidHide(() => {
                    resolve(undefined);
                });
            }
            catch(e) {
                reject(e);
            }
        });
    }

    protected initQuickPick() {
        this._quickPick.placeholder = 'Type to search';
        this._quickPick.canSelectMany = true;
        this._quickPick.buttons = [
            {
                iconPath: {
                    light: this._toolsExtensionContext.getLightImageUri(this._sortNameImage),
                    dark: this._toolsExtensionContext.getDarkImageUri(this._sortNameImage)
                },
                tooltip: this._sortNameText
            }, 
            {
                iconPath: {
                    light: this._toolsExtensionContext.getLightImageUri(this._sortIdImage),
                    dark: this._toolsExtensionContext.getDarkImageUri(this._sortIdImage)
                },
                tooltip: this._sortIdText
            }];

        this._quickPick.onDidTriggerButton(button => this.onButton(button));
        this._quickPick.onDidChangeSelection((itemList) => this.onSelectionChanged(itemList));
    }

    protected updateItems(newSelItems: TableFieldQuickPickItem[], forceUpdate: boolean) {
        let selectionOrder = this.isInSelectionOrderMode();
        let listChanged = !this.isListEqual(newSelItems, this._selectedItems);
        
        if ((selectionOrder && listChanged) || (forceUpdate)) {
            //collect not selected items
            let notSelItems: TableFieldQuickPickItem[] = [];
            for (let i=0; i<this._quickPick.items.length; i++) {
                if (newSelItems.indexOf(this._quickPick.items[i]) < 0) {
                    notSelItems.push(this._quickPick.items[i]);
                }
            }
            //sort and merge selected and not selected items
            if (selectionOrder)
                this.sortItems(notSelItems);

            let newItems = newSelItems.concat(notSelItems);
            
            if (!selectionOrder)
                this.sortItems(newItems);

            //update members and quick pick
            this._selectedItems = newSelItems;
            this._quickPick.items = newItems;
            this._quickPick.selectedItems = newSelItems;
        }
    }

    protected setSortBy(value: string) {       
        this._sortBy = value;
        this.updateItems(this._selectedItems, true);
    }

    protected sortItems(items: TableFieldQuickPickItem[]) {
        items.sort((a,b) => {
            if (this._sortBy == 'id')
                return this.compareValue(a.fieldInformation.id, b.fieldInformation.id);
            return this.compareValue(a.fieldInformation.name, b.fieldInformation.name);            
        });
    }

    protected compareValue(a: any, b:any) {
        if (a > b)
            return 1;
        if (a < b)
            return -1;
        return 0;
    }

    protected onSelectionChanged(itemList: TableFieldQuickPickItem[]) {
        this.updateItems(itemList, false);
    }

    protected onButton(button: vscode.QuickInputButton) {
        if (button.tooltip === this._sortIdText)
            this.setSortBy('id');
        else if (button.tooltip === this._sortNameText)
            this.setSortBy('name');
    }

    private isInSelectionOrderMode(): boolean {
        let resource: vscode.Uri | undefined = undefined;
        if ((vscode.window.activeTextEditor) && (vscode.window.activeTextEditor.document))
            resource = vscode.window.activeTextEditor.document.uri;
        let selectionMode = vscode.workspace.getConfiguration('alOutline', resource).get<string>('fieldsSelectionOrder');
        //convert undefined to boolean
        if ((selectionMode) && (selectionMode.toLowerCase() == 'selection order'))
            return true;
        return false;
    }

    protected isListEqual(a: TableFieldQuickPickItem[], b: TableFieldQuickPickItem[]): boolean {
        if (a.length != b.length)
            return false;
        for (let i=0; i<a.length; i++) {
            if (a[i].fieldInformation.id != b[i].fieldInformation.id)
                return false;
        }
        return true;
    }


}