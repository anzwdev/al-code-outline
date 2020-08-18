import * as vscode from 'vscode';
import { ALSyntaxWriter } from "../../allanguage/alSyntaxWriter";
import { ALPageExtWizardData } from '../wizards/alPageExtWizardData';

export class ALPageExtSyntaxBuilder {
    constructor() {
    }

    buildFromPageExtWizardData(destUri: vscode.Uri | undefined, data: ALPageExtWizardData) : string {
        //generate file content
        let writer : ALSyntaxWriter = new ALSyntaxWriter(destUri);

        writer.writeStartExtensionObject("pageextension", data.objectId, data.objectName, data.basePage);

        writer.writeEndObject();

        return writer.toString();
    }
}