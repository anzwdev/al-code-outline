import { ALCodeunitWizardData } from "../wizards/alCodeunitWizardData";
import { ALSyntaxWriter } from "../../allanguage/alSyntaxWriter";

export class ALCodeunitSyntaxBuilder {
    constructor() {
    }

    buildFromCodeunitWizardData(data : ALCodeunitWizardData, methodHeaders: string[] | undefined) : string {
        //generate file content
        let writer : ALSyntaxWriter = new ALSyntaxWriter();

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

        if (methodHeaders) {
            for (let i=0; i<methodHeaders.length; i++) {
                writer.writeLine(methodHeaders[i].replace(/,/g, ";"));
                writer.writeLine("begin");
                writer.writeLine("end;");
                writer.writeLine("");
            }        
        }

        //finish object
        writer.writeEndObject();
        
        return writer.toString();
    }


}