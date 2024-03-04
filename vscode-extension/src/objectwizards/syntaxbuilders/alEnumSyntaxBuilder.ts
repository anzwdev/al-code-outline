import * as vscode from 'vscode';
import { ALSyntaxWriter } from "../../allanguage/alSyntaxWriter";
import { ALEnumWizardData } from "../wizards/alEnumWizardData";
import { ALSyntaxHelper } from '../../allanguage/alSyntaxHelper';

export class ALEnumSyntaxBuilder {
    
    constructor() {

    }

    buildFromEnumWizardData(destUri: vscode.Uri | undefined, data : ALEnumWizardData) : string {
        //generate file content
        let writer : ALSyntaxWriter = new ALSyntaxWriter(destUri);

        writer.writeNamespace(data.objectNamespace);
        writer.writeUsings(data.objectUsings);

        writer.writeStartObject("enum", data.objectId, data.objectName);
        if (data.extensible) {
            writer.writeProperty("Extensible", "true");
        }
        writer.writeLine("");

        if ((data.valueList) && (data.valueList !== "")) {
            let values : string[] = ALSyntaxHelper.splitNamesList(data.valueList);
            let captions : string[] = ALSyntaxHelper.splitNamesList(data.captionList);

            if ((values) && (values.length > 0)) {
                for (let i = 0; i < values.length; i++) {
                    writer.writeStartNameSourceBlock("value", i.toString(), writer.encodeName(values[i]));

                    if (captions.length > i) {
                        writer.writeProperty("Caption", writer.encodeString(captions[i]));
                    } else {
                        writer.writeProperty("Caption", writer.encodeString(values[i]));
                    }

                    writer.writeEndBlock();
                }
            }
        }

        writer.writeEndObject();
        
        return writer.toWizardGeneratedString();
    }

}