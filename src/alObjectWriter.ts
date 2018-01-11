export class ALObjectWriter {
    private content : string;
    private indentText : string;
    private indentPart : string;     

    constructor() {
        this.content = "";
        this.indentText = "";
        this.indentPart = "    ";
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

    public writeStartObject(type : string, id : number, name : string) {
        var objectIdText : string;
        if (id == 0)
            objectIdText = 'id';
        else
            objectIdText = id.toString();
        
        this.writeLine(type + " " + objectIdText + " \"" + name + "\"");
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

    public writeStartControlGroup(type : string, name : string) {
        this.writeLine(type + "(" + name + ")");
        this.writeStartBlock();
    }

    public writeProperty(name : string, value : string) {
        this.writeLine(name + " = " + value + ";");
    }

    public writePageField(fieldName : string) {
        this.writeLine("field(\"" + fieldName + "\";\"" + fieldName + "\")");
        this.writeStartBlock();
        this.writeEndBlock();
    }

}