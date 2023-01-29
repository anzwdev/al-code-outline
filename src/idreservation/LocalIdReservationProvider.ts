import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { IIdReservationProvider } from "./IIdReservationProvider";

export class LocalIdReservationProvider implements IIdReservationProvider {
    private _context: DevToolsExtensionContext;
  
    constructor(context: DevToolsExtensionContext) {
        this._context = context;
    }

    getName(): string {
        return "LocalFiles";
    }

    isAvailable(): boolean {
        return true;
    }

    async suggestObjectId(uri: vscode.Uri, type: string): Promise<number> {
        return await this._context.toolsLangServerClient.getNextObjectId(uri?.fsPath, type);
    }

    async reserveObjectId(uri: vscode.Uri, type: string, id: number): Promise<number> {
        return id;
    }

}