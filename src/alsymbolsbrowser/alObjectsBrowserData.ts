import { AZSymbolInformation } from "../symbollibraries/azSymbolInformation";
import { AZSymbolKind } from "../symbollibraries/azSymbolKind";
import { AZSymbolsLibrary } from "../symbollibraries/azSymbolsLibrary";
import { ALObjectBrowserSymbolInfo } from "./alObjectBrowserSymbolInfo";

export class ALObjectsBrowserData {
    name : string;
    showLibrary: boolean;
    objectCollections: ALObjectBrowserSymbolInfo[];

    constructor(library: AZSymbolsLibrary) {
        this.objectCollections = [];
        this.name = library.displayName;
        this.showLibrary = false;
        this.collectObjects(library);
    }

    private collectObjects(library: AZSymbolsLibrary) {
        if (library.rootSymbol) {
            this.addObjectList(library, AZSymbolKind.TableObjectList);
            this.addObjectList(library, AZSymbolKind.PageObjectList);
            this.addObjectList(library, AZSymbolKind.ReportObjectList);
            this.addObjectList(library, AZSymbolKind.XmlPortObjectList);
            this.addObjectList(library, AZSymbolKind.QueryObjectList);
            this.addObjectList(library, AZSymbolKind.CodeunitObjectList);
            this.addObjectList(library, AZSymbolKind.ControlAddInObjectList);
            this.addObjectList(library, AZSymbolKind.TableExtensionObjectList);
            this.addObjectList(library, AZSymbolKind.PageExtensionObjectList);
            this.addObjectList(library, AZSymbolKind.ProfileObjectList);
            this.addObjectList(library, AZSymbolKind.PageCustomizationObjectList);
            this.addObjectList(library, AZSymbolKind.EnumObjectList);
            this.addObjectList(library, AZSymbolKind.DotNetPackageList);
        }
    }

    private addObjectList(library: AZSymbolsLibrary, kind: AZSymbolKind) {
        let listSymbolInfo : ALObjectBrowserSymbolInfo | undefined = this.findObjects(undefined,
            library.rootSymbol, kind);
        if (listSymbolInfo)
            this.objectCollections.push(listSymbolInfo);
    }

    private findObjects(listSymbolInfo : ALObjectBrowserSymbolInfo | undefined, parent: AZSymbolInformation, listKind: AZSymbolKind) : ALObjectBrowserSymbolInfo | undefined {
        if ((parent) && (parent.childSymbols)) {
            for (let i=0; i<parent.childSymbols.length; i++) {
                if (parent.childSymbols[i].kind == listKind)
                    listSymbolInfo = this.addChildSymbols(listSymbolInfo, parent.childSymbols[i], parent.name);    
                else
                    listSymbolInfo = this.findObjects(listSymbolInfo, parent.childSymbols[i], listKind);
            }
        }
        return listSymbolInfo;
    }

    private addChildSymbols(listSymbolInfo : ALObjectBrowserSymbolInfo | undefined, symbol: AZSymbolInformation, libraryName: string) : ALObjectBrowserSymbolInfo | undefined {
        if ((symbol.childSymbols) && (symbol.childSymbols.length > 0)) {
            if (listSymbolInfo) //group node already exists which means that we have more than 1 package in the symbols library
                this.showLibrary = true;
            else
                listSymbolInfo = new ALObjectBrowserSymbolInfo(symbol, '');            
            for (let i=0; i<symbol.childSymbols.length; i++) {
                listSymbolInfo.addChildSymbol(new ALObjectBrowserSymbolInfo(symbol.childSymbols[i], libraryName));
            }
        }
        return listSymbolInfo;
    }
}