import * as vscode from 'vscode';
import { ALInterfaceWizardData } from "../wizards/alInterfaceWizardData";
import { DevToolsExtensionContext } from '../../../devToolsExtensionContext';
import { ALSyntaxWriter } from '../../../al_syntax/alSyntaxWriter';

export class ALInterfaceSyntaxBuilder {
    protected _toolsExtensionContext : DevToolsExtensionContext;

    constructor(toolsExtensionContext : DevToolsExtensionContext) {
        this._toolsExtensionContext = toolsExtensionContext;
    }

    async buildFromInterfaceWizardDataAsync(destUri: vscode.Uri | undefined, data : ALInterfaceWizardData) : Promise<string> {       
        //generate file content
        let writer : ALSyntaxWriter = new ALSyntaxWriter(destUri);

        writer.writeNamespace(data.objectNamespace);
        writer.writeUsings(data.objectUsings);

        writer.writeStartInterface(data.objectName);

        writer.writeLine("");

        if ((data.baseCodeunit) && (data.baseCodeunit.name)) {
            let methodsList = await this._toolsExtensionContext.projectInformationService.getObjectMethods(destUri, data.baseCodeunit, false);

            if (methodsList) {
                for (let i=0; i<methodsList.length; i++) {
                    if (methodsList[i].header) {
                        writer.writeLine(methodsList[i].header! + ";");
                    }
                }        
                writer.writeLine("");
            }           
        }

        //finish object
        writer.writeEndObject();
        
        return writer.toWizardGeneratedString();
    }


}