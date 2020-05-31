import * as vscode from 'vscode';
import { ALInterfaceWizardData } from "../wizards/alInterfaceWizardData";
import { ALSyntaxWriter } from "../../allanguage/alSyntaxWriter";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";

export class ALInterfaceSyntaxBuilder {
    protected _toolsExtensionContext : DevToolsExtensionContext;

    constructor(toolsExtensionContext : DevToolsExtensionContext) {
        this._toolsExtensionContext = toolsExtensionContext;
    }

    async buildFromInterfaceWizardDataAsync(destUri: vscode.Uri | undefined, data : ALInterfaceWizardData) : Promise<string> {       
        //generate file content
        let writer : ALSyntaxWriter = new ALSyntaxWriter(destUri);

        writer.writeStartInterface(data.objectName);

        writer.writeLine("");

        if ((data.baseCodeunitName) && (data.baseCodeunitName != '')) {
            let methodHeaders: string[] | undefined = await this._toolsExtensionContext.alLangProxy.getObjectMethods(destUri,
                'codeunit', data.baseCodeunitName);

            if ((methodHeaders) && (methodHeaders.length > 0)) {
                for (let i=0; i<methodHeaders.length; i++) {
                    if (!methodHeaders[i].startsWith("procedure Run(")) { //skip codeunit.Run function
                        let method = methodHeaders[i].replace(/,/g, ";");
                        if (!method.endsWith(";"))
                            method = method + ';';
                        writer.writeLine(method);
                    }
                }
                writer.writeLine("");
            }
        }

        //finish object
        writer.writeEndObject();
        
        return writer.toString();
    }


}