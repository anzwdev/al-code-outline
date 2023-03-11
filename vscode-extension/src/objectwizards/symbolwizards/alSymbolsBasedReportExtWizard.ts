import * as vscode from 'vscode';
import { FileBuilder } from '../fileBuilder';
import { AZSymbolInformation } from '../../symbollibraries/azSymbolInformation';
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';
import { ALSymbolsBasedWizard } from './alSymbolsBasedWizard';
import { ALSyntaxWriter } from '../../allanguage/alSyntaxWriter';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';

export class ALSymbolsBasedReportExtWizard extends ALSymbolsBasedWizard {

    constructor(context: DevToolsExtensionContext) {
        super(context);
    }

    //#region Wizards with UI

    async showWizard(symbols: AZSymbolInformation[]) {
        if (symbols.length == 1)
            await this.showReportExtWizard(symbols[0]);
        else
            await this.showMultiReportExtWizard(symbols);
    }

    async showMultiReportExtWizard(reportSymbols: AZSymbolInformation[]) {
        if (!FileBuilder.checkCrsExtensionFileNamePatternRequired() || !FileBuilder.checkCrsExtensionObjectNamePatternRequired(true))
            return;

        const extObjType : AZSymbolKind = AZSymbolKind.ReportExtensionObject;
        let relativeFileDir: string | undefined = await this.getRelativeFileDir(extObjType);

        let startObjectId: number = await this.getObjectId(relativeFileDir, "reportextension", "Please enter a starting ID for the report extensions.", 0);
        if (startObjectId < 0) {
            return;
        }

        for (let i = 0; i < reportSymbols.length; i++) {
            let reportSymbol = reportSymbols[i];
            let extObjectId: number = startObjectId + i;
            let extObjectName: string = await FileBuilder.getPatternGeneratedExtensionObjectName(extObjType, extObjectId, reportSymbol);

            await this.createAndShowNewReportExtension(reportSymbol, extObjType, extObjectId, extObjectName, relativeFileDir);
        }
    }

    async showReportExtWizard(reportSymbol : AZSymbolInformation) {
        if (!FileBuilder.checkCrsExtensionFileNamePatternRequired() || !FileBuilder.checkCrsExtensionObjectNamePatternRequired(false))
            return;

        const extObjType : AZSymbolKind = AZSymbolKind.ReportExtensionObject;
        let relativeFileDir = await this.getRelativeFileDir(extObjType);
        
        let extObjectId : number = await this.getObjectId(relativeFileDir, "reportextension", "Please enter an ID for the report extension.", 0);
        if (extObjectId < 0) {
            return;
        }

        let extObjectName: string | undefined = await FileBuilder.getPatternGeneratedExtensionObjectName(extObjType, extObjectId, reportSymbol);
        if (!extObjectName)
            extObjectName = reportSymbol.name + ' Extension';
        extObjectName = await this.getObjectName("Please enter a name for the report extension.", extObjectName);
        if (!extObjectName) {
            return;
        }

        await this.createAndShowNewReportExtension(reportSymbol, extObjType, extObjectId, extObjectName, relativeFileDir);
    }

    private async createAndShowNewReportExtension(reportSymbol: AZSymbolInformation, extObjType: AZSymbolKind, extObjectId: number, extObjectName: string, relativeFileDir: string| undefined) {
        let fileName : string = await FileBuilder.getPatternGeneratedExtensionObjectFileName(extObjType, extObjectId, extObjectName, reportSymbol);
        this.showNewDocument(this.buildReportExtForReport(reportSymbol, extObjectId, extObjectName), fileName, relativeFileDir);
    }

    //#endregion

    //#region Report Extension builders

    private buildReportExtForReport(reportSymbol : AZSymbolInformation, objectId : number, extObjectName : string) : string {
        
        let writer : ALSyntaxWriter = new ALSyntaxWriter(undefined);

        writer.writeStartExtensionObject("reportextension", objectId.toString(), extObjectName, reportSymbol.name);
        
        writer.writeStartDataset();

        writer.writeLine("");
        
        writer.writeEndDataset();
        
        writer.writeLine("");

        writer.writeStartRequestPage();

        writer.writeLine("");

        writer.writeEndRequestPage();

        writer.writeEndObject();
        
        return writer.toWizardGeneratedString();
    }

    //#endregion
}