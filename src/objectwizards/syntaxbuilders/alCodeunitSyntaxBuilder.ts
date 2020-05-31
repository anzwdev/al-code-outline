import * as vscode from 'vscode';
import { ALCodeunitWizardData } from "../wizards/alCodeunitWizardData";
import { ALSyntaxWriter } from "../../allanguage/alSyntaxWriter";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";

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
            let methodHeaders: string[] | undefined = await this._toolsExtensionContext.alLangProxy.getObjectMethods(destUri,
                'Interface', data.interfaceName);

            if ((methodHeaders) && (methodHeaders.length > 0)) {
                for (let i=0; i<methodHeaders.length; i++) {
                    writer.writeLine(methodHeaders[i].replace(/,/g, ";"));
                    writer.writeLine("begin");
                    writer.writeLine("end;");
                    writer.writeLine("");
                }        
            }
        }

        //finish object
        writer.writeEndObject();
        
        return writer.toString();
    }


}