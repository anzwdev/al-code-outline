export class ALSourceCodeProcessor {

    static indexOf(text : string, subString : string, startPos : number) {
        let hasName : boolean = true;
        let hasString : boolean = true;
        let hasMlComment : boolean = true;
        let hasSlComment : boolean = true;
        let namePos : number = -1;
        let stringPos : number = -1;
        let mlCommentPos : number = -1;
        let slCommentPos : number = -1;   
        
        while (startPos >= 0) {
            let textPos = text.indexOf(subString, startPos);
            if (textPos < 0)
                return -1;

            if (hasName) {
                namePos = text.indexOf("\"", startPos);
                hasName = (namePos >= 0);
            }
            if (hasString) {
                stringPos = text.indexOf("'", startPos);
                hasString = (stringPos >= 0);
            }
            if (hasMlComment) {
                mlCommentPos = text.indexOf("/*", startPos);
                hasMlComment = (mlCommentPos >= 0);
            }
            if (hasSlComment) {
                slCommentPos = text.indexOf("//", startPos);
                hasSlComment = (slCommentPos >= 0);
            }

            //name text
            if ((hasName) && (namePos < textPos) && ((namePos < stringPos) || (!hasString)) && ((namePos < mlCommentPos) || (!hasMlComment)) && ((namePos < slCommentPos) || (!hasSlComment))) {
                namePos = text.indexOf("\"", namePos + 1);
                if (namePos < 0)
                    return -1;
                startPos = namePos + 1;
            //string text
            } else if ((hasString) && (stringPos < textPos) && ((stringPos < namePos) || (!hasName)) && ((stringPos < mlCommentPos) || (!hasMlComment)) && ((stringPos < slCommentPos) || (!hasSlComment))) {
                stringPos = text.indexOf("'", stringPos + 1);
                if (stringPos < 0)
                    return -1;
                startPos = stringPos + 1;
            //multiline comment
            } else if ((hasMlComment) && (mlCommentPos < textPos) && ((mlCommentPos < namePos) || (!hasName)) && ((mlCommentPos < stringPos) || (!hasString)) && ((mlCommentPos < slCommentPos) || (!hasSlComment))) {
                mlCommentPos = text.indexOf("*/", mlCommentPos + 2);
                if (mlCommentPos < 0)
                    return -1;
                startPos = mlCommentPos + 2;
            //single line comment
            } else if ((hasSlComment) && (slCommentPos < textPos) && ((slCommentPos < namePos) || (!hasName)) && ((slCommentPos < stringPos) || (!hasString)) && ((slCommentPos < mlCommentPos) || (!hasMlComment))) {
                slCommentPos = text.indexOf("\n", slCommentPos + 2);
                if (slCommentPos < 0)
                    return -1;
                startPos = slCommentPos + 1;
            } else
                return textPos;        
        }
        return -1;
    }

    static removeComments(text : string) : string {
        var outText : string = '';
        var processingPos = 0;

        while (processingPos < text.length) {
            var mlCommentPos : number = text.indexOf('/*', processingPos);
            var slCommentPos : number = text.indexOf('//', processingPos);
            var stringPos : number = text.indexOf('\'', processingPos);
            var namePos : number = text.indexOf('"', processingPos);
            var endPos : number = -1;
            var blockStartPos : number = -1;

            //process multiline comment
            if ((mlCommentPos >= 0) && 
                ((slCommentPos < 0) || (mlCommentPos < slCommentPos)) && 
                ((stringPos < 0) || (mlCommentPos < stringPos)) &&
                ((namePos < 0) || (mlCommentPos < namePos)))  {
                blockStartPos = mlCommentPos;
                if (mlCommentPos + 2 < text.length) {
                    endPos = text.indexOf('*/', mlCommentPos + 2);
                    if (endPos >= 0)
                        endPos += 2;
                }
            }
            else if ((slCommentPos >= 0) &&
                ((stringPos < 0) || (slCommentPos < stringPos)) &&
                ((namePos < 0) || (slCommentPos < namePos))) {
                blockStartPos = slCommentPos;
                if (slCommentPos + 2 < text.length) {                
                    endPos = text.indexOf('\n', slCommentPos);
                }
            }
            else if ((stringPos >= 0) && 
                ((namePos < 0) || (stringPos < namePos))) {
                blockStartPos = stringPos;
                if (stringPos + 1 < text.length) {
                    endPos = text.indexOf('\'', stringPos);
                    if (endPos >= 0)
                        endPos += 1;
                }
            }
            else if (namePos >= 0) {
                blockStartPos = namePos;
                if (namePos + 1 < text.length) {
                    endPos = text.indexOf('"', namePos);
                    if (endPos >= 0)
                        endPos += 1;
                }
            } else {
                outText = outText + text.substr(processingPos);
                return outText;
            }

            outText = outText + text.substring(processingPos, blockStartPos);
            if (endPos < 0)
                return outText;
            processingPos = endPos;
        }

        return outText;
    }

    static getLastWord(text : string) : string {
        var endPos : number = text.length - 1;
        while ((endPos >= 0) && (text.charAt(endPos) <= ' '))
            endPos--;
        if (endPos < 0)
            return '';
        var startPos : number = endPos - 1;
        while ((startPos >= 0) && (text.charAt(startPos) > ' '))
            startPos--;
        return text.substring(startPos + 1, endPos + 1);
    }

    static getLastWordAsNumber(text : string) : number {
        var lastWord : string = ALSourceCodeProcessor.getLastWord(text);
        if (lastWord) {
            var value = parseInt(lastWord);
            if (value)
                return value;
        }
        return 0;
    }

}