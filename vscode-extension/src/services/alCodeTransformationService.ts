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
import { LockRemovedFieldsCaptionsModifier } from '../alsyntaxmodifiers/lockRemovedFieldsCaptionsModifier';
import { FormatDocumentModifier } from '../alsyntaxmodifiers/formatDocumentModifier';
import { TrimTrailingWhitespaceModifier } from '../alsyntaxmodifiers/trimTrailingWhitespaceModifier';
import { RemoveBeginEndModifier } from '../alsyntaxmodifiers/removeBeginEndModifier';
import { RefreshToolTipsModifier } from '../alsyntaxmodifiers/refreshToolTipsModifier';
import { ReuseSingleFieldToolTipModifier } from '../alsyntaxmodifiers/reuseSingleFieldToolTipModifier';
import { RemoveEmptyLinesModifier } from '../alsyntaxmodifiers/removeEmptyLinesModifier';
import { RemoveEmptySectionsModifier } from '../alsyntaxmodifiers/removeEmptySectionsModifier';
import { SortCustomizationsModifier } from '../alsyntaxmodifiers/sortCustomizationsModifier';
import { RemoveStrSubstNoFromErrorModifier } from '../alsyntaxmodifiers/removeStrSubstNoFromErrorModifier';
import { RemoveEmptyTriggersModifier } from '../alsyntaxmodifiers/removeEmptyTriggersModifier';
import { MakeFlowFieldsReadOnlyModifier } from '../alsyntaxmodifiers/makeFlowFieldsReadOnlyModifier';
import { RemoveRedundantAppAreasModifier } from '../alsyntaxmodifiers/removeRedundantAppAreasModifier';
import { EnumCaptionsModifier } from '../alsyntaxmodifiers/enumCaptionsModifier';
import { AddAllObjectsPermissionsModifier } from '../alsyntaxmodifiers/addAllObjectsPermissionsModifier';
import { AddTableDataCaptionFieldsModifier } from '../alsyntaxmodifiers/addTableDataCaptionFieldsModifier';
import { AddDropDownFieldGroupsModifier } from '../alsyntaxmodifiers/AddDropDownFieldGroupsModifier';
import { AddReferencedTablesPermissionsModifier } from '../alsyntaxmodifiers/addReferencedTablesPermissionsModifier';
import { GenerateCSVXmlPortHeadersModifier } from '../alsyntaxmodifiers/generateCSVXmlPortHeadersModifier';

export class ALCodeTransformationService extends DevToolsExtensionService {
    protected _syntaxFactories: ISyntaxModifierFactoriesCollection;

    constructor(context: DevToolsExtensionContext) {
        super(context);

        this._syntaxFactories = {};

        //document range commands
        this.registerDocumentRangeCommand('azALDevTools.sortVariables', () => new SortVariablesModifier(this._context));
        this.registerDocumentRangeCommand('azALDevTools.sortProcedures', () => new SortProceduresModifier(this._context));
        this.registerDocumentRangeCommand('azALDevTools.sortProperties', () => new SortPropertiesModifier(this._context));
        this.registerDocumentRangeCommand('azALDevTools.sortReportColumns', () => new SortReportColumnsModifier(this._context));
        this.registerDocumentRangeCommand('azALDevTools.sortTableFields', () => new SortTableFieldsModifier(this._context));
        this.registerDocumentRangeCommand('azALDevTools.sortPermissions', () => new SortPermissionsModifier(this._context));
        this.registerDocumentRangeCommand('azALDevTools.sortPermissionSetList', () => new SortPermissionSetListModifier(this._context));
        this.registerDocumentRangeCommand('azALDevTools.sortCustomizations', () => new SortCustomizationsModifier(this._context));
        this.registerDocumentRangeCommand('azALDevTools.addAllObjectsPermissions', () => new AddAllObjectsPermissionsModifier(this._context));
        this.registerDocumentRangeCommand('azALDevTools.addReferencedTablesPermissions', () => new AddReferencedTablesPermissionsModifier(this._context));
        this.registerDocumentRangeCommand('azALDevTools.generateCSVXmlPortHeaders', () => new GenerateCSVXmlPortHeadersModifier(this._context));
        

        this.registerDocumentRangeCommand('azALDevTools.removeVariable', () => new WorkspaceCommandSyntaxModifier(this._context, 'removeVariable', 'removeVariable'));
        this.registerDocumentSymbolCommand('azALDevTools.ReuseToolTipFromOtherPages', () => new ReuseSingleFieldToolTipModifier(this._context));

        //onsave command
        this.registerDocumentCommand('azALDevTools.fixDocumentOnSave', () => new OnDocumentSaveModifier(this._context));
        
        //editor and workspace commands
        this.registerModifierCommands('RemoveWithStatements', 'azALDevTools.RemoveEditorWithStatements', 'azALDevTools.RemoveProjectWithStatements', () => new WithModifier(this._context));
        this.registerModifierCommands('AddApplicationAreas', 'azALDevTools.AddEditorApplicationAreas', 'azALDevTools.AddProjectApplicationAreas', () => new AppAreasModifier(this._context));
        this.registerModifierCommands('AddToolTips', 'azALDevTools.AddEditorToolTip', 'azALDevTools.AddProjectToolTip', () => new ToolTipModifier(this._context));
        this.registerModifierCommands('RefreshToolTips', 'azALDevTools.RefreshEditorToolTips', 'azALDevTools.RefreshProjectToolTips', () => new RefreshToolTipsModifier(this._context));
        this.registerModifierCommands('AddTableFieldCaptions', 'azALDevTools.AddEditorFieldCaption', 'azALDevTools.AddProjectFieldCaption', () => new FieldCaptionsModifier(this._context));

        this.registerModifierCommands('AddTableDataCaptionFields', 'azALDevTools.AddEditorTableDataCaptionFields', 'azALDevTools.AddProjectTableDataCaptionFields', () => new AddTableDataCaptionFieldsModifier(this._context));
        this.registerModifierCommands('AddDropDownFieldGroups', 'azALDevTools.AddEditorDropDownFieldGroups', 'azALDevTools.AddProjectDropDownFieldGroups', () => new AddDropDownFieldGroupsModifier(this._context));

        this.registerModifierCommands('AddEnumValuesCaptions', 'azALDevTools.AddEditorEnumValuesCaption', 'azALDevTools.AddProjectEnumValuesCaption', () => new EnumCaptionsModifier(this._context));
        this.registerModifierCommands('LockRemovedFieldCaptions', 'azALDevTools.LockEditorRemovedFieldCaptions', 'azALDevTools.LockProjectRemovedFieldCaptions', () => new LockRemovedFieldsCaptionsModifier(this._context));
        this.registerModifierCommands('AddPageFieldCaptions', 'azALDevTools.AddEditorPageFieldCaption', 'azALDevTools.AddProjectPageFieldCaption', () => new PageControlsCaptionsModifier(this._context));
        this.registerModifierCommands('AddObjectCaptions', 'azALDevTools.AddEditorObjectCaption', 'azALDevTools.AddProjectObjectCaption', () => new ObjectCaptionsModifier(this._context));
        this.registerModifierCommands('FixKeywordsCase', 'azALDevTools.FixEditorKeywordsCase', 'azALDevTools.FixProjectKeywordsCase', () => new FixKeywordsCaseModifier(this._context));
        this.registerModifierCommands('FixIdentifiersCase', 'azALDevTools.FixEditorIdentifiersCase', 'azALDevTools.FixProjectIdentifiersCase', () => new FixIdentifiersCaseModifier(this._context));
        this.registerModifierCommands('ConvertObjectIdsToNames', 'azALDevTools.ConvertEditorObjectIdsToNames', 'azALDevTools.ConvertProjectObjectIdsToNames', () => new ConvertObjectIdsToNamesModifier(this._context));
        this.registerModifierCommands('AddMissingParentheses', 'azALDevTools.AddMissingEditorParentheses', 'azALDevTools.AddMissingProjectParentheses', () => new AddMissingParenthesesModifier(this._context));       
        this.registerModifierCommands('AddDataClassifications', 'azALDevTools.AddEditorDataClassification', 'azALDevTools.AddProjectDataClassification',() => new DataClassificationModifier(this._context));
        this.registerModifierCommands('MakeFlowFieldsReadOnly', 'azALDevTools.MakeEditorFlowFieldsReadOnly', 'azALDevTools.MakeProjectFlowFieldsReadOnly', () => new MakeFlowFieldsReadOnlyModifier(this._context));
        this.registerModifierCommands('RemoveUnusedVariables', 'azALDevTools.RemoveEditorUnusedVariables', 'azALDevTools.RemoveProjectUnusedVariables', () => new RemoveUnusedVariablesModifier(this._context));
        this.registerModifierCommands('RemoveBeginEnd', 'azALDevTools.RemoveEditorBeginEnd', 'azALDevTools.RemoveProjectBeginEnd', () => new RemoveBeginEndModifier(this._context));

        this.registerModifierCommands('RemoveEmptyLines', 'azALDevTools.RemoveEditorEmptyLines', 'azALDevTools.RemoveProjectEmptyLines', () => new RemoveEmptyLinesModifier(this._context));
        this.registerModifierCommands('RemoveEmptySections', 'azALDevTools.RemoveEditorEmptySections', 'azALDevTools.RemoveProjectEmptySections', () => new RemoveEmptySectionsModifier(this._context));
        this.registerModifierCommands('RemoveEmptyTriggers', 'azALDevTools.RemoveEditorEmptyTriggers', 'azALDevTools.RemoveProjectEmptyTriggers', () => new RemoveEmptyTriggersModifier(this._context));
        this.registerModifierCommands('RemoveStrSubstNoFromError', 'azALDevTools.RemoveEditorStrSubstNoFromError', 'azALDevTools.RemoveProjectStrSubstNoFromError', () => new RemoveStrSubstNoFromErrorModifier(this._context));
        this.registerModifierCommands('RemoveRedundantAppAreas', 'azALDevTools.RemoveEditorRedundantAppAreas', 'azALDevTools.RemoveProjectRedundantAppAreas', () => new RemoveRedundantAppAreasModifier(this._context));

        this.registerModifierCommands('SortPermissions', 'azALDevTools.SortEditorPermissions', 'azALDevTools.SortWorkspacePermissions', () => new SortPermissionsModifier(this._context));
        this.registerModifierCommands('SortPermissionSetList', 'azALDevTools.SortEditorPermissionSetList', 'azALDevTools.SortWorkspacePermissionSetList', () => new SortPermissionSetListModifier(this._context));
        this.registerModifierCommands('SortProcedures', 'azALDevTools.SortEditorProcedures', 'azALDevTools.SortWorkspaceProcedures', () => new SortProceduresModifier(this._context));
        this.registerModifierCommands('SortProperties', 'azALDevTools.SortEditorProperties', 'azALDevTools.SortWorkspaceProperties', () => new SortPropertiesModifier(this._context));
        this.registerModifierCommands('SortReportColumns', 'azALDevTools.SortEditorReportColumns', 'azALDevTools.SortWorkspaceReportColumns', () => new SortReportColumnsModifier(this._context));
        this.registerModifierCommands('SortTableFields', 'azALDevTools.SortEditorTableFields', 'azALDevTools.SortWorkspaceTableFields', () => new SortTableFieldsModifier(this._context));
        this.registerModifierCommands('SortVariables', 'azALDevTools.SortEditorVariables', 'azALDevTools.SortWorkspaceVariables', () => new SortVariablesModifier(this._context));
        this.registerModifierCommands('SortCustomizations', 'azALDevTools.SortEditorCustomizations', 'azALDevTools.SortWorkspaceCustomizations', () => new SortCustomizationsModifier(this._context));

        this.registerModifierCommands(undefined, 'azALDevTools.RunEditorCodeCleanup', 'azALDevTools.RunWorkspaceCodeCleanup', () => new BatchSyntaxModifier(this._context));
        this.registerModifiedFilesOnlyCommand('azALDevTools.RunModifiedFilesCodeCleanup', () => new BatchSyntaxModifier(this._context));

        //register code cleanup only modifiers
        this._syntaxFactories["FormatDocument"] = (() => new FormatDocumentModifier(this._context));
        this._syntaxFactories["TrimTrailingWhitespace"] = (() => new TrimTrailingWhitespaceModifier(this._context));
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
                    await cmd.runForActiveEditor();
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
                    await cmd.runForDocument(document, undefined, false);
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
                    cmd.modifiedFilesOnly = false;
                    await cmd.runForWorkspace();
                }
            )
        );
    }

    protected registerModifiedFilesOnlyCommand(name: string, modifierFactory: () => SyntaxModifier) {
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                name,
                async (document) => {
                    let cmd = modifierFactory();
                    cmd.modifiedFilesOnly = true;
                    await cmd.runForWorkspace();
                }
            )
        );
    }

    protected registerDocumentRangeCommand(name: string, modifierFactory: () => SyntaxModifier) { //workspaceCommandName: string) {
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                name,
                async (document, range) => {
                    //let cmd = new WorkspaceCommandSyntaxModifier(this._context, workspaceCommandName, workspaceCommandName);
                    let cmd = modifierFactory();
                    await cmd.runForDocument(document, range, false);
                }
            )
        );
    }

    protected registerDocumentSymbolCommand(name: string, modifierFactory: () => SyntaxModifier) {
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                name,
                async (document, symbol) => {
                    let cmd = modifierFactory();
                    await cmd.runForDocumentSymbol(document, symbol, false);
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