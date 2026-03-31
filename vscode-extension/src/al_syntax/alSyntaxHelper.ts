import { LSPIProjectProfileAffixes } from "../langserver/project_information/profile/lspiProjectProfileAffixes";

export class ALSyntaxHelper {
    static nameCharacters = '0123456789QWERTYUIOPLKJHGFDSAZXCVBNMqwertyuioplkjhgfdsazxcvbnm-_"';
    static keyWords = '|action|actions|add|addafter|addbefore|addfirst|addlast|and|area|array|ascending|assembly|asserterror|automation|average|begin' +
        '|biginteger|bigtext|blob|boolean|break|byte|case|char|chartpart|clienttype|code|codeunit|column|completiontriggererrorlevel' +
        '|connectiontype|const|controladdin|count|cuegroup|customizes|database|dataclassification|dataitem|datascope|dataset|date|dateformula' +
        '|datetime|decimal|defaultlayout|descending|dialog|dictionary|div|do|dotnet|dotnetassembly|dotnettypedeclaration|downto|duration' +
        '|elements|else|end|entitlement|enum|enumextension|errorinfo|errortype|event|executioncontext|executionmode|exist|exit|extends' +
        '|field|fieldattribute|fieldclass|fieldelement|fieldgroup|fieldgroups|fieldref|fields|fieldtype|file|filter|filterpagebuilder|fixed' +
        '|for|foreach|function|grid|group|guid|httpclient|httpcontent|httpheaders|httprequestmessage|httpresponsemessage|if|implements|in' +
        '|indataset|instream|integer|interface|internal|joker|jsonarray|jsonobject|jsontoken|jsonvalue|key|keyref|keys|label|labels|layout|list' +
        '|local|lookup|max|min|mod|modify|moduledependencyinfo|moduleinfo|moveafter|movebefore|movefirst|movelast|none|not|notification' +
        '|notificationscope|objecttype|of|option|or|order|outstream|page|pagecustomization|pageextension|pageresult|part|permissionset' +
        '|permissionsetextension|procedure|profile|program|protected|query|record|recordid|recordref|repeat|repeater|report|reportextension' +
        '|reportformat|requestpage|runonclient|schema|securityfilter|securityfiltering|separator|sessionsettings|sorting|sum|suppressdispose' +
        '|systempart|table|tableconnectiontype|tabledata|tableelement|tableextension|tablefilter|temporary|testaction|testfield|testfilterfield' +
        '|testpage|testpermissions|testrequestpage|text|textattribute|textbuilder|textconst|textelement|textencoding|then|time|to|transactionmodel' +
        '|transactiontype|trigger|type|until|upperlimit|usercontrol|value|var|variant|verbosity|version|view|views|webserviceactioncontext' +
        '|webserviceactionresultcode|where|while|with|withevents|xmlattribute|xmlattributecollection|xmlcdata|xmlcomment|xmldeclaration' +
        '|xmldocument|xmldocumenttype|xmlelement|xmlnamespacemanager|xmlnametable|xmlnode|xmlnodelist|xmlport|xmlprocessinginstruction' +
        '|xmlreadoptions|xmltext|xmlwriteoptions|xor|';   

    static isKeyword(name: string) : boolean {
        return (!!((name) && (ALSyntaxHelper.keyWords.indexOf("|" + name.toLowerCase() + "|") >= 0)));
    }

    static toNameText(name : string) : string {
        if ((name.match(/^[a-zA-Z_]\w*$/)) && (!ALSyntaxHelper.isKeyword(name))) {
            return name;
        }
        return "\"" + name.replace(new RegExp("\"", "g"), "\"\"") + "\"";
    }

    static fromNameText(name : string) : string {
        name = name.trim();
        if ((name.length > 1) && (name.substr(0, 1) === "\"") && (name.substr(name.length - 1, 1) === "\"")) {
            name = name.substr(1, name.length - 2).replace(new RegExp("\"\"", "g"), "\"");           
        }
        return name;
    }

    static toStringText(text : string) : string {
        return "'" + text.replace(new RegExp("'", "g"), "''") + "'";
    }

    static validNameChar(text : string) : boolean {
        return (ALSyntaxHelper.nameCharacters.indexOf(text) >= 0);
    }

    static splitNamesList(valueList: string | undefined): string[] {
        let values : string[] = [];

        if ((valueList) && (valueList.length > 0)) {            
            let startPos = 0;
            let inName = false;

            for (let pos = 0; pos < valueList.length; pos++) {
                switch (valueList[pos]) {
                    case ',':
                        if (!inName) {
                            let valueString = valueList.substring(startPos, pos);
                            if ((values.length > 0) || (valueString !== " "))  {
                                valueString = ALSyntaxHelper.fromNameText(valueString.trim());
                            }
                            values.push(valueString);
                            startPos = pos + 1;
                        }
                        break;
                    case '"':
                        inName = !inName;
                        break;
                }
            }

            if (startPos < valueList.length) {
                let valueString = valueList.substring(startPos, valueList.length);
                if ((values.length > 0) || (valueString !== " "))  {
                    valueString = ALSyntaxHelper.fromNameText(valueString.trim());
                }
                values.push(valueString);
            }
        }

        return values;
    }

    static removePrefixSuffix(text: string, projectAffixes: LSPIProjectProfileAffixes | undefined): string
    {
        if (projectAffixes) {
            //remove first suffix
            var data = ALSyntaxHelper.removeSuffix(text, projectAffixes.mandatorySuffixes);
            if (data.found) {
                return data.text;
            }

            //remove first prefix
            data = ALSyntaxHelper.removePrefix(data.text, projectAffixes.mandatoryPrefixes);
            if (data.found) {
                return data.text;
            }

            //remove first prefix/suffix
            data = ALSyntaxHelper.removeSuffix(data.text, projectAffixes.mandatoryAffixes);
            if (data.found) {
                return data.text;
            }
            data = ALSyntaxHelper.removePrefix(data.text, projectAffixes.mandatoryAffixes);
            if (data.found) {
                return data.text;
            }
        }

        return text;
    }

    static removePrefix(text: string, prefixes: string[] | undefined): { text: string, found: boolean } {
        if (prefixes) {
            for (let i = 0; i < prefixes.length; i++) {
                if ((prefixes[i]) && (prefixes[i].trim().length > 0) && (text.startsWith(prefixes[i]))) {
                    return {
                        found: true, 
                        text: text.substring(prefixes[i].length).trim()
                    };
                }
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
                if ((suffixes[i]) && (suffixes[i].trim().length > 0) && (text.endsWith(suffixes[i]))) {
                    return {
                        found: true,
                        text: text.substring(0, text.length - suffixes[i].length).trim()
                    };
                }
            }
        }
        return {
            found: false,
            text: text
        };
    }


}