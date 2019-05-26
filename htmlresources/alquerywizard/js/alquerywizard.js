class QueryWizard extends TableBasedObjectWizard {

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
        $("#querytype").val(this._data.queryType);
        $("#apipublisher").val(this._data.apiPublisher);
        $("#apigroup").val(this._data.apiGroup);
        $("#apiversion").val(this._data.apiVersion);
        $("#entityname").val(this._data.entityName);
        $("#entitysetname").val(this._data.entitySetName);   
        
        this.updateControls();
        this.loadTables();
        this.loadFields();
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
                selectedTable : this._data.selectedTable,
                fields: this._data.selectedFieldList,
                queryType : this._data.queryType,
                apiPublisher: this._data.apiPublisher,
                apiGroup: this._data.apiGroup,
                apiVersion: this._data.apiVersion,
                entityName: this._data.entityName,
                entitySetName: this._data.entitySetName    
            }
        });
    }

    collectStepData(finishSelected) {
        switch (this._step) {
            case 1: this.collectStep1Data(finishSelected);
            case 2: this.collectStep2Data(finishSelected);
        }
    }

    collectStep1Data(finishSelected) {
        var prevTableName = this._data.selectedTable;        
        this._data.objectId = $("#objectid").val();
        this._data.objectName = $("#objectname").val();
        this._data.selectedTable = $("#srctable").val();
        this._data.queryType = $("#querytype").val();
        this._data.apiPublisher = $("#apipublisher").val();
        this._data.apiGroup = $("#apigroup").val();
        this._data.apiVersion = $("#apiversion").val();
        this._data.entityName = $("#entityname").val();
        this._data.entitySetName = $("#entitysetname").val();    

        if (prevTableName != this._data.selectedTable) {
            $("#srcfields").html("");
            $("#destfields").html("");     

            if (!finishSelected)
                this.sendMessage({
                    command: 'selectTable',
                    tableName: $('#srctable').val()
                });    
        }
    }

    collectStep2Data(finishSelected) {
        this._data.selectedFieldList = this.getSelectedFields();
    }

    onQueryTypeChanged() {
        this._data.queryType = $("#querytype").val();
        this.updateControls();
    }

    updateControls() {
        if (this._data.queryType == "API") {
            $("#apipublisherline").show();
            $("#apigroupline").show();
            $("#apiversionline").show();
            $("#entitynameline").show();
            $("#entitysetnameline").show();        
        } else {
            $("#apipublisherline").hide();
            $("#apigroupline").hide();
            $("#apiversionline").hide();
            $("#entitynameline").hide();
            $("#entitysetnameline").hide();        
        }
    }

}

var wizard;

$(function() {
    wizard = new QueryWizard();
});
