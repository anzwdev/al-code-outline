export class ALLineWordReader {
    line: string;
    mlComment: boolean;
    currPos: number;

    constructor() {
        this.line = "";
        this.mlComment = false;
        this.currPos = 0;
    }

    isEOL(): boolean {
        return (this.currPos >= this.line.length);       
    }

    setLine(line: string) {
        this.line = line;
        this.currPos = 0;
    }

    readToEnd() {
        this.skipMLComment();
        while (this.currPos < this.line.length) {
            
            let pos = this.line.indexOf('/', this.currPos);
            if (pos >= 0) {
                this.currPos = pos + 1;
                if (this.currPos >= this.line.length)
                    return;
                
                //line comment
                if (this.line[this.currPos] === '/') {
                    this.currPos = this.line.length;
                    return;
                }

                //ml comment
                if (this.line[this.currPos] === '*') {
                    this.currPos++;
                    this.mlComment = true;
                    this.skipMLComment();
                }

            } else {
                this.currPos = this.line.length;
                return;
            }
        }

    }

    nextWord(): string | undefined {
        if (this.currPos >= this.line.length)
            return undefined;
        
        if (!this.skipSpaces())
            return undefined;
        
        let start = this.currPos;

        if (!this.skipWordChars())
            return undefined;

        return this.line.substring(start, this.currPos);
    }

    private skipMLComment() {
        if (this.mlComment) {
            let endComment = this.line.indexOf('*/', this.currPos);
            if (endComment >= 0) {
                this.currPos = endComment + 2;
                this.mlComment = false;
            } else {
                this.currPos = this.line.length;
            }
        }
    }

    private skipSpaces() : boolean {
        this.skipMLComment();

        while ((this.currPos < this.line.length) && (!this.nonSpaceChar(this.line[this.currPos]))) {
            if ((this.currPos < this.line.length - 1) && (this.line[this.currPos] === '/')) {
                this.currPos++;
                
                if (this.line[this.currPos] === '/') {
                    this.currPos = this.line.length;
                    return false;
                }
                
                if (this.line[this.currPos] === '*') {
                    this.mlComment = true;
                    this.currPos++;
                    this.skipMLComment();
                }
            } else {
                this.currPos++;
            }
        }

        return (this.currPos < this.line.length);
    }

    private skipWordChars(): boolean {
        let start = this.currPos;

        if (this.currPos < this.line.length) {
            let ch = this.line[this.currPos];

            if ((ch === '(') || (ch === ')')) {
                this.currPos++;
                return true;
            }

            if (!this.validFirstWordChar(ch))
                return false;
            this.currPos++;

            while ((this.currPos < this.line.length) && (this.validWordChar(this.line[this.currPos]))) {
                this.currPos++;
            }
        }

        return (this.currPos > start);
    }

    private nonSpaceChar(ch: string): boolean {
        return ((ch >= 'a') && (ch <= 'z')) ||
            ((ch >= 'A') && (ch <= 'Z')) ||
            ((ch >= '0') && (ch <= '9')) ||
            (ch === '_') ||
            (ch === '$') ||
            (ch === '(') ||
            (ch === ')') ||
            (ch === '#');
    }

    private validFirstWordChar(ch: string): boolean {
        return ((ch >= 'a') && (ch <= 'z')) ||
            ((ch >= 'A') && (ch <= 'Z')) ||
            (ch === '_') ||
            (ch === '$') ||
            (ch === '#');
    }

    private validWordChar(ch: string): boolean {
        return ((ch >= 'a') && (ch <= 'z')) ||
            ((ch >= 'A') && (ch <= 'Z')) ||
            ((ch >= '0') && (ch <= '9')) ||
            (ch === '_');
    }

}