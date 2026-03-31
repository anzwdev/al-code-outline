import * as vscode from 'vscode';
import { ALReportExtWizardData } from '../wizards/alReportExtWizardData';
import { ALSyntaxWriter } from '../../../al_syntax/alSyntaxWriter';
import { ALSyntaxHelper } from '../../../al_syntax/alSyntaxHelper';

export class ALReportExtSyntaxBuilder {
    constructor() {
    }

    buildFromReportExtWizardData(destUri: vscode.Uri | undefined, data: ALReportExtWizardData) : string {
        let settings =  vscode.workspace.getConfiguration('alOutline', destUri);
        let addDataItemName = !!settings.get<boolean>('addDataItemToReportColumnName');

        //generate file content
        let writer : ALSyntaxWriter = new ALSyntaxWriter(destUri);

        writer.writeNamespace(data.objectNamespace);
        writer.writeUsings(data.objectUsings);

        writer.writeStartExtensionObject("reportextension", data.objectId, data.objectName, data.baseReport?.name ?? "");

        writer.writeStartDataset();

        if (data.dataItems) {
            for (let diidx=0; diidx<data.dataItems.length; diidx++) {
                if (data.dataItems[diidx].fields.length > 0) {                    
                    writer.writeStartAdd(ALSyntaxHelper.toNameText(data.dataItems[diidx].name));
                    let fields = data.dataItems[diidx].fields;
                    for (let fldidx = 0; fldidx < fields.length; fldidx++) {
                        writer.writeReportColumn(data.dataItems[diidx].name, fields[fldidx].name!, addDataItemName);
                    }
                    writer.writeEndBlock();
                }
            }
        }

        writer.writeEndDataset();

        writer.writeEndObject();

        return writer.toWizardGeneratedString();
    }
}