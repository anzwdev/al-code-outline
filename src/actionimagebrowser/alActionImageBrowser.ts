'use strict';

import * as vscode from 'vscode';
import * as path from 'path';
import { BaseWebViewEditor } from '../webviews/baseWebViewEditor';
import { ALActionImageInfo } from './alActionImageInfo';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { StringHelper } from '../tools/stringHelper';

export class ALActionImageBrowser extends BaseWebViewEditor {
    protected _devToolsContext: DevToolsExtensionContext;

    constructor(devToolsContext: DevToolsExtensionContext) {
        super(devToolsContext.vscodeExtensionContext , "Action Images");
        this._devToolsContext = devToolsContext;
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'actionimagebrowser', 'imagebrowser.html');
    }

    protected getViewType() : string {
        return 'azALDevTools.ALActionImageBrowser';
    }

    protected onDocumentLoaded() {
        this.loadData();
    }

    async loadData() {
        let data: ALActionImageInfo[] = await this.getImageList();
        this.sendMessage({
            command : 'setData',
            data : data
        });
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message))
            return true;

        switch (message.command) {
            case 'copyname':
                this.copyName(message.name, message.withui);
                break;
            case 'copyaction':
                this.copyAction(message.name, message.withui);
                return true;
            case 'copypromotedaction':
                this.copyPromotedAction(message.name, message.withui);
                return true;
        }
        return false;
    }

    protected async copyName(name: string, withUI: boolean) {
        await vscode.env.clipboard.writeText(name);
        if (withUI)
            vscode.window.showInformationMessage('Image name has been copied to the clipboard');
    }

    protected async copyAction(name: string, withUI: boolean) {
        let eol = StringHelper.getDefaultEndOfLine(undefined);
        await vscode.env.clipboard.writeText(
            'action(' + name + 'Action)' + eol +
            '{' + eol +
            '    ApplicationArea = All;' + eol +
            '    Image = ' + name + ';' + eol +
            '' + eol +
            '    trigger OnAction()' + eol +
            '    begin' + eol +
            '' + eol +
            '    end;' + eol +
            '}' + eol);
        if (withUI)
            vscode.window.showInformationMessage('Action code has been copied to the clipboard');
    }

    protected async copyPromotedAction(name: string, withUI: boolean) {
        let eol = StringHelper.getDefaultEndOfLine(undefined);
        await vscode.env.clipboard.writeText(
            'action(' + name + 'Action)' + eol +
            '{' + eol +
            '    ApplicationArea = All;' + eol +
            '    Image = ' + name + ';' + eol +
            '    Promoted = true;' + eol +
            '    PromotedCategory = Process;' + eol +
            eol +
            '    trigger OnAction()' + eol +
            '    begin' + eol +
            eol +
            '    end;' + eol +
            '}' + eol);
        if (withUI)
            vscode.window.showInformationMessage('Promoted action code has been copied to the clipboard');
    }

    async getImageList() : Promise<ALActionImageInfo[]> {
        let fileContent = 'page 0 MyPage9999\n{\nactions\n{\narea(Processing)\n{\naction(ActionName)\n{\nImage=;\n}\n}\n}\n}';

        let list = await this._devToolsContext.alLangProxy.getCompletionForSourceCode(undefined, 'Loading list of action images.', fileContent,
            8, 6, 12, 1);

        //process results
        let out : ALActionImageInfo[] = [];
        
        if (list && list.items) {
            for (let i=0; i<list.items.length; i++) {
                let item : vscode.CompletionItem = list.items[i];
                if (item.kind == vscode.CompletionItemKind.Property) {
                    if (item.documentation) {
                        let docContent : any = item.documentation;
                        let imageString = docContent.value;
                        //decode image
                        let pos : number = imageString.indexOf('(data');
                        if (pos >= 0)
                            imageString = imageString.substr(pos + 1);
                        pos = imageString.indexOf(')');
                        if (pos >= 0)
                            imageString = imageString.substr(0, pos);                            

                        let imageInfo : ALActionImageInfo = new ALActionImageInfo();
                        imageInfo.name = item.label;
                        imageInfo.content = imageString;
                        out.push(imageInfo);
                    }

                //    out.push(ALSyntaxHelper.fromNameText(item.label));
                }
            }
        }

        return out;
        
    }

} 