import * as vscode from 'vscode';
import { ALInterfaceWizardData } from "../wizards/alInterfaceWizardData";
import { ALSyntaxWriter } from "../../allanguage/alSyntaxWriter";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { toolsGetCodeunitMethodsListRequest } from '../../langserver/symbolsinformation/toolsGetCodeunitMethodsListRequest';

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
            let methodsResponse = await this._toolsExtensionContext.toolsLangServerClient.getCodeunitMethodsList(
                new toolsGetCodeunitMethodsListRequest(destUri?.fsPath, data.baseCodeunitName));

            if ((methodsResponse) && (methodsResponse.symbols) && (methodsResponse.symbols.length > 0)) {
                for (let i=0; i<methodsResponse.symbols.length; i++) {
                    if ((methodsResponse.symbols[i].header) && ((!methodsResponse.symbols[i].accessModifier) || (methodsResponse.symbols[i].accessModifier == "")))
                        writer.writeLine(methodsResponse.symbols[i].header! + ";");
                }        
                writer.writeLine("");
            }           
        }

        //finish object
        writer.writeEndObject();
        
        return writer.toWizardGeneratedString();
    }


}