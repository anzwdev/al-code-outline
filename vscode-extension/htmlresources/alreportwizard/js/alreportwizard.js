class ReportWizard extends TableBasedObjectWizard {

    constructor() {
        super(2, false);

        this.registerFieldsSelectionEvents();
    }

    setData(data) {
        super.setData(data);

        //initialize fields
        this.updateObjectIdControl();
        document.getElementById("objectname").value = this._data.objectName;
        document.getElementById("srctable").value = this._data.selectedTable?.name ?? "";
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
        this.selectTableByName(document.getElementById("srctable").value);
        this.saveObjectIdControl();
        this._data.objectName = document.getElementById("objectname").value;
        this._data.applicationArea = document.getElementById("apparea").value;
        this._data.usageCategory = document.getElementById("usagecat").value;
        
        if (this._selectedTableChanged) {
            this._selectedTableChanged = false;

            htmlHelper.clearChildrenById("srcfields");
            htmlHelper.clearChildrenById("destfields");
            this.sendMessage({
                command: 'selectTable',
                selectedTable: this._data.selectedTable
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
