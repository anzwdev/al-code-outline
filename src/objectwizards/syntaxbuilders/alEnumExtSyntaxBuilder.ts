import * as vscode from 'vscode';
import { ALEnumExtWizardData } from "../wizards/alEnumExtWizardData";
import { ALSyntaxWriter } from "../../allanguage/alSyntaxWriter";

export class ALEnumExtSyntaxBuilder {
    
    constructor() {
    }

    buildFromEnumExtWizardData(destUri: vscode.Uri | undefined, data : ALEnumExtWizardData) : string {
        //generate file content
        let writer : ALSyntaxWriter = new ALSyntaxWriter(destUri);

        writer.writeStartExtensionObject("enumextension", data.objectId, data.objectName, data.baseEnum);
        writer.writeLine("");

        if ((data.valueList) && (data.valueList != "")) {
            let values : string[] = data.valueList.split(",");
            let captions : string[];
            if ((data.captionList) && (data.captionList != ""))
                captions = data.captionList.split(",");
            else
                captions = [];
            let valueId = data.firstValueId;
            if ((values) && (values.length > 0)) {
                for (let i = 0; i < values.length; i++) {
                    writer.writeStartNameSourceBlock("value", valueId.toString(), writer.encodeName(values[i]));
                    if (captions.length > i)
                        writer.writeProperty("Caption", writer.encodeString(captions[i]));
                    else
                        writer.writeProperty("Caption", writer.encodeString(values[i]));
                    writer.writeEndBlock();
                    valueId++;
                }
            }
        }

        writer.writeLine("");
        writer.writeEndObject();
        
        return writer.toString();
    }

} 