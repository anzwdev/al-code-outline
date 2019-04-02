var wizard = {
    _vscode : null,
    _data : null,
    _fields : null,
    _step : 1,

    initialize : function() {
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
            case 'setEnums':
                this.setEnums(message.data);
                break;
        }
    },

    sendMessage : function(data) {
        this._vscode.postMessage(data);    
    },

    setData : function(data) {
        this._data = data;
        //initialize fields
        $("#objectid").val(this._data.objectId);
        $("#objectname").val(this._data.objectName);
        $("#baseenum").val(this._data.baseEnum);
        $("#firstvalueid").val(this._data.firstValueId);
        $("#valuelist").val(this._data.valueList);
        $("#captionlist").val(this._data.captionList);
        this.loadEnums();
    },
   
    loadEnums : function() {
        if (this._data)
            wizardHelper.setElementOptions("#enumlist", this._data.baseEnumList, false);
        else
            $("#enumlist").html("");
    },

    setEnums : function(data) {
        if (!this._data)
            this._data = {};
        this._data.baseEnumList = data;
        this.loadEnums();
    },

    onFinish : function() {
        this.collectStepData(true);
        this.sendMessage({
            command: "finishClick",
            data: {
                objectId : this._data.objectId,
                objectName : this._data.objectName,
                baseEnum : this._data.baseEnum,
                firstValueId : this._data.firstValueId,
                valueList : this._data.valueList,
                captionList : this._data.captionList
            }
        });
    },

    onCancel : function() {
        this.sendMessage({
            command : "cancelClick"
        })
    },

    collectStepData : function(finishSelected) {
        this._data.objectId = $("#objectid").val();
        this._data.objectName = $("#objectname").val();
        this._data.baseEnum = $("#baseenum").val();
        this._data.firstValueId = $("#firstvalueid").val();
        this._data.valueList = $("#valuelist").val();
        this._data.captionList = $("#captionlist").val();
    }

}

$(function() {
    wizard.initialize();
});
