import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { TextRange } from '../symbollibraries/textRange';
import { ISyntaxModifierResult } from './iSntaxModifierResult';
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class RemoveRedundantAppAreasModifier extends WorkspaceCommandSyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "Remove Redundant App Areas", "removeRedundantAppAreas");
    }

    async runForWorkspaceWithoutUI(workspaceUri: vscode.Uri, forFiles: string[] | undefined): Promise<ISyntaxModifierResult | undefined> {
        if (!this.canRun(workspaceUri))
            return this.getCannotRunResult();
        return await super.runForWorkspaceWithoutUI(workspaceUri, forFiles);
    }

    async runForDocumentWithoutUI(text: string, workspaceUri: vscode.Uri, documentUri: vscode.Uri, range: TextRange | undefined): Promise<ISyntaxModifierResult | undefined> {
        if (!this.canRun(documentUri))
            return this.getCannotRunResult();
        return await super.runForDocumentWithoutUI(text, workspaceUri, documentUri, range);
    }

    private canRun(resourceUri: vscode.Uri) : boolean {
        return this._context.alLangProxy.supportsAppAreasInheritance(resourceUri);
    }

    private getCannotRunResult() : ISyntaxModifierResult {
        return {
            success: false,
            message: "Remove Redundant App Areas command can only be run for runtime 10.0 or greater",
            source: undefined
        };
    }

}