import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { AppAreasModifier } from '../alsyntaxmodifiers/appAreasModifier';
import { ToolTipModifier } from '../alsyntaxmodifiers/toolTipsModifier';
import { DataClassificationModifier } from '../alsyntaxmodifiers/dataClassificationModifier';
import { OnDocumentSaveModifier } from '../alsyntaxmodifiers/onDocumentSaveModifier';
import { SyntaxModifier } from '../alsyntaxmodifiers/syntaxModifier';
import { WithModifier } from '../alsyntaxmodifiers/withModifier';

export class ALCodeTransformationService {
    protected _context: DevToolsExtensionContext;

    constructor(context: DevToolsExtensionContext) {
        this._context = context;

        this.registerDocumentRangeCommand('azALDevTools.sortVariables', 'sortVariables');
        this.registerDocumentRangeCommand('azALDevTools.sortProcedures', 'sortProcedures');
        this.registerDocumentRangeCommand('azALDevTools.sortProperties', 'sortProperties');
        this.registerDocumentRangeCommand('azALDevTools.sortReportColumns', 'sortReportColumns');

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.fixDocumentOnSave',
                async (document) => {
                    let cmd = new OnDocumentSaveModifier(this._context);
                    await cmd.RunForDocument(document, undefined, false);
                }
            )
        );

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.RemoveProjectWithStatements',
                () => {
                    let withModified: WithModifier = new WithModifier(this._context);
                    withModified.RunForWorkspace();
                }
            )
        );       

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

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.AddEditorDataClassification',
                () => {
                    let modifier: DataClassificationModifier = new DataClassificationModifier(this._context);
                    modifier.AddMissingDataClassificationToActiveEditor();
                }
            )
        );

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.AddProjectDataClassification',
                () => {
                    let modifier: DataClassificationModifier = new DataClassificationModifier(this._context);
                    modifier.AddMissinDataClassificationToWorkspace();
                }
            )
        );


    }

    protected registerDocumentCommand(name: string, modifierFactory: () => SyntaxModifier) {
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                name,
                async (document) => {
                    let cmd = modifierFactory();
                    await cmd.RunForDocument(document, undefined, false);
                }
            )
        );
    }

    protected registerDocumentRangeCommand(name: string, workspaceCommandName: string) {
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                name,
                async (document, range) => {
                    let cmd = new SyntaxModifier(this._context, workspaceCommandName);
                    await cmd.RunForDocument(document, range, false);
                }
            )
        );
    }

}