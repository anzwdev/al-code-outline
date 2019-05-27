class ReportWizard extends TableBasedObjectWizard {

    constructor() {
        super(2);

        //initialize steps visibility
        this._step = 1;
        $("#wizardstep2").hide();

    }

    setStep(newStep) {
        $("#wizardstep" + this._step.toString()).hide();        
        this._step = newStep;
        $("#wizardstep" + this._step.toString()).show();
        $("#prevBtn").prop("disabled", (this._step <= 1));
        $("#nextBtn").prop("disabled", (this._step == 2));
        $("#finishBtn").prop("disabled", (this._step < 2));
    }

    setData(data) {
        super.setData(data);
        //initialize fields
        $("#objectid").val(this._data.objectId);
        $("#objectname").val(this._data.objectName);
        $("#srctable").val(this._data.selectedTable);
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
        this._data.objectId = $("#objectid").val();
        this._data.objectName = $("#objectname").val();
        this._data.selectedTable = $("#srctable").val();
        if (prevTableName != this._data.selectedTable) {
            $("#srcfields").html("");
            $("#destfields").html("");            
            this.sendMessage({
                command: 'selectTable',
                tableName: $('#srctable').val()
            });    
        }
    }

    collectStep2Data() {
        this._data.selectedFieldList = this.getSelectedFields();
    }

}

var wizard;

$(function() {
    wizard = new ReportWizard();
});
