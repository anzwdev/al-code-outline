class XmlPortWizard extends TableBasedObjectWizard {

    constructor() {
        super(2, false);

        this.registerFieldsSelectionEvents();
    }

    setData(data) {
        super.setData(data);

        //initialize fields
        this.updateObjectIdControl();
        document.getElementById("objectname").value = this._data.objectName?.name ?? "";
        document.getElementById("srctable").value = this._data.selectedTable;
        document.getElementById("fieldsas").value = this._data.fieldNodeType;
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
                fieldNodeType : this._data.fieldNodeType,
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
        this._data.fieldNodeType = document.getElementById("fieldsas").value;

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
    wizard = new XmlPortWizard();
};
