import * as vscode from 'vscode';
import { ALEnumExtWizardData } from "../wizards/alEnumExtWizardData";
import { ALSyntaxWriter } from "../../allanguage/alSyntaxWriter";
import { ALSyntaxHelper } from '../../allanguage/alSyntaxHelper';

export class ALEnumExtSyntaxBuilder {
    
    constructor() {
    }

    buildFromEnumExtWizardData(destUri: vscode.Uri | undefined, data : ALEnumExtWizardData) : string {
        //generate file content
        let writer : ALSyntaxWriter = new ALSyntaxWriter(destUri);

        writer.writeNamespace(data.objectNamespace);
        writer.writeUsings(data.objectUsings);

        writer.writeStartExtensionObject("enumextension", data.objectId, data.objectName, data.baseEnum);
        writer.writeLine("");

        if ((data.valueList) && (data.valueList != "")) {
            let values : string[] = ALSyntaxHelper.splitNamesList(data.valueList);
            let captions : string[] = ALSyntaxHelper.splitNamesList(data.captionList);
            let valueId = data.firstValueId;
            if ((values) && (values.length > 0)) {
                for (let i = 0; i < values.length; i++) {
                    writer.writeStartNameSourceBlock("value", valueId.toString(), writer.encodeName(values[i]));
                    if (captions.length > i) {
                        writer.writeProperty("Caption", writer.encodeString(captions[i]));
                    } else {
                        writer.writeProperty("Caption", writer.encodeString(values[i]));
                    }
                    writer.writeEndBlock();
                    valueId++;
                }
            }
        }

        writer.writeEndObject();
        
        return writer.toWizardGeneratedString();
    }

} 