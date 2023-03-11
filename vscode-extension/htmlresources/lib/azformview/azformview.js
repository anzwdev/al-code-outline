class AZFormView {

    constructor(elementId, fields) {
        this._elementId = elementId;
        this._fields = fields;
        this._mainElement = document.getElementById(elementId);
        this._tabView = new AZTabView(elementId);
        this._tabView.tabContentFocused = (idx => {
            this.onTabContentFocused(idx);
        });

        this.renderView();
    }

    show() {
        this._tabView.show();
    }

    hide() {
        this._tabView.hide();
    }

    renderView() {
        if (!this._fields)
            return;

        this._gridViews = {};

        for (let i=0; i<this._fields.length; i++)
            this.renderField(i);
        this._tabView.selectTab(0);
        this.onTabContentFocused(0);
    }

    renderField(idx) {
        let field = this._fields[idx];
        
        if (!field.type) 
            return;

        if (field.type == "group") {
            this._tabView.addTab(field.caption);
            field.tabIndex = this._tabView.noOfTabs() - 1;
            return;
        }

        field.tabIndex = this._tabView.noOfTabs() - 1;
        let lineElement = document.createElement("div");
        lineElement.className = "formline";
       
        let captionElement = document.createElement("div");
        captionElement.className = "formcaption";
        captionElement.innerText = field.caption;

        //tooltip
        if ((field.description) && (field.description != "")) {
            let tooltip = document.createElement("span");
            tooltip.className = "tooltiptext";
            tooltip.innerText = field.description;
            captionElement.appendChild(tooltip);
        }

        lineElement.appendChild(captionElement);

        let fieldElement = document.createElement("div");
        fieldElement.className = "formfield";
            
        if (field.type == "string") {
            this.createEditorField(idx, field, "text");
            fieldElement.appendChild(field.editor);
        } else if (field.type == "longstring") {
            this.createEditorField(idx, field, "textarea");
            fieldElement.appendChild(field.editor);
        } else if (field.type == "boolean") {
            this.createEditorField(idx, field, "checkbox");
            fieldElement.appendChild(field.editor);
        }
        lineElement.appendChild(fieldElement);

        this._tabView.appendContentChild(lineElement);
        
        if ((field.type == "array") || (field.type == "list")) {
            let isList = (field.type == "list"); 
            
            if (isList) {
                field.inline = true;
                field.fields = [{
                    autocomplete: field.autocomplete
                }]
            };

            if (field.fields) {
                if (!field.inline) {
                    lineElement = document.createElement("div");
                    lineElement.className = "formline";        
                }
                       
                field.editor = document.createElement("div");
                field.editor.className = "azgridviewcont";
                field.editor.id = "fld" + field.name;

                if (field.inline)
                    fieldElement.appendChild(field.editor);
                else {
                    lineElement.appendChild(field.editor);                
                    this._tabView.appendContentChild(lineElement);
                }

                field.gridView = new AZGridView(field.editor.id, field.fields,
                    undefined, undefined, "", true);
                field.gridView.saveOnInput = true;
                field.gridView.cellChanged = ((rowIndex, fieldName, value) => {
                    this.onFieldChanged(idx);
                });
                field.gridView.rowsDeleted = (() => {
                    this.onFieldChanged(idx);
                });
                field.gridView.prevElementSelected = (() => {
                    this.changeFieldFocus(idx, idx - 1);
                });
                field.gridView.nextElementSelected = (() => {
                    this.changeFieldFocus(idx, idx + 1);
                });

            }
        }
    }

    createEditorField(idx, field, type) {
        let editor;
        
        if (type == "textarea") {
            editor = document.createElement("textarea");
            editor.className = "fulltext";
        } else {        
            editor = document.createElement("input");
            editor.type = type;
            if (type == "checkbox")
                editor.className = "checkbox";
            else
                editor.className = "fulltext";
        }
        editor.id = "fld" + field.name;
        editor.tabIndex = 100 + idx;

        if (type == "text")
            editor.addEventListener('input', event => {
                this.onFieldChanged(idx);
            });
        else if (type == "textarea")
            editor.addEventListener('input', event => {
                this.updateTextAreaSize(idx);
                this.onFieldChanged(idx);
            });
        else
            editor.addEventListener('change', event => {
                this.onFieldChanged(idx);
            });
        editor.addEventListener('keydown', event => {
            this.onKeyDown(event, idx); 
        });            

        field.editor = editor;

        if (field.autocomplete)
            this.initEditorAutocomplete(field);

        return editor;
    }

    initEditorAutocomplete(field) {
        let me = this;
        let allowedChars = new RegExp(/^[a-zA-Z\s]+$/);
        field.autocompleteControl = autocomplete({
            input: field.editor,
            disableOpenOnKeyDown: true,
            minLength: -1,
            onSelect: function (item, inputfield) {
                if (item.value)
                    inputfield.value = item.value;
                else
                    inputfield.value = item;
            },
            fetch: function (text, callback) {
                var match = text.toLowerCase();
                callback(field.autocomplete.filter(function(n) { 
                    if (n.value)
                        return n.value.toLowerCase().startsWith(match);
                    return n.toLowerCase().startsWith(match); 
                }));
            },
            render: function(item, value) {
                let itemElement = document.createElement("div");
                let ival = item.value?item.value:item;
                let icont = ival;
                
                if (allowedChars.test(value)) {
                    var regex = new RegExp(value, 'gi');
                    ival = ival.replace(regex, function(match) { 
                        return "<strong>" + match + "</strong>" 
                    });
                };
                if (item.description)
                    ival = "<div>" + ival + "</div><div class=\"acpdesc\">" + item.description + "</div>";
                itemElement.innerHTML = ival;
                return itemElement;
            },
            //emptyMsg: "No  found",
            customize: function(input, inputRect, container, maxHeight) {
                if (maxHeight < 100) {
                    container.style.top = "";
                    container.style.bottom = (window.innerHeight - inputRect.bottom + input.offsetHeight) + "px";
                    container.style.maxHeight = "140px";
                }
                if ((field.autocomplete.length > 0) && (field.autocomplete[0].description)) {
                    container.style.minWidth = input.width;
                    container.style.width = "";
                    container.style.maxWidth = (window.innerWidth - inputRect.left - 20) + "px";
                }
        }
        });
    }

    updateTextAreaSize(idx) {
        this._fields[idx].editor.style.height = (this._fields[idx].editor.scrollHeight - 4) + "px";
    }

    getFieldIndex(name) {
        if (this._fields) {
            for (let i=0; i<this._fields.length; i++)
                if (this._fields[i].name == name)
                    return i;
        }
        return -1;
    }

    setAutocomplete(path, data) {
        if (path.length > 0) {
            let idx = this.getFieldIndex(path[0]);
            if (idx >= 0) {
                let field = this._fields[idx];
                if ((field.type == "string") || (field.type == "longstring")) {
                    field.autocomplete = data;
                    this.initEditorAutocomplete(field);
                } else if ((field.gridView) && (path.length > 1)) {
                    field.gridView.setAutocomplete(path[1], data);
                }
            }
        }
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

            if (field.type == "longstring")
                this.updateTextAreaSize(idx);

        }
    }

    onFieldChanged(idx) {
        let field = this._fields[idx];
        let value;

        console.info('onFieldChanged ' + idx.toString())

        if (field.gridView) {
            value = field.gridView._data;
        } else {
            let editor = document.getElementById("fld" + field.name);
            if (field.type == "boolean")
                value = editor.checked;
            else {
                value = editor.value;
                if (field.type == "number") {
                    let num = parseInt(value);
                    if (!isNaN(value))
                        value = num;
                }
            }
        }

        this._data[field.name] = value;
        if (this.fieldChanged)
            this.fieldChanged(idx);
    }

    onKeyDown(e, idx) {
        let handled = false;
        switch (e.which) {
            case 38:    //up
                if ((this._fields[idx].type != "longstring") || (this._fields[idx].editor.selectionStart == 0)) {
                    if (e.ctrlKey)
                        this.focusTabs();
                    else
                        this.changeFieldFocus(idx, idx - 1);
                    handled = true;
                }
                break;
            case 40:    //down
                if ((this._fields[idx].type != "longstring") || (this._fields[idx].editor.selectionStart == this._fields[idx].editor.value.length)) {
                    this.changeFieldFocus(idx, idx + 1);
                    handled = true;
                }
                break;
        }
        if (handled) {
            e.preventDefault();
            e.stopPropagation();
            return false;
        }

        return true;
    }

    onTabContentFocused(idx) {
        for (let i=0; i<this._fields.length; i++) {
            if ((this._fields[i].tabIndex == idx) && (this._fields[i].type != "group")) {
                this.focusField(i);               
                return;
            }            
        }
    }

    changeFieldFocus(currIdx, newIdx) {
        if ((newIdx >= 0) && 
            (newIdx < this._fields.length) && 
            (this._fields[currIdx].tabIndex === this._fields[newIdx].tabIndex) &&
            (this._fields[newIdx].type != "group")) {
                this.focusField(newIdx);
            return;
        }

        //go up on first tab control - select tabs
        if (newIdx < currIdx)
            this.focusTabs();
    }

    focusField(idx) {
        if (this._fields[idx].gridView)
            this._fields[idx].gridView.focus();
        else if (this._fields[idx].editor)
            this._fields[idx].editor.focus();
    }

    focusTabs() {
        this._tabView.focus();
    }
 
}