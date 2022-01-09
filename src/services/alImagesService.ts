import * as vscode from 'vscode';
import { ALImageBrowser } from '../imagebrowser/alImageBrowser';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { DevToolsExtensionService } from './devToolsExtensionService';

export class ALImagesService extends DevToolsExtensionService {

    constructor(context: DevToolsExtensionContext) {
        super(context);
        this.registerCommands();
    }

    protected registerCommands() {
        this.registerImageBrowserCommand('azALDevTools.viewActionImages', 'Action Images', 'actionImages', true, 'imageaction');
        this.registerImageBrowserCommand('azALDevTools.viewCueGroupActionImages', 'CueGroup Action Images', 'actionCueGroupImages', true, 'imagecgaction');
        this.registerImageBrowserCommand('azALDevTools.viewCueGroupFieldsImages', 'CueGroup Fields Images', 'fieldCueGroupImages', false, 'imagecgfld');
        this.registerImageBrowserCommand('azALDevTools.viewRoleCenterActionImages', 'Role Center Action Images', 'roleCenterActionImages', true, 'imagercaction');
    }

    protected registerImageBrowserCommand(commandName: string, caption: string, imagesType: string, withActions: boolean, imageStyleType: string) {
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                commandName, () => {
                    let imageBrowser: ALImageBrowser = new ALImageBrowser(this._context, caption, imagesType, withActions, imageStyleType);
                    imageBrowser.show();
                }
            ));
    }

}