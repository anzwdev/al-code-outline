'use strict';

import { AZSymbolKind } from "../symbollibraries/azSymbolKind";

export class ALSyntaxHelper {
    static nameCharacters = '0123456789QWERTYUIOPLKJHGFDSAZXCVBNMqwertyuioplkjhgfdsazxcvbnm-_"';

    static toNameText(name : string) : string {
        if (name.match(/^[a-zA-Z_]\w*$/))
            return name;
        return "\"" + name.replace(new RegExp("\"", "g"), "\"\"") + "\"";
    }

    static toStringText(text : string) : string {
        return "'" + text.replace(new RegExp("'", "g"), "''") + "'";
    }

    static fromNameText(name : string) : string {
        name = name.trim();
        if ((name.length > 1) && (name.substr(0, 1) == "\"") && (name.substr(name.length - 1, 1) == "\"")) {
            name = name.substr(1, name.length - 2).replace(new RegExp("\"\"", "g"), "\"");           
        }
        return name;
    }

    static fromStringText(text : string) : string {
        text = text.trim();
        if ((text.length > 1) && (text.substr(0, 1) == "'") && (text.substr(text.length - 1, 1) == "'")) {
            text = text.substr(1, text.length - 2).replace(new RegExp("''", "g"), "'");
        }
        return text;
    }

    static getNameFromString(text : string, startPos : number) : string {
        let endPos : number = startPos;
        let inName : boolean = false;

        //skip invalid characters
        while ((startPos < text.length) && (ALSyntaxHelper.spaceChar(text.charAt(startPos))))
            startPos++;

        //skip name
        inName = false;
        endPos = startPos;
        while ((endPos < text.length) && ((inName) || (ALSyntaxHelper.validNameChar(text.charAt(endPos))))) {
            if (text.charAt(endPos) == '"')
                inName = !inName;
            endPos++;
        }

        if (endPos > startPos)
            return ALSyntaxHelper.fromNameText(text.substr(startPos, endPos - startPos));

        return '';
    }

    static validNameChar(text : string) : boolean {
        return (ALSyntaxHelper.nameCharacters.indexOf(text) >= 0);
    }

    static spaceChar(text : string) : boolean {
        return (text <= ' ');
    }

    static kindToVariableType(kind : AZSymbolKind) : string | undefined {
        switch (kind) {
            case AZSymbolKind.TableObject:
                return "record";
            case AZSymbolKind.CodeunitObject:
                return "codeunit";
            case AZSymbolKind.PageObject:
                return "page";
            case AZSymbolKind.ReportObject:
                return "report";
            case AZSymbolKind.QueryObject:
                return "query";
            case AZSymbolKind.XmlPortObject:
                return "xmlport";
            case AZSymbolKind.ControlAddInObject:
                return "usercontrol";
            case AZSymbolKind.EnumType:
                return "enum";
        }
        return undefined;
    }
    
}