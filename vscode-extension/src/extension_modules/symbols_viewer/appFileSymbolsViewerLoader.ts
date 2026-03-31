import { LSSymbolsViewerClient } from "../../langserver/symbols_viewer/lsSymbolsViewerClient";
import { SymbolsViewerDocument } from "./symbolsViewerDocument";
import { SymbolsViewerLoader } from "./symbolsViewerLoader";

export class AppFileSymbolsViewerLoader implements SymbolsViewerLoader {

    lsClient: LSSymbolsViewerClient;
    private _documentPath: string;

    constructor(lsClient: LSSymbolsViewerClient, documentPath: string) {
        this.lsClient = lsClient;
        this._documentPath = documentPath;
    }

    async load(): Promise<SymbolsViewerDocument | undefined> {
        const response = await this.lsClient.OpenAppFileDocument({ path: this._documentPath });
        return {
            uid: response?.documentUid ?? -1,
            root: response?.root
        };
    }

    getName(): string {
        return this._documentPath;
    }

}