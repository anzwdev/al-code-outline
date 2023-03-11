import * as vscode from 'vscode';

export class TextPosition {
    line : number;
    character : number;

    constructor() {
        this.line = 0;
        this.character = 0;
    }
    
    static fromAny(source : any) {
        let val : TextPosition = new TextPosition(); 
        if (source.line)
            val.line = source.line;
        if (source.character)
            val.character = source.character;
        return val;
    }

    public compareVsPosition(position : vscode.Position) : number {
        if (position.line == this.line)
            return (this.character - position.character);
        else
            return (this.line - position.line);
    }

    set(newLine: number, newCharacter: number) {
        this.line = newLine;
        this.character = newCharacter;
    }

    public compare(position: TextPosition) {
        if (this.line > position.line)
            return 1;
        if (this.line < position.line)
            return -1;
        if (this.character > position.character)
            return 1;
        if (this.character < position.character)
            return -1;
        return 0;
    }

}