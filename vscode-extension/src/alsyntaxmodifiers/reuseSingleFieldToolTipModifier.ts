import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { ToolsGetPageFieldAvailableToolTipsRequest } from '../langserver/symbolsinformation/toolsGetPageFieldAvailableToolTipsRequest';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";
import { LabelInformation } from '../symbolsinformation/labelInformation';
import { TemplateQuickPickItem } from '../tools/templateQuickPickItem';

export class ReuseSingleFieldToolTipModifier extends WorkspaceCommandSyntaxModifier {
    private _toolTip: LabelInformation | undefined;
    private _availableToolTips: LabelInformation[] | undefined;

    constructor(context: DevToolsExtensionContext) {
        super(context, "Reuse Field ToolTip from other Pages", "setPageFieldToolTip");
        this._toolTip = undefined;
        this._availableToolTips = undefined;
    }

    async runForDocumentSymbol(document: vscode.TextDocument, symbol: AZSymbolInformation, withUI: boolean) {
        //process symbol and get details
        if (!symbol) {
            return;
        }

        if (symbol.kind !== AZSymbolKind.PageField) {
            if (symbol.kind === AZSymbolKind.Property) {
                if ((symbol.parent) && (symbol.parent.parent) && (symbol.parent.parent.kind === AZSymbolKind.PageField)) {
                    symbol = symbol.parent.parent;
                }
            }
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
            new ToolsGetPageFieldAvailableToolTipsRequest(document.uri.fsPath, objectType, {
                    id: objectSymbol.id,
                    name: objectSymbol.name,
                    namespaceName: objectSymbol.namespaceName
                }, {
                    usings: objectSymbol.usings,
                    nameWithNamespaceOrId: sourceTable
                },
                symbol.source));

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
        if (this._toolTip) {
            parameters.toolTip = this._toolTip.value;
            parameters.comment = this._toolTip.comment;
        }
        return parameters;
    }

    async askForParameters(uri: vscode.Uri | undefined): Promise<boolean> {
        //ask for Application Area Type
        let quickPickItems = this._availableToolTips!.map(x => new TemplateQuickPickItem<LabelInformation>(x.value!, x, false));
        let selectedToolTip : TemplateQuickPickItem<LabelInformation> | undefined = await vscode.window.showQuickPick(quickPickItems, {
            canPickMany: false,
            placeHolder: 'Select tooltip for this field'
        });    

        this._toolTip = selectedToolTip?.value;

        return ((!!this._toolTip) && (this._toolTip.value !== ''));
    }

}