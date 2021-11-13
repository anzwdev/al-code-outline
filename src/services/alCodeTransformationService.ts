import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { AppAreasModifier } from '../alsyntaxmodifiers/appAreasModifier';
import { ToolTipModifier } from '../alsyntaxmodifiers/toolTipsModifier';
import { DataClassificationModifier } from '../alsyntaxmodifiers/dataClassificationModifier';
import { OnDocumentSaveModifier } from '../alsyntaxmodifiers/onDocumentSaveModifier';
import { SyntaxModifier } from '../alsyntaxmodifiers/syntaxModifier';
import { WithModifier } from '../alsyntaxmodifiers/withModifier';
import { FieldCaptionsModifier } from '../alsyntaxmodifiers/fieldCaptionsModifier';
import { PageControlsCaptionsModifier } from '../alsyntaxmodifiers/pageControlsCaptionsModifier';
import { ObjectCaptionsModifier } from '../alsyntaxmodifiers/objectCaptionsModifier';
import { DevToolsExtensionService } from './devToolsExtensionService';
import { FixKeywordsCaseModifier } from '../alsyntaxmodifiers/fixKeywordsCaseModifier';
import { FixIdentifiersCaseModifier } from '../alsyntaxmodifiers/fixIdentifiersCaseModifier';
import { ConvertObjectIdsToNamesModifier } from '../alsyntaxmodifiers/convertObjectIdsToNamesModifier';
import { RemoveUnusedVariablesModifier } from '../alsyntaxmodifiers/removeUnusedVariablesModifier';
import { AddMissingParenthesesModifier } from '../alsyntaxmodifiers/addMissingParenthesesModifier';

export class ALCodeTransformationService extends DevToolsExtensionService {

    constructor(context: DevToolsExtensionContext) {
        super(context);

        this.registerDocumentRangeCommand('azALDevTools.sortVariables', 'sortVariables');
        this.registerDocumentRangeCommand('azALDevTools.sortProcedures', 'sortProcedures');
        this.registerDocumentRangeCommand('azALDevTools.sortProperties', 'sortProperties');
        this.registerDocumentRangeCommand('azALDevTools.sortReportColumns', 'sortReportColumns');
        this.registerDocumentRangeCommand('azALDevTools.sortTableFields', 'sortTableFields');
        this.registerDocumentRangeCommand('azALDevTools.sortPermissions', 'sortPermissions');
        this.registerDocumentRangeCommand('azALDevTools.sortPermissionSetList', 'sortPermissionSetList');

        this.registerDocumentRangeCommand('azALDevTools.addAllObjectsPermissions', 'addAllObjectsPermissions');

        this.registerDocumentRangeCommand('azALDevTools.removeVariable', 'removeVariable');

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
                'azALDevTools.RemoveEditorWithStatements',
                () => {
                    let withModifier: WithModifier = new WithModifier(this._context);
                    withModifier.RunForActiveEditor();
                }
            )
        );

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.RemoveProjectWithStatements',
                () => {
                    let withModifier: WithModifier = new WithModifier(this._context);
                    withModifier.RunForWorkspace();
                }
            )
        );       

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.AddEditorApplicationAreas',
                () => {
                    let appAreasModifier: AppAreasModifier = new AppAreasModifier(this._context);
                    appAreasModifier.AddMissingAppAreaToActiveEditor(undefined);
                }
            )
        );

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.AddProjectApplicationAreas',
                () => {
                    let appAreasModifier: AppAreasModifier = new AppAreasModifier(this._context);
                    appAreasModifier.AddMissinAppAreaToWorkspace(undefined);
                }
            )
        );

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.AddEditorToolTip',
                () => {
                    let toolTipModifier: ToolTipModifier = new ToolTipModifier(this._context);
                    toolTipModifier.AddMissingToolTipToActiveEditor(undefined);
                }
            )
        );

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.AddProjectToolTip',
                () => {
                    let toolTipModifier: ToolTipModifier = new ToolTipModifier(this._context);
                    toolTipModifier.AddMissinToolTipToWorkspace(undefined);
                }
            )
        );

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.AddEditorFieldCaption',
                () => {
                    let fieldCaptionsModifier: FieldCaptionsModifier = new FieldCaptionsModifier(this._context);
                    fieldCaptionsModifier.RunForActiveEditor();
                }
            )
        );

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.AddProjectFieldCaption',
                () => {
                    let fieldCaptionsModifier: FieldCaptionsModifier = new FieldCaptionsModifier(this._context);
                    fieldCaptionsModifier.RunForWorkspace();
                }
            )
        );

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.AddEditorPageFieldCaption',
                () => {
                    let pageFieldCaptionsModifier: PageControlsCaptionsModifier = new PageControlsCaptionsModifier(this._context);
                    pageFieldCaptionsModifier.RunForActiveEditor();
                }
            )
        );

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.AddProjectPageFieldCaption',
                () => {
                    let pageFieldCaptionsModifier: PageControlsCaptionsModifier = new PageControlsCaptionsModifier(this._context);
                    pageFieldCaptionsModifier.RunForWorkspace();
                }
            )
        );

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.AddEditorObjectCaption',
                () => {
                    let objectCaptionsModifier: ObjectCaptionsModifier = new ObjectCaptionsModifier(this._context);
                    objectCaptionsModifier.RunForActiveEditor();
                }
            )
        );

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.AddProjectObjectCaption',
                () => {
                    let objectCaptionsModifier: ObjectCaptionsModifier = new ObjectCaptionsModifier(this._context);
                    objectCaptionsModifier.RunForWorkspace();
                }
            )
        );

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.FixEditorKeywordsCase',
                () => {
                    let fixKeywordsCaseModifier: FixKeywordsCaseModifier = new FixKeywordsCaseModifier(this._context);
                    fixKeywordsCaseModifier.RunForActiveEditor();
                }
            )
        );

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.FixProjectKeywordsCase',
                () => {
                    let fixKeywordsCaseModifier: FixKeywordsCaseModifier = new FixKeywordsCaseModifier(this._context);
                    fixKeywordsCaseModifier.RunForWorkspace();
                }
            )
        );

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.FixEditorIdentifiersCase',
                () => {
                    let fixIdentifiersCaseModifier: FixIdentifiersCaseModifier = new FixIdentifiersCaseModifier(this._context);
                    fixIdentifiersCaseModifier.RunForActiveEditor();
                }
            )
        );

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.FixProjectIdentifiersCase',
                () => {
                    let fixIdentifiersCaseModifier: FixIdentifiersCaseModifier = new FixIdentifiersCaseModifier(this._context);
                    fixIdentifiersCaseModifier.RunForWorkspace();
                }
            )
        );

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.ConvertEditorObjectIdsToNames',
                () => {
                    let convertIdsToNamesModifier: ConvertObjectIdsToNamesModifier = new ConvertObjectIdsToNamesModifier(this._context);
                    convertIdsToNamesModifier.RunForActiveEditor();
                }
            )
        );

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.ConvertProjectObjectIdsToNames',
                () => {
                    let convertIdsToNamesModifier: ConvertObjectIdsToNamesModifier = new ConvertObjectIdsToNamesModifier(this._context);
                    convertIdsToNamesModifier.RunForWorkspace();
                }
            )
        );

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.AddMissingEditorParentheses',
                () => {
                    let addMissingParenthesesModifier: AddMissingParenthesesModifier = new AddMissingParenthesesModifier(this._context);
                    addMissingParenthesesModifier.RunForActiveEditor();
                }
            )
        );

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.AddMissingProjectParentheses',
                () => {
                    let addMissingParenthesesModifier: AddMissingParenthesesModifier = new AddMissingParenthesesModifier(this._context);
                    addMissingParenthesesModifier.RunForWorkspace();
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

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.RemoveEditorUnusedVariables',
                () => {
                    let removeUnusedVariablesModifier: RemoveUnusedVariablesModifier = new RemoveUnusedVariablesModifier(this._context);
                    removeUnusedVariablesModifier.RunForActiveEditor();
                }
            )
        );

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.RemoveProjectUnusedVariables',
                () => {
                    let removeUnusedVariablesModifier: RemoveUnusedVariablesModifier = new RemoveUnusedVariablesModifier(this._context);
                    removeUnusedVariablesModifier.RunForWorkspace();
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