import * as vscode from 'vscode';
import { ALCodeunitWizardData } from "../wizards/alCodeunitWizardData";
import { DevToolsExtensionContext } from '../../../devToolsExtensionContext';
import { ALSyntaxWriter } from '../../../al_syntax/alSyntaxWriter';

export class ALCodeunitSyntaxBuilder {
    protected _toolsExtensionContext : DevToolsExtensionContext;

    constructor(toolsExtensionContext : DevToolsExtensionContext) {
        this._toolsExtensionContext = toolsExtensionContext;
    }

    async buildFromCodeunitWizardDataAsync(destUri: vscode.Uri | undefined, data : ALCodeunitWizardData) : Promise<string> {
        //generate file content
        let writer : ALSyntaxWriter = new ALSyntaxWriter(destUri);

        writer.writeNamespace(data.objectNamespace);
        writer.writeUsings(data.objectUsings);

        writer.writeStartCodeunit(data.objectId, data.objectName, data.interface?.name);

        //write properties
        if ((data.selectedTable) && (data.selectedTable)) {
            writer.writeProperty("TableNo", writer.encodeName(data.selectedTable.name));

            writer.writeLine("");
            writer.writeLine("trigger OnRun()");
            writer.writeLine("begin");
            writer.writeLine("");
            writer.writeLine("end;");
        }

        writer.writeLine("");

        if ((data.interface) && (data.interface.name !== "")) {
            let methodsList = await this._toolsExtensionContext.projectInformationService.getObjectMethods(destUri, data.interface, false);
             if (methodsList) {
                for (let i=0; i<methodsList.length; i++) {
                    if (methodsList[i].header) {
                        writer.writeLine(methodsList[i].header!);
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