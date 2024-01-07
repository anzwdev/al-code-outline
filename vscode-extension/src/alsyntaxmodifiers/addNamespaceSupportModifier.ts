import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class AddNamespaceSupportModifier extends WorkspaceCommandSyntaxModifier {
   
    private _rootNamespaceName: string | undefined;

    constructor(context: DevToolsExtensionContext) {
        super(context, "Add namespaces support", "enableNamespacesSupport");
    }

    protected getParameters(uri: vscode.Uri): any {
        let parameters = super.getParameters(uri);

        let settings = vscode.workspace.getConfiguration('alOutline', uri);                        
        parameters.useFolderStructure = true;
        parameters.rootNamespace = this._rootNamespaceName;
        
        return parameters;
    }

    async askForParameters(uri: vscode.Uri | undefined): Promise<boolean> {
        let namespaceName = await vscode.window.showInputBox(
            {
                prompt: "Enter root namespace name",                
                value: this._rootNamespaceName                                
            });
        if (namespaceName !== undefined)
        {
            this._rootNamespaceName = namespaceName;
            return true;
        }
        return false;
    }

}