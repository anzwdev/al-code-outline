import * as vscode from 'vscode';

export class CodeOutlineTreeDocumentState {
    private _id: string;
    private _sourceId: string;
    private _state: { [key: string] : vscode.TreeItemCollapsibleState };

    constructor(id: string, sourceId: string) {
        this._id = id;
        this._sourceId = sourceId;
        this._state = {};
    }

    getState(nodeId: string, defaultState: vscode.TreeItemCollapsibleState) : vscode.TreeItemCollapsibleState {
        let state = this._state[nodeId];
        return state !== undefined ? state : defaultState;
    }

    setState(nodeId: string, state: vscode.TreeItemCollapsibleState) {
        this._state[nodeId] = state;
    }

    getId(): string {
        return this._id;
    }
}