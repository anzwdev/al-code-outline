class AZFormView {

    constructor(elementId, fields) {
        this._elementId = elementId;
        this._fields = fields;
        this._mainElement = document.getElementById(elementId);
        this._tabView = new AZTabView(elementId);

        this.renderView();
    }

    renderView() {
        if (!this._fields)
            return;

        this._gridViews = {};

        for (let i=0; i<this._fields.length; i++)
            this.renderField(i);
        this._tabView.selectTab(0);
    }

    renderField(idx) {
        let field = this._fields[idx];
        
        if (!field.type) 
            return;

        if (field.type == "group") {
            this._tabView.addTab(field.caption);
            return;
        }

        let gridElement = undefined;

        let lineElement = document.createElement("div");
        lineElement.className = "formline";
        
        let captionElement = document.createElement("div");
        captionElement.className = "formcaption";
        captionElement.innerText = field.caption;
        lineElement.appendChild(captionElement);

        let fieldElement = document.createElement("div");
        fieldElement.className = "formfield";
            
        if (field.type == "string") {
            fieldElement.appendChild(this.createEditorField(idx, field, "text"));
        } else if (field.type == "boolean") {
            fieldElement.appendChild(this.createEditorField(idx, field, "checkbox"));
        }
        lineElement.appendChild(fieldElement);

        this._tabView.appendContentChild(lineElement);

        
        if (field.type == "array") {
            if (field.fields) {
                lineElement = document.createElement("div");
                lineElement.className = "formline";        
       
                let gridElement = document.createElement("div");
                gridElement.className = "azgridviewcont";
                gridElement.id = "fld" + field.name;
                lineElement.appendChild(gridElement);
                
                this._tabView.appendContentChild(lineElement);
                
                field.gridView = new AZGridView(gridElement.id, field.fields,
                    undefined, undefined, "", true);
                field.gridView.cellChanged = ((rowIndex, fieldName, value) => {
                    if (this.fieldChanged)
                        this.fieldChanged(idx);
                });
            }
        }
        
    }

    createEditorField(idx, field, type) {
        let editor = document.createElement("input");
        editor.type = type;
        if (type == "checkbox")
            editor.className = "checkbox";
        else
            editor.className = "fulltext";
        editor.id = "fld" + field.name;

        editor.addEventListener('change', event => {
            this.onFieldChanged(idx);
        });
        //editor.addEventListener('input', event => {
        //    this.onFieldChanged(idx);
        //});

        if (field.enum) {
            let me = this;
            let allowedChars = new RegExp(/^[a-zA-Z\s]+$/);
            this._autocomplete = autocomplete({
                input: editor,
                disableOpenOnKeyDown: true,
                minLength: -1,
                onSelect: function (item, inputfield) {
                    inputfield.value = item
                },
                fetch: function (text, callback) {
                    var match = text.toLowerCase();
                    callback(field.enum.filter(function(n) { return n.toLowerCase().startsWith(match); }));
                },
                render: function(item, value) {
                    var itemElement = document.createElement("div");
                    if (allowedChars.test(value)) {
                        var regex = new RegExp(value, 'gi');
                        var inner = item.replace(regex, function(match) { return "<strong>" + match + "</strong>" });
                        itemElement.innerHTML = inner;
                    } else {
                        itemElement.textContent = item;
                    }
                    return itemElement;
                },
                //emptyMsg: "No  found",
                customize: function(input, inputRect, container, maxHeight) {
                    if (maxHeight < 100) {
                        container.style.top = "";
                        container.style.bottom = (window.innerHeight - inputRect.bottom + input.offsetHeight) + "px";
                        container.style.maxHeight = "140px";
                    }
                }
            });
        }

        return editor;
    }

    setData(data) {
        this._data = data;
        if (this._fields) {
            for (let i=0; i<this._fields.length; i++)
                this.setFieldValue(i);
        } 
    }

    setFields(fields) {
        this._fields = fields;
        this.renderView();
    }

    getData() {
        return this._data;
    }

    setFieldValue(idx) {
        let field = this._fields[idx];
        
        if (field.gridView) {
            field.gridView.setData(this._data[field.name]);
        } else {        
            let fldElement = document.getElementById("fld" + field.name);
            if (fldElement) {
                let fldData = this._data[field.name];
                if (field.type == "boolean")
                    fldElement.checked = !(!fldData);
                else {
                    if (!fldData)
                        fldData = "";
                    fldElement.value = fldData;
                }
            }
        }
    }

    onFieldChanged(idx) {
        let field = this._fields[idx];
        let editor = document.getElementById("fld" + field.name);
        let value;
        if (field.type == "boolean")
            value = editor.checked;
        else {
            value = editor.value;
            if (field.type == "number") {
                if (!isNaN(value))
                    value = parseInt(value);
            }
        }

        this._data[field.name] = value;
        if (this.fieldChanged)
            this.fieldChanged(idx);
    }

}