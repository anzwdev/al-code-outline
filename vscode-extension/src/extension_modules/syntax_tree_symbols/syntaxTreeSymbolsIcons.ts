import * as vscode from 'vscode';
import * as path from 'path';
import { LSSyntaxNodeKind } from '../../langserver/common_types/lsSyntaxNodeKind';
import { LSSyntaxNodeAccessModifier } from '../../langserver/common_types/lsSyntaxNodeAccessModifier';
import { LSSyntaxNodeKindHelper } from '../../langserver/common_types/lsSyntaxNodeKindHelper';

export class SyntaxTreeSymbolsIcons {

    public static getIconPath(context: vscode.ExtensionContext, kind: LSSyntaxNodeKind, access: LSSyntaxNodeAccessModifier | undefined, subtype: string | undefined) : vscode.IconPath {
        let name = "tree-" + LSSyntaxNodeKindHelper.getIconName(kind, access, subtype) + ".svg";
        return {
            light: vscode.Uri.file(context.asAbsolutePath(path.join("resources", "images", "light", name))),
            dark: vscode.Uri.file(context.asAbsolutePath(path.join("resources", "images", "dark", name)))
        };
    }    

}