class EnumWizard {

    constructor() {
        //initialize properties
        _step = 1;
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
        }
    }

    sendMessage(data) {
        this._vscode.postMessage(data);    
    }

    setData(data) {
        this._data = data;
        //initialize fields
        $("#objectid").val(this._data.objectId);
        $("#objectname").val(this._data.objectName);
        $("#valuelist").val(this._data.valueList);
        $("#captionlist").val(this._data.captionList);
        $("#extensible").prop('checked', this._data.extensible);
    }
   
    onFinish() {
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
    }

    onCancel() {
        this.sendMessage({
            command : "cancelClick"
        })
    }

    collectStepData(finishSelected) {
        this._data.objectId = $("#objectid").val();
        this._data.objectName = $("#objectname").val();
        this._data.valueList = $("#valuelist").val();
        this._data.captionList = $("#captionlist").val();
        this._data.extensible = $("#extensible").prop("checked");
    }

}

var wizard;

$(function() {
    wizard = new EnumWizard();
});
