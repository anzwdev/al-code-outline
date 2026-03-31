import { LSSymbolsViewerClient } from "../../langserver/symbols_viewer/lsSymbolsViewerClient";
import { SymbolsViewerDocument } from "./symbolsViewerDocument";
import { SymbolsViewerLoader } from "./symbolsViewerLoader";

export class ProjectSymbolsViewerLoader implements SymbolsViewerLoader {

    lsClient: LSSymbolsViewerClient;
    private _projectPath: string;
    private _includeDependencies: boolean;

    constructor(lsClient: LSSymbolsViewerClient, projectPath: string, includeDependencies: boolean) {
        this.lsClient = lsClient;
        this._projectPath = projectPath;
        this._includeDependencies = includeDependencies;
    }

    async load(): Promise<SymbolsViewerDocument | undefined> {
        const response = await this.lsClient.OpenProjectDocument({ path: this._projectPath, includeDependencies: this._includeDependencies });
        return {
            uid: response?.documentUid ?? -1,
            root: response?.root
        };
    }

    getName(): string {
        return this._projectPath;
    }

}