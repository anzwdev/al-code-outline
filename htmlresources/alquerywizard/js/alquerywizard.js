class QueryWizard extends TableBasedObjectWizard {

    constructor() {
        super(2, false);

        this.registerFieldsSelectionEvents();

        document.getElementById('querytype').addEventListener('change', event => {
            this.onQueryTypeChanged();
        });

        //initialize steps visibility
        this._step = 1;
        htmlHelper.hideById("wizardstep2");

    }

    updateMainButtons() {
        document.getElementById("prevBtn").disabled = (this._step <= 1);
        document.getElementById("nextBtn").disabled = (this._step == 2);
        //document.getElementById("finishBtn").disabled = (this._step < 2);
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
        document.getElementById("querytype").value = this._data.queryType;
        document.getElementById("apipublisher").value = this._data.apiPublisher;
        document.getElementById("apigroup").value = this._data.apiGroup;
        document.getElementById("apiversion").value = this._data.apiVersion;
        document.getElementById("entityname").value = this._data.entityName;
        document.getElementById("entitysetname").value = this._data.entitySetName;   
        
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
        this._data.objectId = document.getElementById("objectid").value;
        this._data.objectName = document.getElementById("objectname").value;
        this._data.selectedTable = document.getElementById("srctable").value;
        this._data.queryType = document.getElementById("querytype").value;
        this._data.apiPublisher = document.getElementById("apipublisher").value;
        this._data.apiGroup = document.getElementById("apigroup").value;
        this._data.apiVersion = document.getElementById("apiversion").value;
        this._data.entityName = document.getElementById("entityname").value;
        this._data.entitySetName = document.getElementById("entitysetname").value;    

        if (prevTableName != this._data.selectedTable) {
            htmlHelper.clearChildrenById("srcfields");
            htmlHelper.clearChildrenById("destfields");

            if (!finishSelected)
                this.sendMessage({
                    command: 'selectTable',
                    tableName: document.getElementById('srctable').value
                });    
        }
    }

    collectStep2Data(finishSelected) {
        this._data.selectedFieldList = this.getSelectedFields();
    }

    onQueryTypeChanged() {
        this._data.queryType = document.getElementById("querytype").value;
        this.updateControls();
    }

    updateControls() {
        if (this._data.queryType == "API") {
            htmlHelper.showById("apipublisherline");
            htmlHelper.showById("apigroupline");
            htmlHelper.showById("apiversionline");
            htmlHelper.showById("entitynameline");
            htmlHelper.showById("entitysetnameline");        
        } else {
            htmlHelper.hideById("apipublisherline");
            htmlHelper.hideById("apigroupline");
            htmlHelper.hideById("apiversionline");
            htmlHelper.hideById("entitynameline");
            htmlHelper.hideById("entitysetnameline");
        }
        this.updateMainButtons();
    }

}

var wizard;

window.onload = function() {
    wizard = new QueryWizard();
};
