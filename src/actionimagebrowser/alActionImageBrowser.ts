'use strict';

import * as vscode from 'vscode';
import * as path from 'path';
import { BaseWebViewEditor } from '../webviews/baseWebViewEditor';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { StringHelper } from '../tools/stringHelper';
import { ToolsGetImagesRequest } from '../langserver/languageInformation/toolsGetImagesRequest';
import { ImageInformation } from '../langserver/languageInformation/imageInformation';

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
        let data: ImageInformation[] | undefined = await this.getImageList();
        if (data)
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

    async getImageList() : Promise<ImageInformation[] | undefined> {
        return await vscode.window.withProgress<ImageInformation[] | undefined>({
            location: vscode.ProgressLocation.Notification,
            title: 'Loading list of action images.'
        }, async (progress) => {
            let response = await this._devToolsContext.toolsLangServerClient.getImages(new ToolsGetImagesRequest('actionImages'));
            if ((response) && (response.images))
                return response.images;
            return [];   
        });
    }

} 