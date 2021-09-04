import * as vscode from 'vscode';
import { ALSyntaxHelper } from './alSyntaxHelper';
import { StringHelper } from '../tools/stringHelper';
import { NameValue } from '../tools/nameValue';

export class ALSyntaxWriter {
    private content : string;
    private indentText : string;
    private indentPart : string;  
    public applicationArea : string;   
    private propertiesCache : NameValue[];
    private fieldToolTip: string;
    private fieldToolTipComment: string;
    private useTableFieldDescriptionAsToolTip: boolean;
    private eol: string;

    constructor(destUri: vscode.Uri | undefined) {
        let config = vscode.workspace.getConfiguration('alOutline', destUri);
        
        this.content = "";
        this.indentText = "";
        this.indentPart = "    ";
        this.applicationArea = StringHelper.emptyIfNotDef(config.get<string>('defaultAppArea'));
        this.fieldToolTip = StringHelper.emptyIfNotDef(config.get<string>('pageFieldToolTip'));
        this.fieldToolTipComment = StringHelper.emptyIfNotDef(config.get<string>('pageFieldToolTipComment'));
        this.useTableFieldDescriptionAsToolTip = !!config.get<boolean>('useTableFieldDescriptionAsToolTip');
        this.propertiesCache = [];        
        this.eol = StringHelper.getDefaultEndOfLine(destUri);
    }

    public toString() : string {
        return this.content;
    }

    public incIndent() {
        this.indentText += this.indentPart;
    }

    public decIndent() {
        if (this.indentText.length > this.indentPart.length)
            this.indentText = this.indentText.substr(0, this.indentText.length - this.indentPart.length);
        else
            this.indentText = "";
    }

    public setIndent(value : number) {
        let text : string = " ";
        this.indentText = text.repeat(value);
    }

    public writeLine(line : string) {
        this.content += (this.indentText + line + this.eol);
    }

    public writeStartBlock() {
        this.writeLine("{");
        this.incIndent();
    }

    public writeEndBlock() {
        this.decIndent();
        this.writeLine("}");
    }

    public writeStartNamedBlock(name : string) {
        this.writeLine(name);
        this.writeStartBlock();
    }

    public writeStartNameSourceBlock(blockName : string, propertyName : string, propertySource : string) {
        this.writeLine(blockName + "(" + propertyName + "; " + propertySource + ")");
        this.writeStartBlock();
    }

    public writeNameSourceBlock(blockName : string, propertyName : string, propertySource : string) {
        this.writeStartNameSourceBlock(blockName, propertyName, propertySource);
        this.writeEndBlock();
    }

    public writeStartObject(type : string, id : string, name : string) {
        var objectIdText : string;
        if ((id == '') || (id == '0'))
            objectIdText = 'id';
        else
            objectIdText = id.toString();
        
        name = ALSyntaxHelper.toNameText(name);

        this.writeLine(type + " " + objectIdText + " " + name);
        this.writeStartBlock();
    }

    public writeStartInterface(name: string) {
        name = ALSyntaxHelper.toNameText(name);
        this.writeLine("interface " + name);
        this.writeStartBlock();
    }

    public writeStartCodeunit(id : string, name : string, interfaceName: string | undefined) {
        var objectIdText : string;
        if ((id == '') || (id == '0'))
            objectIdText = 'id';
        else
            objectIdText = id.toString();
        
        name = ALSyntaxHelper.toNameText(name);

        let interfaceText = "";
        if ((interfaceName) && (interfaceName.length > 0))
            interfaceText = " implements " + ALSyntaxHelper.toNameText(interfaceName);

        this.writeLine("codeunit " + objectIdText + " " + name + interfaceText);
        this.writeStartBlock();
    }


    public writeStartExtensionObject(type : string, id : string, extname : string, targetName : string) {
        var objectIdText : string;
        if ((id == '') || (id == '0'))
            objectIdText = 'id';
        else
            objectIdText = id.toString();
        
        extname = ALSyntaxHelper.toNameText(extname);
        targetName = ALSyntaxHelper.toNameText(targetName);

        this.writeLine(type + " " + objectIdText + " " + extname + " extends " + targetName);
        
        this.writeStartBlock();
    }

    public writeEndObject() {
        this.writeEndBlock();
    }

    public writeStartLayout() {
        this.writeLine("layout");
        this.writeStartBlock();
    }

    public writeEndLayout() {
        this.writeEndBlock();
    }

    public writeStartActions() {
        this.writeLine("actions");
        this.writeStartBlock();
    }

    public writeEndActions() {
        this.writeEndBlock();
    }

    public writeStartDataset() {
        this.writeLine("dataset");
        this.writeStartBlock();
    }

    public writeEndDataset() {
        this.writeEndBlock();
    }

    public writeStartRequestPage() {
        this.writeLine("requestpage");
        this.writeStartBlock();
    }

    public writeEndRequestPage() {
        this.writeEndBlock();
    }

    public writeStartFields() {
        this.writeLine("fields");
        this.writeStartBlock();
    }

    public writeEndFields() {
        this.writeEndBlock();
    }

    public writeStartAdd(name: string) {
        this.writeStartGroup("add", name);
    }

    public writeStartGroup(type : string, name : string) {
        this.writeLine(type + "(" + name + ")");
        this.writeStartBlock();
    }

    public writeProperty(name : string, value : string) {
        this.writeLine(name + " = " + value + ";");
    }

    public writeStartProperty(name : string) {
        this.writeLine(name + " =");
        this.incIndent();
    }
    
    public writePropertyValue(value: string, lastValue: boolean) {
        if (lastValue) {
            this.writeLine(value + ";");
            this.decIndent();
        } else
            this.writeLine(value + ",");
    }

    public addProperty(name : string, value : string) {
        this.propertiesCache.push(new NameValue(name, value));
    }

    public writeProperties() {
        if (this.propertiesCache.length > 0) {
            
            this.propertiesCache.sort((propA, propB) => {
                return propA.name.localeCompare(propB.name, undefined, { numeric: true, sensitivity: 'base' });
            });

            for (let i=0; i<this.propertiesCache.length; i++)
                this.writeProperty(this.propertiesCache[i].name, this.propertiesCache[i].value);
            
            this.propertiesCache = [];
        }
    }

    public writeReportColumn(dataSetName: string, source: string, addDataItemName: boolean) {
        let columnName = this.createName(source);
        if (addDataItemName)
            columnName = columnName + "_" + this.createName(dataSetName);
        
        this.writeNameSourceBlock("column", columnName, 
            this.encodeName(source));        
    }

    public writeTableField(fieldId: string, fieldName: string, fieldDataType: string, fieldLength: string, dataClassification: string) {
        let dataType = fieldDataType.toLowerCase();
        if ((fieldLength) && ((dataType == 'text') || (dataType == 'code')))
            fieldDataType = fieldDataType + '[' + fieldLength + ']';

        if ((dataType.startsWith("enum ")) && (dataType.length > 5)) {
            let enumType = fieldDataType.substr(5).trim();
            if (!enumType.startsWith('"'))
                enumType = ALSyntaxHelper.toNameText(enumType);
            fieldDataType = "Enum " + enumType;
        }

        this.writeLine("field(" + fieldId + "; " + ALSyntaxHelper.toNameText(fieldName) + "; " + fieldDataType + ")");
        this.writeStartBlock();
        this.writeProperty('Caption', ALSyntaxHelper.toStringText(fieldName));
        if (dataClassification)
            this.writeProperty("DataClassification", dataClassification);
        else
            this.writeProperty("DataClassification", "ToBeClassified");
        this.writeEndBlock();
    }

    public writePageField(fieldName : string, fieldCaption: string | undefined, fieldCaptionComment: string | undefined, fieldDescription: string | undefined, createToolTip: boolean) {
        this.writeStartNameSourceBlock("field", this.encodeName(fieldName), 'Rec.' + this.encodeName(fieldName));
        if (createToolTip)
            this.writeTooltip(this.fieldToolTip, this.fieldToolTipComment, fieldCaption, fieldCaptionComment, fieldDescription);
        this.writeApplicationArea();
        this.writeEndBlock();
    }

    public writeApiPageField(fieldName : string, fieldCaption: string | undefined, fieldCaptionComment: string | undefined, useTableFieldCaption: boolean) {
        let name : string = this.createApiName(fieldName);
        this.writeStartNameSourceBlock("field", this.encodeName(name), 'Rec.' + this.encodeName(fieldName));
        
        if (useTableFieldCaption) {
            if ((!fieldCaption) || (fieldCaption === ''))
                fieldCaption = fieldName;
            if ((fieldCaptionComment) && (fieldCaptionComment !== ''))
                this.addProperty("Caption", this.encodeString(fieldCaption) + ', Comment = ' + this.encodeString(fieldCaptionComment));
            else
                this.addProperty("Caption", this.encodeString(fieldCaption));
        } else
            this.addProperty("Caption", this.encodeString(name) + ', Locked = true');
        
        this.writeProperties();
        this.writeEndBlock();
    }

    public writeApplicationArea() {
        if ((this.applicationArea) && (this.applicationArea !== ""))
            this.writeProperty("ApplicationArea", this.applicationArea);
    }

    public writeTooltip(captionTemplate: string, commentTemplate: string, value: string | undefined, comment: string | undefined, fieldDescription: string | undefined) {
        let textValue: string | undefined = undefined;

        if ((this.useTableFieldDescriptionAsToolTip) && (fieldDescription) && (fieldDescription != ""))
            textValue = this.encodeString(fieldDescription);
        else if ((captionTemplate) && (captionTemplate != "") && (value) && (value != "")) {
            textValue = this.applyCaptionTemplate(captionTemplate, value, comment);
            let commentValue = this.applyCaptionTemplate(commentTemplate, value, comment);
            textValue = this.encodeString(textValue);
            if ((commentValue) && (commentValue != ""))
                textValue = textValue + ", Comment = " + this.encodeString(commentValue);            
        }
        if ((textValue) && (textValue != ""))
            this.writeProperty("ToolTip", textValue);
    }

    protected applyCaptionTemplate(template: string, value: string | undefined, comment: string | undefined) {
        if ((template) && (template != "")) {
            if (!value)
                value = "";
            if (!comment)
                comment = "";
            template = template.replace(new RegExp("%1", "g"), value);
            template = template.replace(new RegExp("%Caption%", "g"), value);
            template = template.replace(new RegExp("%Caption.Comment%", "g"), comment);
        }
        return template;
    }

    public addApplicationAreaProperty() {
        if ((this.applicationArea) && (this.applicationArea !== ""))
            this.addProperty("ApplicationArea", this.applicationArea);
    }

    public encodeString(text : string) : string {
        return ALSyntaxHelper.toStringText(text); 
    }

    public encodeName(name : string) : string {
        return ALSyntaxHelper.toNameText(name);
    }

    public createName(source : string) : string {
        return source.replace(/\W/g, '');
    }

    public createApiName(source : string) : string {
        let text = '';
        let toLower = true;
        let toUpper = false;
        source = source.trim();        
        for (let i=0; i<source.length; i++) {
            let character = source[i];
            let isLowerCaseLetterChar = ((character >= 'a') && (character <= 'z'));
            let isUpperCaseLetterChar = ((character >= 'A') && (character <= 'Z'));
            let isDigitChar = ((character >= '0') && (character <= '9'));
            let validCharacter = ((isLowerCaseLetterChar) || (isUpperCaseLetterChar) || ((isDigitChar) && (text !== '')));            

            if ((text !== '') || (validCharacter)) {
                //if text starts with upperCase, conver all these characters to lowerCase
                if (isUpperCaseLetterChar) {
                    toUpper = false;

                    //do not convert to lowerCase if next character is lowerCase (i.e. EDIDocument => ediDocument), but only if it is not first character in the name (i.e. MyField => myField)
                    if ((text !== '') && (toLower) && (i < (source.length - 1))) {
                        let nextCharacter = source[i + 1];
                        if (((nextCharacter >= 'a') && (nextCharacter <= 'z')))
                            toLower = false;
                    }

                    if (toLower)
                        character = character.toLowerCase();
                } else {
                    toLower = false;
                    if ((isLowerCaseLetterChar) && (toUpper)) {
                        character = character.toUpperCase();
                        toUpper = false;
                    }
                    //if current character is not lowerCase letter, then convert next lowerCase letter to upperCase
                    if (!isLowerCaseLetterChar)
                        toUpper = true;
                }

                //append letters to text
                if (validCharacter)
                    text = text + character;
            }
        }

        return text;
   }

}