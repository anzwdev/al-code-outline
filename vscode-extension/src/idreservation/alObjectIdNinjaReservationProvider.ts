import * as vscode from 'vscode';
import { IIdNinjaApi } from './IIdNinjaApi';
import { IIdReservationProvider } from "./IIdReservationProvider";

export class ALObjectIdNinjaReservationProvider implements IIdReservationProvider {
    private _idNinjaApi: IIdNinjaApi | undefined;

    getName(): string {
        return "ALObjectIdNinja";
    }

    isAvailable(): boolean {
        if (this._idNinjaApi)
            return true;
        let idNinjaExtension = vscode.extensions.getExtension("vjeko.vjeko-al-objid");
        return ((!!idNinjaExtension) && (!!idNinjaExtension.isActive) && (!!idNinjaExtension.exports));
    }

    async suggestObjectId(uri: vscode.Uri, type: string): Promise<number> {
        try {
            if (await this.loadIdNinjaApi()) {
                type = type.toLowerCase();
                let ids = await this._idNinjaApi!.suggestIds(uri, type);
                if (ids)
                    for (let idx = 0; idx < ids.length; idx++)
                        if (ids[idx] > 0)
                            return ids[idx];
            }
        }
        catch (e) {
        }
        return 0;
    }

    async reserveObjectId(uri: vscode.Uri, type: string, id: number): Promise<number> {
        try {
            if (await this.loadIdNinjaApi()) {
                type = type.toLowerCase();
                let newId = await this._idNinjaApi!.reserveId(uri, type, id);
                if (newId > 0)
                    return newId;
            }
        }
        catch (e) {            
        }
        return id;
    }

    private async loadIdNinjaApi(): Promise<boolean> {
        if (!this._idNinjaApi) {
            let idNinjaExtension = vscode.extensions.getExtension("vjeko.vjeko-al-objid");
            if (idNinjaExtension) {
                if (!idNinjaExtension.isActive)
                    await idNinjaExtension.activate();
                this._idNinjaApi = idNinjaExtension.exports;
            }
        }
        return !!this._idNinjaApi;
    }

}