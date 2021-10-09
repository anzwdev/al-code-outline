class EnumWizard {

    constructor() {
        //initialize properties
        this._step = 1;
        this._vscode = acquireVsCodeApi();

        this.initNameLenUpdate();

        // Handle messages sent from the extension to the webview
        window.addEventListener('message', event => {
            this.onMessage(event.data);
        });

        document.getElementById('finishBtn').addEventListener('click', event => {
            this.onFinish();
        });

        document.getElementById('cancelBtn').addEventListener('click', event => {
            this.onCancel();
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
        document.getElementById("objectid").value = this._data.objectId;
        document.getElementById("objectname").value = this._data.objectName;
        document.getElementById("valuelist").value = this._data.valueList;
        document.getElementById("captionlist").value = this._data.captionList;
        document.getElementById("extensible").checked = this._data.extensible;
    }
   
    onFinish() {
        this.collectStepData(true);

        if (!this.canFinish())
            return;
            
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
        this._data.objectId = document.getElementById("objectid").value;
        this._data.objectName = document.getElementById("objectname").value;
        this._data.valueList = document.getElementById("valuelist").value;
        this._data.captionList = document.getElementById("captionlist").value;
        this._data.extensible = document.getElementById("extensible").checked;
    }

    canFinish() {
        if ((!this._data.objectName) || (this._data.objectName == '')) {
            this.sendMessage({
                command: 'showError',
                message: 'Please enter object name.'
            });
            return false;
        }
        return true;
    }

    initNameLenUpdate() {
        this._ctName = document.getElementById('objectname');
        this._ctNameLen = document.getElementById('objectnamelen');
        if ((this._ctName) && (this._ctNameLen)) {
            this.updateNameLen();
            this._ctName.addEventListener('input', event => {
                this.updateNameLen();
            });   
        }
    }

    updateNameLen() {
        this._ctNameLen.innerText = this._ctName.value.length.toString();
    }

}

var wizard;

window.onload = function() {
    wizard = new EnumWizard();
};
