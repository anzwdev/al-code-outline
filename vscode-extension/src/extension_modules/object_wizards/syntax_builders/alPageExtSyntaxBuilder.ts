import * as vscode from 'vscode';
import { ALPageExtWizardData } from '../wizards/alPageExtWizardData';
import { ALSyntaxWriter } from '../../../al_syntax/alSyntaxWriter';

export class ALPageExtSyntaxBuilder {
    constructor() {
    }

    buildFromPageExtWizardData(destUri: vscode.Uri | undefined, data: ALPageExtWizardData) : string {
        //generate file content
        let writer : ALSyntaxWriter = new ALSyntaxWriter(destUri);

        writer.writeNamespace(data.objectNamespace);
        writer.writeUsings(data.objectUsings);

        writer.writeStartExtensionObject("pageextension", data.objectId, data.objectName, data.basePage?.name ?? "");

        writer.writeEndObject();

        return writer.toWizardGeneratedString();
    }
}