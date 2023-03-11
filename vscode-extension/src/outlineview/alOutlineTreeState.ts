import { ALOutlineTreeDocumentState } from './alOutlineTreeDocumentState';

export class ALOutlineTreeState {
    private _documents: { [key: string] : ALOutlineTreeDocumentState};
    private _nextId: number;

    constructor() {
        this._nextId = 0;
        this._documents = { };
    }

    getDocumentState(sourceId: string): ALOutlineTreeDocumentState {
        let state = this._documents[sourceId];
        if (state === undefined) {
            this._nextId++;
            state = new ALOutlineTreeDocumentState(this._nextId.toString(), sourceId);
            this._documents[sourceId] = state;           
        }
        return state;
    }

}