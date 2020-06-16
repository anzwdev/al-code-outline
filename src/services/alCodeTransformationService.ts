import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { AppAreasModifier } from '../alsyntaxmodifiers/appAreasModifier';
import { ToolTipModifier } from '../alsyntaxmodifiers/toolTipsModifier';

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

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.AddEditorToolTip',
                () => {
                    let ToolTipModified: ToolTipModifier = new ToolTipModifier(this._context);
                    ToolTipModified.AddMissingToolTipToActiveEditor(undefined);
                }
            )
        );

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.AddProjectToolTip',
                () => {
                    let ToolTipModified: ToolTipModifier = new ToolTipModifier(this._context);
                    ToolTipModified.AddMissinToolTipToWorkspace(undefined);
                }
            )
        );

    }



}