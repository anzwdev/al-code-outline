class QueryWizard {

    constructor() {
        //initialize steps visibility
        this._step = 1;
        $("#wizardstep2").hide();

        //initialize properties
        this._vscode = acquireVsCodeApi();
       
        // Handle messages sent from the extension to the webview
        var me = this;
        window.addEventListener('message', event => {
            me.onMessage(event.data);
        });

        this.sendMessage({
            command: 'documentLoaded'
        });
        
    }

    onMessage(message) {     
        switch (message.command) {
            case 'setData':
                this.setData(message.data);
                break;
            case 'setTables':
                this.setTables(message.data);
                break;
            case 'setFields':
                this.setFields(message.data);
                break;
        }
    }

    sendMessage(data) {
        this._vscode.postMessage(data);    
    }

    setStep(newStep) {
        $("#wizardstep" + this._step.toString()).hide();        
        this._step = newStep;
        $("#wizardstep" + this._step.toString()).show();
        $("#prevBtn").prop("disabled", (this._step <= 1));
        $("#nextBtn").prop("disabled", (this._step == 2));
        $("#finishBtn").prop("disabled", (this._step < 2));
    }

    setData(data) {
        this._data = data;
        //initialize fields
        $("#objectid").val(this._data.objectId);
        $("#objectname").val(this._data.objectName);
        $("#srctable").val(this._data.selectedTable);
        $("#querytype").val(this._data.queryType);
        $("#apipublisher").val(this._data.apiPublisher);
        $("#apigroup").val(this._data.apiGroup);
        $("#apiversion").val(this._data.apiVersion);
        $("#entityname").val(this._data.entityName);
        $("#entitysetname").val(this._data.entitySetName);   
        
        this.updateControls();
        this.loadTables();
        this.loadFields();
        this.loadSelectedFields();
    }

    loadFields() {
        if (this._data)
            wizardHelper.setElementOptions("#srcfields", this._data.fieldList, false);
        else
            $("#srcfields").html("");
        $("#destfields").html("");
    }

    loadTables() {
        if (this._data)        
            this.initAutoComplete()
    }

    initAutoComplete() {
        let me = this;
        let allowedChars = new RegExp(/^[a-zA-Z\s]+$/)

        autocomplete({
			input: document.getElementById('srctable'),
			minLength: 1,
			onSelect: function (item, inputfield) {
				inputfield.value = item
			},
			fetch: function (text, callback) {
				var match = text.toLowerCase();
				callback(me._data.tableList.filter(function(n) { return n.toLowerCase().indexOf(match) !== -1; }));
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
			emptyMsg: "No fields found",
			customize: function(input, inputRect, container, maxHeight) {
				if (maxHeight < 100) {
					container.style.top = "";
					container.style.bottom = (window.innerHeight - inputRect.bottom + input.offsetHeight) + "px";
					container.style.maxHeight = "140px";
				}
			}
		})
    }
    
    loadSelectedFields() {
        $("#destfields").html("");
    }

    setFields(data) {
        if (!this._data)
            this.data = {};
        this._data.fieldList = data;
        this.loadFields();
    }

    setTables(data) {
        if (!this._data)
            this._data = {};
        this._data.tableList = data;
        this.loadTables();
    }
    
    onFinish() {
        this.collectStepData(true);
        this.sendMessage({
            command: "finishClick",
            data: {
                objectId : this._data.objectId,
                objectName : this._data.objectName,
                selectedTable : this._data.selectedTable,
                fields: this._data.selectedFieldList,
                queryType : this._data.queryType,
                apiPublisher: this._data.apiPublisher,
                apiGroup: this._data.apiGroup,
                apiVersion: this._data.apiVersion,
                entityName: this._data.entityName,
                entitySetName: this._data.entitySetName    
            }
        });
    }

    onCancel() {
        this.sendMessage({
            command : "cancelClick"
        })
    }

    onPrev() {
        if (this._step > 1) {
            this.collectStepData(false);
            this.setStep(this._step - 1);
        }
    }

    onNext() {
        if (this._step < 2) {
            this.collectStepData(false);
            this.setStep(this._step + 1);
        }
    }

    collectStepData(finishSelected) {
        switch (this._step) {
            case 1: this.collectStep1Data(finishSelected);
            case 2: this.collectStep2Data(finishSelected);
        }
    }

    collectStep1Data(finishSelected) {
        var prevTableName = this._data.selectedTable;        
        this._data.objectId = $("#objectid").val();
        this._data.objectName = $("#objectname").val();
        this._data.selectedTable = $("#srctable").val();
        this._data.queryType = $("#querytype").val();
        this._data.apiPublisher = $("#apipublisher").val();
        this._data.apiGroup = $("#apigroup").val();
        this._data.apiVersion = $("#apiversion").val();
        this._data.entityName = $("#entityname").val();
        this._data.entitySetName = $("#entitysetname").val();    

        if (prevTableName != this._data.selectedTable) {
            $("#srcfields").html("");
            $("#destfields").html("");     

            if (!finishSelected)
                this.sendMessage({
                    command: 'selectTable',
                    tableName: $('#srctable').val()
                });    
        }
    }

    collectStep2Data(finishSelected) {
        this._data.selectedFieldList = this.getSelectedFields();
    }

    onMoveFieldsRight() {
        wizardHelper.moveSelectedOptions("#srcfields", "#destfields");
    }

    onMoveFieldsLeft() {
        wizardHelper.moveSelectedOptions("#destfields", "#srcfields");
    }

    onMoveAllRight() {
        wizardHelper.moveAllOptions("#srcfields", "#destfields");
    }

    onMoveAllLeft() {
        wizardHelper.moveAllOptions("#destfields", "#srcfields");
    }

    getSelectedFields() {
        return wizardHelper.getAllElementOptions("#destfields");
    }

    setSelectedFields(list) {
        wizardHelper.setElementOptions("#destfields", list, false);
    }

    onQueryTypeChanged() {
        this._data.queryType = $("#querytype").val();
        this.updateControls();
    }

    updateControls() {
        if (this._data.queryType == "API") {
            $("#apipublisherline").show();
            $("#apigroupline").show();
            $("#apiversionline").show();
            $("#entitynameline").show();
            $("#entitysetnameline").show();        
        } else {
            $("#apipublisherline").hide();
            $("#apigroupline").hide();
            $("#apiversionline").hide();
            $("#entitynameline").hide();
            $("#entitysetnameline").hide();        
        }
    }

}

var wizard;

$(function() {
    wizard = new QueryWizard();
});
