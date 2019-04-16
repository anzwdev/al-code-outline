import * as vscode from 'vscode';

export class ALObjectWriter {
    private content : string;
    private indentText : string;
    private indentPart : string;  
    private applicationArea : string;   

    constructor() {
        this.content = "";
        this.indentText = "";
        this.indentPart = "    ";
        this.applicationArea = vscode.workspace.getConfiguration('alOutline').get('defaultAppArea');
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

    public writeLine(line : string) {
        this.content += (this.indentText + line + "\n");
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
        this.writeLine(blockName + "(" + propertyName + ";" + propertySource + ")");
        this.writeStartBlock();
    }

    public writeNameSourceBlock(blockName : string, propertyName : string, propertySource : string) {
        this.writeStartNameSourceBlock(blockName, propertyName, propertySource);
        this.writeEndBlock();
    }

    public writeStartObject(type : string, id : number, name : string) {
        var objectIdText : string;
        if (id == 0)
            objectIdText = 'id';
        else
            objectIdText = id.toString();
        
        name = name.replace("\"", "\"\"");
        name = this.escapeObjectName(name);

        this.writeLine(type + " " + objectIdText + " " + name);
        this.writeStartBlock();
    }

    public writeStartExtensionObject(type : string, id : number, extname : string, targetName : string) {
        var objectIdText : string;
        if (id == 0)
            objectIdText = 'id';
        else
            objectIdText = id.toString();
        
        extname = extname.replace("\"", "\"\"");
        extname = this.escapeObjectName(extname);
        targetName = this.escapeObjectName(targetName);

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

    public writeStartFields() {
        this.writeLine("fields");
        this.writeStartBlock();
    }

    public writeEndFields() {
        this.writeEndBlock();
    }

    public writeStartGroup(type : string, name : string) {
        this.writeLine(type + "(" + name + ")");
        this.writeStartBlock();
    }

    public writeProperty(name : string, value : string) {
        this.writeLine(name + " = " + value + ";");
    }

    public writePageField(fieldName : string) {
        this.writeStartNameSourceBlock("field", this.encodeName(fieldName), this.encodeName(fieldName));
        this.writeApplicationArea();
        this.writeEndBlock();
    }

    public writeApiPageField(fieldName : string) {
        let name : string = this.createApiName(fieldName);
        this.writeStartNameSourceBlock("field", this.encodeName(name), this.encodeName(fieldName));
        this.writeProperty("Caption", this.encodeString(name));
        this.writeApplicationArea();
        this.writeEndBlock();
    }

    public writeApplicationArea() {
        if ((this.applicationArea) && (this.applicationArea !== ""))
            this.writeProperty("ApplicationArea", this.applicationArea);
    }

    public encodeString(text : string) : string {
        return "'" + text.replace("'", "''") + "'";
    }

    public encodeName(name : string) : string {
        return "\"" + name.replace("\"", "\"\"") + "\"";
    }

    public escapeObjectName(name : string) : string {
        if (/\W/.test(name)) {
            name = "\"" + name + "\"";
        }
        return name;
    }

    public createName(source : string) : string {
        return source.replace(/\W/g, '');
    }

    public createApiName(source : string) : string {
        return source.replace(/[^A-Za-z]/g, '');
    }

}