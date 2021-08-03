import * as vscode from 'vscode';
import { ALSyntaxWriter } from "../../allanguage/alSyntaxWriter";
import { ALReportExtWizardData } from '../wizards/alReportExtWizardData';

export class ALReportExtSyntaxBuilder {
    constructor() {
    }

    buildFromReportExtWizardData(destUri: vscode.Uri | undefined, data: ALReportExtWizardData) : string {
        //generate file content
        let writer : ALSyntaxWriter = new ALSyntaxWriter(destUri);

        writer.writeStartExtensionObject("reportextension", data.objectId, data.objectName, data.baseReport);

        writer.writeEndObject();

        return writer.toString();
    }
}