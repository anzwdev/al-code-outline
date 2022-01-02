import { execFileSync } from 'child_process';
import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { ToolsGetPageFieldAvailableToolTipsRequest } from '../langserver/symbolsinformation/toolsGetPageFieldAvailableToolTipsRequest';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class ReuseSingleFieldToolTipModifier extends WorkspaceCommandSyntaxModifier {
    private _toolTip: string | undefined;
    private _availableToolTips: string[] | undefined;

    constructor(context: DevToolsExtensionContext) {
        super(context, "Reuse Field ToolTip from other Pages", "setPageFieldToolTip");
        this._toolTip = undefined;
        this._availableToolTips = undefined;
    }

    async runForDocumentSymbol(document: vscode.TextDocument, symbol: AZSymbolInformation, withUI: boolean) {
        //process symbol and get details
        if (!symbol)
            return;

        if (symbol.kind != AZSymbolKind.PageField) {
            if (symbol.kind == AZSymbolKind.Property)
                if ((symbol.parent) && (symbol.parent.parent) && (symbol.parent.parent.kind == AZSymbolKind.PageField))
                    symbol = symbol.parent.parent;
        }

        if ((symbol.kind != AZSymbolKind.PageField) ||
            (!symbol.source) ||
            (symbol.source == ''))
            return;

        //find parent page or page extension
        let objectSymbol = symbol.findParentObject();
        if ((!objectSymbol) ||
            ((objectSymbol.kind != AZSymbolKind.PageObject) && (objectSymbol.kind != AZSymbolKind.PageExtensionObject)))
            return;

        let objectType = (objectSymbol.kind == AZSymbolKind.PageExtensionObject)?'PageExtension':'Page';
        let sourceTable: string = '';
        if ((objectSymbol.kind == AZSymbolKind.PageObject) && (objectSymbol.source))
            sourceTable = objectSymbol.source;

        //download list of available tooltips
        let response = await this._context.toolsLangServerClient.getPageFieldAvailableToolTips(
            new ToolsGetPageFieldAvailableToolTipsRequest(document.uri.fsPath, objectType, objectSymbol.name, sourceTable, symbol.source));

        if ((!response) || (!response.toolTips) || (response.toolTips.length == 0)) {
            vscode.window.showInformationMessage('Cannot find any tooltips for this field');
            return;
        }

        this._availableToolTips = response.toolTips;

        await this.runForDocument(document, symbol.range, withUI);

        this._availableToolTips = undefined;
        this._toolTip = undefined;
    }

    protected getParameters(uri: vscode.Uri): any {
        let parameters = super.getParameters(uri);
        parameters.toolTip = this._toolTip;
        return parameters;
    }

    async askForParameters(uri: vscode.Uri | undefined): Promise<boolean> {
        //ask for Application Area Type
        this._toolTip = await vscode.window.showQuickPick(this._availableToolTips!, {
            canPickMany: false,
            placeHolder: 'Select tooltip fot this field'
        });    

        return ((!!this._toolTip) && (this._toolTip != ''));
    }


}