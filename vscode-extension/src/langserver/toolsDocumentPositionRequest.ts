import * as vscode from 'vscode';
import { TextPosition } from "../symbollibraries/textPosition";

export class ToolsDocumentPositionRequest {
    isActiveDocument: boolean;
    source?: string;
    position: TextPosition;

    constructor(newIsActive: boolean, newSource: string | undefined, newPosition: vscode.Position) {
        this.isActiveDocument = newIsActive;
        this.source = newSource;
        this.position = new TextPosition();
        this.position.set(newPosition.line, newPosition.character);
    }
}