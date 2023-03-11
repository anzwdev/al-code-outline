class ReportWizard extends TableBasedObjectWizard {

    constructor() {
        super(2, false);

        this.registerFieldsSelectionEvents();
    }

    setData(data) {
        super.setData(data);

        //initialize fields
        document.getElementById("objectid").value = this._data.objectId;
        document.getElementById("objectname").value = this._data.objectName;
        document.getElementById("srctable").value = this._data.selectedTable;
        document.getElementById("apparea").value = this._data.applicationArea;
        document.getElementById("usagecat").value = this._data.usageCategory;
        this.updateMainButtons();
        this.loadTables();
        this.loadFields();
    }

    sendFinishMessage() {
        this.sendMessage({
            command: "finishClick",
            data: {
                objectId : this._data.objectId,
                objectName : this._data.objectName,
                selectedTable : this._data.selectedTable,
                applicationArea : this._data.applicationArea,
                usageCategory : this._data.usageCategory,
                fields: this._data.selectedFieldList
            }
        });
    }

    collectStepData() {
        switch (this._step) {
            case 1: this.collectStep1Data();
            case 2: this.collectStep2Data();
        }
    }

    collectStep1Data() {
        var prevTableName = this._data.selectedTable;        
        this._data.objectId = document.getElementById("objectid").value;
        this._data.objectName = document.getElementById("objectname").value;
        this._data.selectedTable = document.getElementById("srctable").value;
        this._data.applicationArea = document.getElementById("apparea").value;
        this._data.usageCategory = document.getElementById("usagecat").value;
        
        if (prevTableName != this._data.selectedTable) {
            htmlHelper.clearChildrenById("srcfields");
            htmlHelper.clearChildrenById("destfields");
            this.sendMessage({
                command: 'selectTable',
                tableName: document.getElementById('srctable').value
            });    
        }
    }

    collectStep2Data() {
        this._data.selectedFieldList = this.getSelectedFields();
    }

}

var wizard;

window.onload = function() {
    wizard = new ReportWizard();
};
