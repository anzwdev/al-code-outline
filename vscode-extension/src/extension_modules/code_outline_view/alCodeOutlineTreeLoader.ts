import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { CodeOutlineTreeDocumentState } from "./codeOutlineTreeDocumentState";
import { CodeOutlineTreeItem } from "./codeOutlineTreeItem";
import { CodeOutlineTreeLoader } from "./codeOutlineTreeLoader";
import { LSSTSyntaxTreeSymbolsTreeViewNode } from '../../langserver/syntaxtreesymbols/lsstsSyntaxTreeSymbolsTreeViewNode';
import { LSTextRangeToVSCRangeConverter } from '../../langserver/common_types/lsTextRangeToVSCRangeConverter';
import { SyntaxTreeSymbolsIcons } from '../syntax_tree_symbols/syntaxTreeSymbolsIcons';
import { LSSyntaxNodeKind } from '../../langserver/common_types/lsSyntaxNodeKind';
import { LSSyntaxNodeAccessModifier } from '../../langserver/common_types/lsSyntaxNodeAccessModifier';

export class ALCodeOutlineTreeLoader implements CodeOutlineTreeLoader {

    public getTreeId(): string | undefined {
        return vscode.window.activeTextEditor?.document.uri.toString();
    }

    public  async loadTree(context: DevToolsExtensionContext, state: CodeOutlineTreeDocumentState): Promise<CodeOutlineTreeItem | undefined> {
        let symbols = await context.syntaxTreeSymbolsService.getActiveDocumentContent();
        if (symbols) {
            return this.createTreeItem(context.vscodeExtensionContext, symbols, undefined, state);
        }
        return undefined;
    }

    private createTreeItem(context: vscode.ExtensionContext, symbol: LSSTSyntaxTreeSymbolsTreeViewNode, parent: CodeOutlineTreeItem | undefined, state: CodeOutlineTreeDocumentState) : CodeOutlineTreeItem {
        let range = LSTextRangeToVSCRangeConverter.convert(symbol.range);
        let selectionRange = LSTextRangeToVSCRangeConverter.convert(symbol.selectionRange);
        let iconPath = SyntaxTreeSymbolsIcons.getIconPath(context, symbol.kind ?? LSSyntaxNodeKind.Undefined, symbol.access ?? LSSyntaxNodeAccessModifier.Public, symbol.subtype);
        let treeNode = new CodeOutlineTreeItem(symbol.uid, symbol.fullName ?? symbol.name, symbol.kind, range, selectionRange, iconPath, parent, state);

        let childNodes: CodeOutlineTreeItem[] | undefined = undefined;
        if ((symbol.childSymbols) && (symbol.childSymbols.length > 0)) {
            childNodes = [];

            for (let i=0; i < symbol.childSymbols.length; i++) {
                let childNode = this.createTreeItem(context, symbol.childSymbols[i], treeNode, state);
                childNodes.push(childNode);
            }

            treeNode.updateChildNodes(childNodes, state);
        }

        return treeNode;
    }

}