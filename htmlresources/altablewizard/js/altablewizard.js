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

            let prevId = this.getId(data, idx - 1); 
            let newId = prevId;
            if (idx < data.length) {
                newId = Math.round((prevId + this.getId(data, idx)) / 2);
                if (newId == prevId)
                    newId++;
            } else
                newId += 1;

            item.id = newId.toString();
        };

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

    getId(data, idx) {
        if ((idx >=0) && (idx < data.length) && (data[idx].id)) {
            let val = Number.parseInt(data[idx].id);
            if (!isNaN(val))
                return val;
        }
        return 0;
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
            document.getElementById("datapercompany").checked = this._data.dataPerCompany;
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
                dataPerCompany : this._data.dataPerCompany,
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
        this._data.dataPerCompany = document.getElementById("datapercompany").checked;
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
    wizard = new TableWizard();
};
