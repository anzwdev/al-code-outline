class TableWizard {

    constructor() {
        //initialize properties
        this._step = 1;
        this._vscode = acquireVsCodeApi();

        //initialize controls
        this._fieldsgrid = new AZGridView('fieldsgrid', 
            [{name: 'id', caption: 'Id', style: 'width: 80px;'},
            {name: 'name', caption: 'Name', style: 'width: 50%;'},
            //{name: 'caption', caption: 'Caption', style: 'width: 30%'},
            {name: 'dataType', caption: 'Data Type', style: 'width: 20%', autocomplete: [                
                'Blob', 'Boolean', 'Code', 'Date', 'DateFormula', 'DateTime', 'Decimal', 'Duration',
                'Enum', 'Guid', 'Integer', 'Media', 'MediaSet', 'Option', 'RecordId', 'TableFilter',
                'Text', 'Time']},
            {name: 'dataClassification', caption: 'Data Classification', style: 'width: 20%', autocomplete: [
                'AccountData', 'CustomerContent', 'EndUserIdentifiableInformation', 'EndUserPseudonymousIdentifiers',
                'OrganizationIdentifiableInformation', 'SystemMetadata', 'ToBeClassified']},
            {name: 'length', caption: 'Length', style: 'width:100px'}],
            undefined, undefined, 'Loading data...', true);

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
        this._fieldsgrid._columns[2].autocomplete = types;
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
