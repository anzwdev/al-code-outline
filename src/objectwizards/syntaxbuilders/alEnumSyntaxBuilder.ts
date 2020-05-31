import * as vscode from 'vscode';
import { ALSyntaxWriter } from "../../allanguage/alSyntaxWriter";
import { ALEnumWizardData } from "../wizards/alEnumWizardData";

export class ALEnumSyntaxBuilder {
    
    constructor() {

    }

    buildFromEnumWizardData(destUri: vscode.Uri | undefined, data : ALEnumWizardData) : string {
        //generate file content
        let writer : ALSyntaxWriter = new ALSyntaxWriter(destUri);

        writer.writeStartObject("enum", data.objectId, data.objectName);
        if (data.extensible)
            writer.writeProperty("Extensible", "true");
        writer.writeLine("");

        if ((data.valueList) && (data.valueList != "")) {
            let values : string[] = data.valueList.split(",");
            let captions : string[];
            if ((data.captionList) && (data.captionList != ""))
                captions = data.captionList.split(",");
            else
                captions = [];

            if ((values) && (values.length > 0)) {
                for (let i = 0; i < values.length; i++) {
                    writer.writeStartNameSourceBlock("value", i.toString(), writer.encodeName(values[i]));

                    if (captions.length > i)
                        writer.writeProperty("Caption", writer.encodeString(captions[i]));
                    else
                        writer.writeProperty("Caption", writer.encodeString(values[i]));

                    writer.writeEndBlock();
                }
            }
        }

        writer.writeLine("");
        writer.writeEndObject();
        
        return writer.toString();
    }

} 