import * as vscode from 'vscode';
import { ALCodeunitWizardData } from "../wizards/alCodeunitWizardData";
import { ALSyntaxWriter } from "../../allanguage/alSyntaxWriter";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ToolsGetInterfaceMethodsListResponse } from '../../langserver/symbolsinformation/toolsGetInterfaceMethodsListResponse';
import { toolsGetInterfaceMethodsListRequest } from '../../langserver/symbolsinformation/toolsGetInterfaceMethodsListRequest';

export class ALCodeunitSyntaxBuilder {
    protected _toolsExtensionContext : DevToolsExtensionContext;

    constructor(toolsExtensionContext : DevToolsExtensionContext) {
        this._toolsExtensionContext = toolsExtensionContext;
    }

    async buildFromCodeunitWizardDataAsync(destUri: vscode.Uri | undefined, data : ALCodeunitWizardData) : Promise<string> {
        //generate file content
        let writer : ALSyntaxWriter = new ALSyntaxWriter(destUri);

        writer.writeStartCodeunit(data.objectId, data.objectName, data.interfaceName);

        //write properties
        if ((data.selectedTable) && (data.selectedTable.length > 0)) {
            writer.writeProperty("TableNo", writer.encodeName(data.selectedTable));

            writer.writeLine("");
            writer.writeLine("trigger OnRun()");
            writer.writeLine("begin");
            writer.writeLine("");
            writer.writeLine("end;")
        }

        writer.writeLine("");

        if ((data.interfaceName) && (data.interfaceName != '')) {
            let methodsResponse = await this._toolsExtensionContext.toolsLangServerClient.getInterfaceMethodsList(
                new toolsGetInterfaceMethodsListRequest(destUri?.fsPath, data.interfaceName));
             if ((methodsResponse) && (methodsResponse.symbols) && (methodsResponse.symbols.length > 0)) {
                for (let i=0; i<methodsResponse.symbols.length; i++) {
                    if (methodsResponse.symbols[i].header) {
                        writer.writeLine(methodsResponse.symbols[i].header!);
                        writer.writeLine("begin");
                        writer.writeLine("end;");
                        writer.writeLine("");
                    }
                }        
            }
        }

        //finish object
        writer.writeEndObject();
        
        return writer.toWizardGeneratedString();
    }


}