import * as vscode from 'vscode';
import { TableFieldInformation } from '../../symbolsinformation/tableFieldInformation';
import { TableFieldQuickPickItem } from './tableFieldQuickPickItem';

export class TableFieldsSelector {

    selectFields(placeholder: string, fieldsList: TableFieldInformation[]): Promise<TableFieldInformation[] | undefined> {
        let items: TableFieldQuickPickItem[] = [];
        for (let i=0; i<fieldsList.length; i++) {
            items.push(new TableFieldQuickPickItem(fieldsList[i]));
        }

        let selectionOrder = this.isInSelectionOrderMode();
        let quickPick = vscode.window.createQuickPick<TableFieldQuickPickItem>();
        quickPick.placeholder = placeholder;
        quickPick.canSelectMany = true;
        quickPick.items = items;

        let selectedItems: TableFieldQuickPickItem[] = [];

        return new Promise<any>((resolve, reject) => {
            try {

                quickPick.show();
                
                quickPick.onDidChangeSelection((itemList) => {
                    if ((selectionOrder) && (!this.fieldsListsEquals(itemList, selectedItems))) {
                        //collect not selected items
                        let notSelItems:TableFieldQuickPickItem[] = [];
                        for (let i=0; i<quickPick.items.length; i++) {
                            if (itemList.indexOf(quickPick.items[i]) < 0) {
                                notSelItems.push(quickPick.items[i]);
                            }
                        }
                        notSelItems.sort((a,b) => {
                            if (a.label > b.label)
                                return 1;
                            if (a.label < b.label)
                                return -1;
                            return 0;
                        });

                        let newItems = itemList.concat(notSelItems);

                        selectedItems = itemList;
                        quickPick.items = newItems;
                        quickPick.selectedItems = selectedItems;
                    }
                });
                
                quickPick.onDidAccept(() => {
                    let data: TableFieldInformation[] = [];
                    for (let i=0; i<quickPick.selectedItems.length; i++) {
                        data.push(quickPick.selectedItems[i].fieldInformation);
                    }
                    resolve(data);
                    quickPick.hide();
                });
                quickPick.onDidHide(() => {
                    resolve(undefined);
                });
            }
            catch(e) {
                reject(e);
            }
        });
    }

    private fieldsListsEquals(fieldList1: TableFieldQuickPickItem[], fieldList2: TableFieldQuickPickItem[]): boolean {
        if (fieldList1.length != fieldList2.length)
            return false;
        for (let i=0; i<fieldList1.length; i++) {
            if (fieldList1[i].label != fieldList2[i].label)
                return false;
        }
        return true;
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

}