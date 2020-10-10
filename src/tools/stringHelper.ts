import * as vscode from 'vscode';

export class StringHelper {

    static getDefaultEndOfLine(destUri: vscode.Uri | undefined): string {
        let eolText = vscode.workspace.getConfiguration('files', destUri).get<string>('eol');
        if ((eolText) && (eolText != 'auto'))
            return eolText;
        return (process.platform === 'win32' ? '\r\n' : '\n');
    }

    static emptyIfNotDef(value: string | undefined) {
        if (value)
            return value;
        return "";
    }

    static equalStartLength(text1: string, text2: string): number {
        let len1 = text1.length;
        let len2 = text2.length;
        let len = Math.min(len1, len2);

        for (let i=0; i<len; i++) {
            if (text1.charAt(i) !== text2.charAt(i))
                return i;
        }
        return len;
    }

    static equalEndLength(text1: string, text2: string): number {
        let len1 = text1.length;
        let len2 = text2.length;
        let t2diff = len2 - len1;

        for (let i = (len1 - 1); (i>=0) && ((i+t2diff) >= 0); i--) {
            if (text1.charAt(i) !== text2.charAt(i + t2diff))
                return (len1 - i - 1);
        }
        return Math.min(len1, len2);
    }

}