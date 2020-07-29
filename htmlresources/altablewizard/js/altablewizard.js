class TableWizard {

    constructor() {
        //initialize properties
        this._step = 1;
        this._vscode = acquireVsCodeApi();

        //initialize controls
        this._fieldsgrid = new TableFieldsGridView(true);
        this._fieldsgrid.onCreateDataEntry = (data, idx, item) => {
            if ((idx == 0) && (data.length == 0))
                item.pk = true;
        };

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
            case 'setTypes':
                this.setTypes(message.data);
                break;
        }
    }

    sendMessage(data) {
        this._vscode.postMessage(data);    
    }

    setData(data) {
        this._data = data;        
       
        if (this._data) {
            //initialize inputs
            document.getElementById("objectid").value = this._data.objectId;
            document.getElementById("objectname").value = this._data.objectName;
            //initialize fields list
            if (this._data.fields)
                this._fieldsgrid.setData(this._data.fields);
            else
                this._fieldsgrid.setData([]);
        }

    }
   
    setTypes(types) {
        this._fieldsgrid.setAutocomplete('dataType', types);
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
                fields: this._data.fields
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
        this._data.fields = this._fieldsgrid.getData();
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

}

var wizard;

window.onload = function() {
    wizard = new TableWizard();
};
