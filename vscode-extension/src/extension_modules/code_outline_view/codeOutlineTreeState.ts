import { CodeOutlineTreeDocumentState } from "./codeOutlineTreeDocumentState";

export class CodeOutlineTreeState {
    private _documents: { [key: string] : CodeOutlineTreeDocumentState};
    private _nextId: number;

    constructor() {
        this._nextId = 0;
        this._documents = { };
    }

    getDocumentState(sourceId: string): CodeOutlineTreeDocumentState {
        let state = this._documents[sourceId];
        if (state === undefined) {
            this._nextId++;
            state = new CodeOutlineTreeDocumentState(this._nextId.toString(), sourceId);
            this._documents[sourceId] = state;           
        }
        return state;
    }

}