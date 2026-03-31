import * as vscode from 'vscode';

export interface IIdNinjaApi {
    suggestIds(uri: vscode.Uri, type: string) : Promise<number[]|undefined>;
    reserveId(uri: vscode.Uri, type: string, objectId: number): Promise<number>;
}