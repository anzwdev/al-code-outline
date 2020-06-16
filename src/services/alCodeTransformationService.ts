import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { AppAreasModifier } from '../alsyntaxmodifiers/appAreasModifier';

export class ALCodeTransformationService {
    protected _context: DevToolsExtensionContext;
    
    constructor(context: DevToolsExtensionContext) {
        this._context = context;

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.AddEditorApplicationAreas',
                () => {
                    let appAreasModified: AppAreasModifier = new AppAreasModifier(this._context);
                    appAreasModified.AddMissingAppAreaToActiveEditor(undefined);
                }
            )
        );

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.AddProjectApplicationAreas',
                () => {
                    let appAreasModified: AppAreasModifier = new AppAreasModifier(this._context);
                    appAreasModified.AddMissinAppAreaToWorkspace(undefined);
                }
            )
        );

    }



}