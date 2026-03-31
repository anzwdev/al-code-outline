import * as vscode from 'vscode';
import { IIdReservationProvider } from "./IIdReservationProvider";
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { LSObjectKindHelper } from '../../langserver/common_types/lsObjectKindHelper';
import { LSObjectKind } from '../../langserver/common_types/lsObjectKind';

export class LocalIdReservationProvider implements IIdReservationProvider {
    private context: DevToolsExtensionContext;
  
    constructor(context: DevToolsExtensionContext) {
        this.context = context;
    }

    getName(): string {
        return "LocalFiles";
    }

    isAvailable(): boolean {
        return true;
    }

    async suggestObjectId(uri: vscode.Uri, type: string): Promise<number> {
        let objectKind = LSObjectKindHelper.fromString(type, LSObjectKind.Unknown);
        if (objectKind !== LSObjectKind.Unknown) {
            let objectId = await this.context.projectInformationService.getNextObjectId(uri, objectKind);
            if ((objectId) && (objectId > 0)) {
                return objectId;
            }
        }
        return 0;
    }

    async reserveObjectId(uri: vscode.Uri, type: string, id: number): Promise<number> {
        return id;
    }

}