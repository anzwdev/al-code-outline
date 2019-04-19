import { AZSymbolInformation } from "../symbollibraries/azSymbolInformation";
import { AZSymbolsLibrary } from "../symbollibraries/azSymbolsLibrary";
import { AZSymbolKind } from "../symbollibraries/azSymbolKind";

export class ALAppFileViewerData {
    objectCollections: AZSymbolInformation[];

    constructor(library: AZSymbolsLibrary) {
        this.objectCollections = [];
        this.collectObjects(library);
    }

    private collectObjects(library: AZSymbolsLibrary) {
        if (library.objectListRootSymbol) {
            this.addObjectList(library.objectListRootSymbol.findFirstSymbolByKind(AZSymbolKind.TableObjectList));
            this.addObjectList(library.objectListRootSymbol.findFirstSymbolByKind(AZSymbolKind.PageObjectList));
            this.addObjectList(library.objectListRootSymbol.findFirstSymbolByKind(AZSymbolKind.ReportObjectList));
            this.addObjectList(library.objectListRootSymbol.findFirstSymbolByKind(AZSymbolKind.XmlPortObjectList));
            this.addObjectList(library.objectListRootSymbol.findFirstSymbolByKind(AZSymbolKind.QueryObjectList));
            this.addObjectList(library.objectListRootSymbol.findFirstSymbolByKind(AZSymbolKind.CodeunitObjectList));
            this.addObjectList(library.objectListRootSymbol.findFirstSymbolByKind(AZSymbolKind.ControlAddInObjectList));
            this.addObjectList(library.objectListRootSymbol.findFirstSymbolByKind(AZSymbolKind.TableExtensionObjectList));
            this.addObjectList(library.objectListRootSymbol.findFirstSymbolByKind(AZSymbolKind.PageExtensionObjectList));
            this.addObjectList(library.objectListRootSymbol.findFirstSymbolByKind(AZSymbolKind.ProfileObjectList));
            this.addObjectList(library.objectListRootSymbol.findFirstSymbolByKind(AZSymbolKind.PageCustomizationObjectList));
            this.addObjectList(library.objectListRootSymbol.findFirstSymbolByKind(AZSymbolKind.EnumObjectList));
            this.addObjectList(library.objectListRootSymbol.findFirstSymbolByKind(AZSymbolKind.DotNetPackageList));
        }

    }

    private addObjectList(symbol: AZSymbolInformation | undefined) {
        if (symbol)
            this.objectCollections.push(symbol);
    }

}