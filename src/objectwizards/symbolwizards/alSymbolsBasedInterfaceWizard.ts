import { ALSymbolsBasedWizard } from "./alSymbolsBasedWizard";
import { AZSymbolInformation } from "../../symbollibraries/azSymbolInformation";
import { FileBuilder } from "../fileBuilder";
import { AZSymbolKind } from "../../symbollibraries/azSymbolKind";
import { ALInterfaceSyntaxBuilder } from "../syntaxbuilders/alInterfaceSyntaxBuilder";
import { ALInterfaceWizardData } from "../wizards/alInterfaceWizardData";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";

export class ALSymbolsBasedInterfaceWizard extends ALSymbolsBasedWizard {
    constructor(context : DevToolsExtensionContext) {
        super(context);
    }

    //#region Wizards with UI

    async showWizard(sourceSymbols: AZSymbolInformation[]) {
        if (sourceSymbols.length == 1)
            await this.showInterfaceWizard(sourceSymbols[0]);
        else
            await this.showMultiInterfaceWizard(sourceSymbols);
    }

    async showMultiInterfaceWizard(sourceSymbols: AZSymbolInformation[]) {
        if (!FileBuilder.checkCrsExtensionFileNamePatternRequired())
            return;

        const objType : AZSymbolKind = AZSymbolKind.Interface;

        let relativeFileDir = await this.getRelativeFileDir(objType);

        for (let i = 0; i < sourceSymbols.length; i++) {
            let sourceSymbol = sourceSymbols[i];
            let objectName : string = this.getDefaultInterfaceName(sourceSymbol);

            await this.createAndShowNewInterface(sourceSymbol, objectName, relativeFileDir);
        }
    }

    async showInterfaceWizard(sourceSymbol : AZSymbolInformation) {
        if (!FileBuilder.checkCrsFileNamePatternRequired())
            return;
            
        const objType : AZSymbolKind = AZSymbolKind.Interface;

        let objectName = await this.getObjectName("Please enter a name for the new object.", this.getDefaultInterfaceName(sourceSymbol));        
        if (!objectName)
            return;

        let relativeFileDir = await this.getRelativeFileDir(objType);
        await this.createAndShowNewInterface(sourceSymbol, objectName, relativeFileDir);
    }

    private async createAndShowNewInterface(sourceSymbol: AZSymbolInformation, objectName: string, relativeFileDir: string | undefined) {
        let fileName : string = await FileBuilder.getPatternGeneratedFullObjectFileName(AZSymbolKind.Interface, 0, objectName);
        let sourceCode = await this.buildInterfaceAsync(sourceSymbol, objectName);        
        this.showNewDocument(sourceCode, fileName, relativeFileDir);
    }

    //#endregion
    
    //#region Interface builders

    async buildInterfaceAsync(sourceSymbol : AZSymbolInformation, objectName : string) : Promise<string> {
        let syntaxBuilder = new ALInterfaceSyntaxBuilder(this._toolsExtensionContext);
        let settings = new ALInterfaceWizardData();
        settings.objectName = objectName;
        settings.baseCodeunitName = sourceSymbol.name;

        return await syntaxBuilder.buildFromInterfaceWizardDataAsync(undefined, settings);
    }

    //#endregion

    //#region Helper Methods

    private getDefaultInterfaceName(sourceSymbol: AZSymbolInformation) : string {
        return 'I' + sourceSymbol.name.trim();
    }
    
    //#endregion

}