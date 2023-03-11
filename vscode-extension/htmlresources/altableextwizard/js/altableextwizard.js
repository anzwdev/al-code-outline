class TableExtWizard extends TableBasedObjectWizard {

    constructor() {
        super(1, false);

        this._fieldsgrid = new TableFieldsGridView(false);

        this._fieldsgrid.onCreateDataEntry = (data, idx, item) => {
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

    onMessage(message) {    
        super.onMessage(message); 

        switch (message.command) {
            case 'setTypes':
                this.setTypes(message.data);
                break;
        }
    }

    setTables(data) {
        super.setTables(data);
        this.sendMessage({
            command : "loadTypes"
        });
    }

    setData(data) {
        super.setData(data);

        //initialize inputs
        document.getElementById("objectid").value = this._data.objectId;
        document.getElementById("objectname").value = this._data.objectName;
        document.getElementById("srctable").value = this._data.selectedTable;
        //initialize field list
        if (this._data.fields) {
            this._fieldsgrid.setData(this._data.fields);
        }
        else {
            this._fieldsgrid.setData([]);
        }
        this.loadTables();
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
                selectedTable : this._data.selectedTable,
                fields: this._data.fields
            }
        });
    }

    collectStepData(finishSelected) {
        this._data.objectId = document.getElementById("objectid").value;
        this._data.objectName = document.getElementById("objectname").value;
        this._data.selectedTable = document.getElementById("srctable").value;
        this._data.fields = this._fieldsgrid.getData();
    }

    canFinish() {
        if (!super.canFinish) {
            return false;
        }

        if ((!this._data.selectedTable) || (this._data.selectedTable == '')) {
            this.sendMessage({
                command: 'showError',
                message: 'Please enter a target object name.'
            });
            return false;
        }
        return true;
    }

    getId(data, idx) {
        if ((idx >=0) && (idx < data.length) && (data[idx].id)) {
            let val = Number.parseInt(data[idx].id);
            if (!isNaN(val))
                return val;
        }

        if (this._data.idRangeStart) {
            let val = Number.parseInt(this._data.idRangeStart);
            if (!isNaN(val))
                return val - 1;
        }

        return 0;
    }

}

var wizard;

window.onload = function() {
    wizard = new TableExtWizard();
};
