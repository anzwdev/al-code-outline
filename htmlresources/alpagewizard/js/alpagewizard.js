class PageWizard extends TableBasedObjectWizard {

    constructor() {
        super(2);

        //initialize steps visibility
        this._step = 1;
        this._activeFastTab = 0;
        htmlHelper.hideById("wizardstep2");

        this.registerFieldsSelectionEvents();

        document.getElementById('pagetype').addEventListener('change', event => {
            this.onPageTypeChanged();
        });
        document.getElementById('activefasttab').addEventListener('change', event => {
            this.onActiveFastTabChanged();
        });

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
        document.getElementById("pagetype").value = this._data.pageType;
        document.getElementById("fasttabs").value = this._data.fastTabs;
        document.getElementById("apparea").value = this._data.applicationArea;
        document.getElementById("usagecat").value = this._data.usageCategory;

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
                pageType : this._data.pageType,
                fastTabs : this._data.fastTabs,
                applicationArea : this._data.applicationArea,
                usageCategory : this._data.usageCategory,
                fields: this._data.selectedFieldList,
                fastTabsData: this._data.fastTabsData,
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
        var prevFastTab = this._data.fastTabs;
        
        this._data.objectId = document.getElementById("objectid").value;
        this._data.objectName = document.getElementById("objectname").value;
        this._data.selectedTable = document.getElementById("srctable").value;
        this._data.pageType = document.getElementById("pagetype").value;
        this._data.fastTabs = document.getElementById("fasttabs").value;
        this._data.applicationArea = document.getElementById("apparea").value;
        this._data.usageCategory = document.getElementById("usagecat").value;
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
                    tableName: this._data.selectedTable
                });    
        }

        if ((prevFastTab != this._data.fastTabs) || (!this._data.fastTabsData) || (this._data.fastTabsData.length == 0)) {
            this.rebuildFastTabs();
        }

    }

    collectStep2Data(finishSelected) {
        this.saveSelectedFields();
    }


    onPageTypeChanged() {
        let prevHasTabs = this.hasFastTabs();        
        this._data.pageType = document.getElementById("pagetype").value;
        this.updateControls();
        if (prevHasTabs != this.hasFastTabs())
            this.rebuildFastTabs();
    }

    updateControls() {
        if (this.hasFastTabs())
            htmlHelper.showById("fasttabsline");
        else
            htmlHelper.hideById("fasttabsline");            
        
        if (this._data.pageType == "List")
            htmlHelper.showById("usagecatline");
        else
            htmlHelper.hideById("usagecatline");

        if (this._data.pageType == "API") {
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

    hasFastTabs() {
        return ((this._data.pageType == "Card") || (this._data.pageType == "Document") || (this._data.pageType == "CardPart") ||
            (this._data.pageType == "ConfirmationDialog") || (this._data.pageType == "NavigatePage"));
    }

    hasFields () {
        return ((this._data.pageType != "RoleCenter") && (this._data.pageType != "StandardDialog"));
    }

    saveSelectedFields() {
        if (this.hasFastTabs()) {
            var fastTabIndex = this._activeFastTab;            
            if ((this._data.fastTabsData) && (fastTabIndex >= 0) && (fastTabIndex < this._data.fastTabsData.length)) {
                this._data.fastTabsData[fastTabIndex].fields = this.getSelectedFields(); 
            }
        } else {
            this._data.selectedFieldList = this.getSelectedFields();            
        }
    }

    restoreSelectedFields() {
        if (this.hasFastTabs()) {
            var fastTabIndex = this._activeFastTab;            
            if ((this._data.fastTabsData) && (fastTabIndex >= 0) && (fastTabIndex < this._data.fastTabsData.length)) {
                this.setSelectedFields(this._data.fastTabsData[fastTabIndex].fields);
            }
        } else {
            this.setSelectedFields(this._data.selectedFieldList);            
        }
    }

    selectFastTab(idx) {       
        this.saveSelectedFields();
        this._activeFastTab = parseInt(idx);
        this.restoreSelectedFields();
    }

    onActiveFastTabChanged() {
        this.selectFastTab(document.getElementById("activefasttab").value);
    }

    rebuildFastTabs() {
        if (this.hasFastTabs()) {
            //get list of fast tabs
            if ((!this._data.fastTabs) || (this._data.fastTabs.trim() == ""))
                this._data.fastTabs = "General";
            var fastTabsNames = this._data.fastTabs.split(",");
            var fastTabList = [];
            for (var i=0; i<fastTabsNames.length; i++) {
                fastTabList.push({
                    name : fastTabsNames[i],
                    fields : []
                })
            }
            this._data.fastTabsData = fastTabList;

            //build fast tabs selector
            this._activeFastTab = 0;
            htmlHelper.setElementOptions(document.getElementById("activefasttab"), fastTabsNames, true);            
            document.getElementById("activefasttab").value = this._activeFastTab;
            htmlHelper.showById("activefasttabline");
        } else {
            this._data.fastTabsData = [];
            this._activeFastTab = 0;
            htmlHelper.hideById("activefasttabline");
        }
        //reload fields
        this.loadFields();
    }

}

var wizard;

window.onload = function() {
    wizard = new PageWizard();
};
