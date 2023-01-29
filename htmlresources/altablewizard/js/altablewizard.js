class TableWizard extends BaseObjectWizard {

    constructor() {
        super(1);

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
            default:
                super.onMessage(message);
                break;
        }
    }

    setData(data) {
        super.setData(data);
       
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

    sendFinishMessage() {
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

    collectStepData(finishSelected) {
        this._data.objectId = document.getElementById("objectid").value;
        this._data.objectName = document.getElementById("objectname").value;
        this._data.dataPerCompany = document.getElementById("datapercompany").checked;
        this._data.fields = this._fieldsgrid.getData();
    }
}

var wizard;

window.onload = function() {
    wizard = new TableWizard();
};
