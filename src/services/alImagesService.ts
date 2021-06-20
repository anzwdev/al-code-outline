import * as vscode from 'vscode';
import { ALActionImageBrowser } from '../actionimagebrowser/alActionImageBrowser';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { DevToolsExtensionService } from './devToolsExtensionService';

export class ALImagesService extends DevToolsExtensionService {

    constructor(context: DevToolsExtensionContext) {
        super(context);
        this.registerCommands();
    }

    protected registerCommands() {
        //al action images viewer
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.viewActionImages',
                () => {
                    let actionImageBrowser: ALActionImageBrowser = new ALActionImageBrowser(this._context);
                    actionImageBrowser.show();
                }
            ));

    }

}