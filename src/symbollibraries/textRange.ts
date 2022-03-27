
import * as vscode from 'vscode';
import { TextPosition } from "./textPosition";

export class TextRange {
    start : TextPosition;
    end : TextPosition;
    isEmpty : boolean;
    isSingleLine : boolean;

    constructor() {
        this.isEmpty = true;
        this.isSingleLine = true;
        this.start = new TextPosition();
        this.end = new TextPosition();
    }

    static fromAny(source : any) : TextRange {
        let val = new TextRange();
        if (source.start)
            val.start = TextPosition.fromAny(source.start);
        if (source.end)
            val.end = TextPosition.fromAny(source.end);
        if (source.isEmpty)
            val.isEmpty = true;
        if (source.isSingleLine)
            val.isSingleLine = true;
        return val;
    }

    public intersectVsRange(range : vscode.Range) : boolean {
        return ((this.start.compareVsPosition(range.end) <= 0) && 
            (this.end.compareVsPosition(range.start) >= 0));
    }

    public equalsVsRange(range : vscode.Range) : boolean {
        return ((this.start.character == range.start.character) && (this.start.line == range.start.line) &&
            (this.end.character == range.end.character) && (this.end.line == range.end.line));
    }

    public insideVsRange(range : vscode.Range) : boolean {
        return ((this.start.compareVsPosition(range.start) <= 0) &&
            (this.end.compareVsPosition(range.end) >= 0));
    }

}