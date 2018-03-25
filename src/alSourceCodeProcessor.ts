export class ALSourceCodeProcessor {

    static RemoveComments(text : string) : string {
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

    static GetLastWord(text : string) : string {
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

    static GetLastWordAsNumber(text : string) : number {
        var lastWord : string = ALSourceCodeProcessor.GetLastWord(text);
        if (lastWord) {
            var value = parseInt(lastWord);
            if (value)
                return value;
        }
        return 0;
    }

}