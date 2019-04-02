var wizard = {
    _vscode : null,
    _data : null,
    _fields : null,
    _step : 1,

    initialize : function() {
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
        
    },

    onMessage : function(message) {     
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
    },

    sendMessage : function(data) {
        this._vscode.postMessage(data);    
    },

    setStep : function(newStep) {
        $("#wizardstep" + this._step.toString()).hide();        
        this._step = newStep;
        $("#wizardstep" + this._step.toString()).show();
        $("#prevBtn").prop("disabled", (this._step <= 1));
        $("#nextBtn").prop("disabled", (this._step == 2));
        $("#finishBtn").prop("disabled", (this._step < 2));
    },

    setData : function(data) {
        this._data = data;
        //initialize fields
        $("#objectid").val(this._data.objectId);
        $("#objectname").val(this._data.objectName);
        $("#srctable").val(this._data.selectedTable);
        this.loadTables();
        this.loadFields();
        this.loadSelectedFields();
    },

    loadFields : function() {
        if (this._data)
            wizardHelper.setElementOptions("#srcfields", this._data.fieldList, false);
        else
            $("#srcfields").html("");
        $("#destfields").html("");
    },

    loadTables : function() {
        if (this._data)
            wizardHelper.setElementOptions("#tablelist", this._data.tableList, false);
        else
            $("#tablelist").html("");
    },

    loadSelectedFields : function() {
        $("#destfields").html("");
    },

    setFields : function(data) {
        if (!this._data)
            this.data = {};
        this._data.fieldList = data;
        this.loadFields();
    },

    setTables : function(data) {
        if (!this._data)
            this._data = {};
        this._data.tableList = data;
        this.loadTables();
    },
    
    onFinish : function() {
        this.collectStepData();
        this.sendMessage({
            command: "finishClick",
            data: {
                objectId : this._data.objectId,
                objectName : this._data.objectName,
                selectedTable : this._data.selectedTable,
                fields: this._data.selectedFieldList
            }
        });
    },

    onCancel : function() {
        this.sendMessage({
            command : "cancelClick"
        })
    },

    onPrev : function() {
        if (this._step > 1) {
            this.collectStepData();
            this.setStep(this._step - 1);
        }
    },

    onNext: function() {
        if (this._step < 2) {
            this.collectStepData();
            this.setStep(this._step + 1);
        }
    },

    collectStepData : function() {
        switch (this._step) {
            case 1: this.collectStep1Data();
            case 2: this.collectStep2Data();
        }
    },

    collectStep1Data : function() {
        var prevTableName = this._data.selectedTable;        
        this._data.objectId = $("#objectid").val();
        this._data.objectName = $("#objectname").val();
        this._data.selectedTable = $("#srctable").val();
        if (prevTableName != this._data.selectedTable) {
            $("#srcfields").html("");
            $("#destfields").html("");            
            this.sendMessage({
                command: 'selectTable',
                tableName: $('#srctable').val()
            });    
        }
    },

    collectStep2Data : function() {
        this._data.selectedFieldList = this.getSelectedFields();
    },

    onMoveFieldsRight : function() {
        wizardHelper.moveSelectedOptions("#srcfields", "#destfields");
    },

    onMoveFieldsLeft : function() {
        wizardHelper.moveSelectedOptions("#destfields", "#srcfields");
    },

    onMoveAllRight : function() {
        wizardHelper.moveAllOptions("#srcfields", "#destfields");
    },

    onMoveAllLeft : function() {
        wizardHelper.moveAllOptions("#destfields", "#srcfields");
    },

    getSelectedFields : function() {
        return wizardHelper.getAllElementOptions("#destfields");
    },

    setSelectedFields : function(list) {
        wizardHelper.setElementOptions("#destfields", list, false);
    }

}

$(function() {
    wizard.initialize();
});
