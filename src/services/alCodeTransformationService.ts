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
import { SortProceduresModifier } from '../alsyntaxmodifiers/sortProceduresModifier';
import { SortPermissionsModifier } from '../alsyntaxmodifiers/sortPermissionsModifier';
import { SortPropertiesModifier } from '../alsyntaxmodifiers/sortPropertiesModifier';
import { SortReportColumnsModifier } from '../alsyntaxmodifiers/sortReportColumnsModifier';
import { SortTableFieldsModifier } from '../alsyntaxmodifiers/sortTableFieldsModifier';
import { SortVariablesModifier } from '../alsyntaxmodifiers/sortVariablesModifier';
import { SortPermissionSetListModifier } from '../alsyntaxmodifiers/sortPermissionSetListModifier';
import { ISyntaxModifierFactoriesCollection } from '../alsyntaxmodifiers/iSyntaxModifierFactoriesCollection';
import { BatchSyntaxModifier } from '../alsyntaxmodifiers/batchSyntaxModifier';
import { WorkspaceCommandSyntaxModifier } from '../alsyntaxmodifiers/workspaceCommandSyntaxModifier';

export class ALCodeTransformationService extends DevToolsExtensionService {
    protected _syntaxFactories: ISyntaxModifierFactoriesCollection;

    constructor(context: DevToolsExtensionContext) {
        super(context);

        this._syntaxFactories = {};

        //document range commands
        this.registerDocumentRangeCommand('azALDevTools.sortVariables', 'sortVariables');
        this.registerDocumentRangeCommand('azALDevTools.sortProcedures', 'sortProcedures');
        this.registerDocumentRangeCommand('azALDevTools.sortProperties', 'sortProperties');
        this.registerDocumentRangeCommand('azALDevTools.sortReportColumns', 'sortReportColumns');
        this.registerDocumentRangeCommand('azALDevTools.sortTableFields', 'sortTableFields');
        this.registerDocumentRangeCommand('azALDevTools.sortPermissions', 'sortPermissions');
        this.registerDocumentRangeCommand('azALDevTools.sortPermissionSetList', 'sortPermissionSetList');
        this.registerDocumentRangeCommand('azALDevTools.addAllObjectsPermissions', 'addAllObjectsPermissions');
        this.registerDocumentRangeCommand('azALDevTools.removeVariable', 'removeVariable');

        //onsave command
        this.registerDocumentCommand('azALDevTools.fixDocumentOnSave', () => new OnDocumentSaveModifier(this._context));
        
        //editor and workspace commands
        this.registerModifierCommands('RemoveWithStatements', 'azALDevTools.RemoveEditorWithStatements', 'azALDevTools.RemoveProjectWithStatements', () => new WithModifier(this._context));
        this.registerModifierCommands('AddApplicationAreas', 'azALDevTools.AddEditorApplicationAreas', 'azALDevTools.AddProjectApplicationAreas', () => new AppAreasModifier(this._context));
        this.registerModifierCommands('AddToolTips', 'azALDevTools.AddEditorToolTip', 'azALDevTools.AddProjectToolTip', () => new ToolTipModifier(this._context));
        this.registerModifierCommands('AddTableFieldCaptions', 'azALDevTools.AddEditorFieldCaption', 'azALDevTools.AddProjectFieldCaption', () => new FieldCaptionsModifier(this._context));
        this.registerModifierCommands('AddPageFieldCaptions', 'azALDevTools.AddEditorPageFieldCaption', 'azALDevTools.AddProjectPageFieldCaption', () => new PageControlsCaptionsModifier(this._context));
        this.registerModifierCommands('AddObjectCaptions', 'azALDevTools.AddEditorObjectCaption', 'azALDevTools.AddProjectObjectCaption', () => new ObjectCaptionsModifier(this._context));
        this.registerModifierCommands('FixKeywordsCase', 'azALDevTools.FixEditorKeywordsCase', 'azALDevTools.FixProjectKeywordsCase', () => new FixKeywordsCaseModifier(this._context));
        this.registerModifierCommands('FixIdentifiersCase', 'azALDevTools.FixEditorIdentifiersCase', 'azALDevTools.FixProjectIdentifiersCase', () => new FixIdentifiersCaseModifier(this._context));
        this.registerModifierCommands('ConvertObjectIdsToNames', 'azALDevTools.ConvertEditorObjectIdsToNames', 'azALDevTools.ConvertProjectObjectIdsToNames', () => new ConvertObjectIdsToNamesModifier(this._context));
        this.registerModifierCommands('AddMissingParentheses', 'azALDevTools.AddMissingEditorParentheses', 'azALDevTools.AddMissingProjectParentheses', () => new AddMissingParenthesesModifier(this._context));       
        this.registerModifierCommands('AddDataClassifications', 'azALDevTools.AddEditorDataClassification', 'azALDevTools.AddProjectDataClassification',() => new DataClassificationModifier(this._context));
        this.registerModifierCommands('RemoveUnusedVariables', 'azALDevTools.RemoveEditorUnusedVariables', 'azALDevTools.RemoveProjectUnusedVariables', () => new RemoveUnusedVariablesModifier(this._context));

        this.registerModifierCommands('SortPermissions', 'azALDevTools.SortEditorPermissions', 'azALDevTools.SortWorkspacePermissions', () => new SortPermissionsModifier(this._context));
        this.registerModifierCommands('SortPermissionSetList', 'azALDevTools.SortEditorPermissionSetList', 'azALDevTools.SortWorkspacePermissionSetList', () => new SortPermissionSetListModifier(this._context));
        this.registerModifierCommands('SortProcedures', 'azALDevTools.SortEditorProcedures', 'azALDevTools.SortWorkspaceProcedures', () => new SortProceduresModifier(this._context));
        this.registerModifierCommands('SortProperties', 'azALDevTools.SortEditorProperties', 'azALDevTools.SortWorkspaceProperties', () => new SortPropertiesModifier(this._context));
        this.registerModifierCommands('SortReportColumns', 'azALDevTools.SortEditorReportColumns', 'azALDevTools.SortWorkspaceReportColumns', () => new SortReportColumnsModifier(this._context));
        this.registerModifierCommands('SortTableFields', 'azALDevTools.SortEditorTableFields', 'azALDevTools.SortWorkspaceTableFields', () => new SortTableFieldsModifier(this._context));
        this.registerModifierCommands('SortVariables', 'azALDevTools.SortEditorVariables', 'azALDevTools.SortWorkspaceVariables', () => new SortVariablesModifier(this._context));

        this.registerModifierCommands(undefined, 'azALDevTools.PrettifyMyEditorCode', 'azALDevTools.PrettifyMyWorkspaceCode', () => new BatchSyntaxModifier(this._context));
    }

    protected registerModifierCommands(name: string | undefined, editorCmdName: string, workspaceCmdName: string, modifierFactory: () => SyntaxModifier) {
        if (name)
            this._syntaxFactories[name] = modifierFactory;
        this.registerEditorCommand(editorCmdName, modifierFactory);
        this.registerWorkspaceCommand(workspaceCmdName, modifierFactory);
    }

    protected registerEditorCommand(name: string, modifierFactory: () => SyntaxModifier) {
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                name,
                async (document) => {
                    let cmd = modifierFactory();
                    await cmd.RunForActiveEditor();
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

    protected registerWorkspaceCommand(name: string, modifierFactory: () => SyntaxModifier) {
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                name,
                async (document) => {
                    let cmd = modifierFactory();
                    await cmd.RunForWorkspace();
                }
            )
        );
    }

    protected registerDocumentRangeCommand(name: string, workspaceCommandName: string) {
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                name,
                async (document, range) => {
                    let cmd = new WorkspaceCommandSyntaxModifier(this._context, workspaceCommandName, workspaceCommandName);
                    await cmd.RunForDocument(document, range, false);
                }
            )
        );
    }

    getSyntaxModifier(name: string): SyntaxModifier|undefined {
        let factory = this._syntaxFactories[name];
        if (factory)
            return factory();
        return undefined;
    }

}