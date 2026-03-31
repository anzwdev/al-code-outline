import * as vscode from 'vscode';

export interface IIdReservationProvider {
    getName(): string;
    isAvailable(): boolean;
    suggestObjectId(uri: vscode.Uri, type: string): Promise<number>
    reserveObjectId(uri: vscode.Uri, type: string, id: number): Promise<number>;
}