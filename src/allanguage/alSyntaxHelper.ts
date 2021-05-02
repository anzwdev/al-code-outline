'use strict';

import { ToolsGetProjectSettingsResponse } from "../langserver/toolsGetProjectSettingsResponse";
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

    static removePrefixSuffix(text: string, projectSettings: ToolsGetProjectSettingsResponse | undefined): string
    {
        if (projectSettings) {
            //remove first suffix
            var data = ALSyntaxHelper.removeSuffix(text, projectSettings.mandatorySuffixes);
            if (data.found)
                return data.text;

            //remove first prefix
            data = ALSyntaxHelper.removePrefix(data.text, projectSettings.mandatoryPrefixes);
            if (data.found)
                return data.text;

            //remove first prefix/suffix
            data = ALSyntaxHelper.removeSuffix(data.text, projectSettings.mandatoryAffixes);
            if (data.found)
                return data.text;
            data = ALSyntaxHelper.removePrefix(data.text, projectSettings.mandatoryAffixes);
            if (data.found)
                return data.text;
        }

        return text;
    }

    static removePrefix(text: string, prefixes: string[] | undefined): { text: string, found: boolean } {
        if (prefixes) {
            for (let i = 0; i < prefixes.length; i++) {
                if ((prefixes[i]) && (prefixes[i].trim().length > 0) && (text.startsWith(prefixes[i])))
                    return {
                        found: true, 
                        text: text.substring(prefixes[i].length).trim()
                    };
            }
        }
        return {
            found: false,
            text: text
        };
    }

    static removeSuffix(text: string, suffixes: string[] | undefined): { text: string, found: boolean } {
        if (suffixes) {
            for (let i = 0; i < suffixes.length; i++)
            {
                if ((suffixes[i]) && (suffixes[i].trim().length > 0) && (text.endsWith(suffixes[i])))
                    return {
                        found: true,
                        text: text.substring(0, text.length - suffixes[i].length).trim()
                    };
            }
        }
        return {
            found: false,
            text: text
        };
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
            case AZSymbolKind.Interface:
                return "interface";
        }
        return undefined;
    }

    static kindToWorkspaceSymbolType(kind: AZSymbolKind) : string | undefined {
        switch (kind) {
            case AZSymbolKind.TableObject:
                return "Record";
            case AZSymbolKind.CodeunitObject:
                return "Codeunit";
            case AZSymbolKind.PageObject:
                return "Page";
            case AZSymbolKind.ReportObject:
                return "Report";
            case AZSymbolKind.QueryObject:
                return "Query";
            case AZSymbolKind.XmlPortObject:
                return "XmlPort";
            case AZSymbolKind.ControlAddInObject:
                return "UserControl";
            case AZSymbolKind.EnumType:
                return "Enum";
            case AZSymbolKind.TableExtensionObject:
                return "TableExtension";
            case AZSymbolKind.PageExtensionObject:
                return "PageExtension";
            case AZSymbolKind.PageCustomizationObject:
                return "PageCustomization";
            case AZSymbolKind.EnumExtensionType:
                return "EnumExtension";
            case AZSymbolKind.ProfileObject:
                return "Profile";
            case AZSymbolKind.Interface:
                return "Interface";
            case AZSymbolKind.ReportExtensionObject:
                return 'ReportExtension';
            case AZSymbolKind.PermissionSet:
                return 'PermissionSet';
            case AZSymbolKind.PermissionSetExtension:
                return 'PermissionSetExtension';
            case AZSymbolKind.Entitlement:
                return 'Entitlement';
        }
        return undefined;
    }

}