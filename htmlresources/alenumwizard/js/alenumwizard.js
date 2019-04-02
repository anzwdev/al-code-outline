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
        $("#valuelist").val(this._data.valueList);
        $("#captionlist").val(this._data.captionList);
        $("#extensible").prop('checked', this._data.extensible);
    },
   
    onFinish : function() {
        this.collectStepData(true);
        this.sendMessage({
            command: "finishClick",
            data: {
                objectId : this._data.objectId,
                objectName : this._data.objectName,
                valueList : this._data.valueList,
                captionList : this._data.captionList,
                extensible : this._data.extensible
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
        this._data.valueList = $("#valuelist").val();
        this._data.captionList = $("#captionlist").val();
        this._data.extensible = $("#extensible").prop("checked");
    }

}

$(function() {
    wizard.initialize();
});
