class ReportWizard extends TableBasedObjectWizard {

    constructor() {
        super(2);

        this.registerFieldsSelectionEvents();

        //initialize steps visibility
        this._step = 1;
        htmlHelper.hideById("wizardstep2");

    }

    updateMainButtons() {
        document.getElementById("prevBtn").disabled = (this._step <= 1);
        document.getElementById("nextBtn").disabled = (this._step == 2);
        //document.getElementById("finishBtn").disabled", (this._step < 2);
    }

    setStep(newStep) {
        htmlHelper.hideById("wizardstep" + this._step.toString());        
        this._step = newStep;
        htmlHelper.showById("wizardstep" + this._step.toString());
        this.updateMainButtons();        
    }

    setData(data) {
        super.setData(data);
        //initialize fields
        document.getElementById("objectid").value = this._data.objectId;
        document.getElementById("objectname").value = this._data.objectName;
        document.getElementById("srctable").value = this._data.selectedTable;
        this.updateMainButtons();
        this.loadTables();
        this.loadFields();
    }

    onFinish() {
        this.collectStepData();

        if (!this.canFinish())
            return;
            
        this.sendMessage({
            command: "finishClick",
            data: {
                objectId : this._data.objectId,
                objectName : this._data.objectName,
                selectedTable : this._data.selectedTable,
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
