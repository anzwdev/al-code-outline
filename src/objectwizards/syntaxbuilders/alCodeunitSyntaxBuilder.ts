import { ALCodeunitWizardData } from "../wizards/alCodeunitWizardData";
import { ALSyntaxWriter } from "../../allanguage/alSyntaxWriter";

export class ALCodeunitSyntaxBuilder {
    constructor() {
    }

    buildFromCodeunitWizardData(data : ALCodeunitWizardData) : string {
        //generate file content
        let writer : ALSyntaxWriter = new ALSyntaxWriter();

        writer.writeStartObject("codeunit", data.objectId, data.objectName);

        //write properties
        if ((data.selectedTable) && (data.selectedTable.length > 0))
            writer.writeProperty("TableNo", writer.encodeName(data.selectedTable));

        writer.writeLine("");

        //write interface procedures


        //finish object
        writer.writeLine("");
        writer.writeEndObject();
        
        return writer.toString();
    }


}