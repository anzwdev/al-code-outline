class ReportExtWizard extends BaseObjectWizard {

    constructor() {
        super(2);
        this._srcFields = new FilteredList('srcfieldsfilter', 'srcfields');
        this._destFields = new FilteredList('destfieldsfilter', 'destfields');
        this._srcFields._captionMember = 'name';
        this._srcFields._sortByMember = 'name';
        this._destFields._captionMember = 'name';
        this.registerFieldsSelectionEvents();

        let fldSortName = document.getElementById('srcfldsortname');
        let fldSortId = document.getElementById('srcfldsortid');
        if (fldSortName)
            fldSortName.className = "mselcaptb mseltbbtnsel";
        if (fldSortId)
            fldSortId.addEventListener('click', event => {
                this.sortSrcFieldsBy("id");
            });
        if (fldSortName)
            fldSortName.addEventListener('click', event => {
                this.sortSrcFieldsBy("name");
            });
    }

    onMessage(message) {
        switch (message.command) {
            case 'setReports':
                this.setReports(message.data);
                break;
            case 'setBaseReport':
                this.setBaseReport(message.data);
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
            document.getElementById("basereport").value = this._data.baseReport;

            this.updateControls();
        }

    }

    setBaseReport(data) {
        if (!this._data)
            this._data = {};
        this._data.report = data;
        this.loadBaseReport();
    }

    setReports(data) {
        if (!this._data)
            this._data = {};
        this._data.reportList = data;
        this.loadReports();
    }

    loadReports() {
        if (this._data) {
            this.initReportAutoComplete();
        }
    }

    initReportAutoComplete() {
        let me = this;
        let allowedChars = new RegExp(/^[a-zA-Z\s]+$/);

        autocomplete({
			input: document.getElementById('basereport'),
			minLength: 1,
			onSelect: function (item, inputfield) {
				inputfield.value = item;
			},
			fetch: function (text, callback) {
				let match = text.toLowerCase();
				callback(me._data.reportList.filter(function(n) { return n.toLowerCase().indexOf(match) !== -1; }));
			},
			render: function(item, value) {
				let itemElement = document.createElement("div");
				if (allowedChars.test(value)) {
					let regex = new RegExp(value, 'gi');
					let inner = item.replace(regex, function(match) { return "<strong>" + match + "</strong>"; });
					itemElement.innerHTML = inner;
				} else {
					itemElement.textContent = item;
				}
				return itemElement;
			},
			emptyMsg: "No reports found",
			customize: function(input, inputRect, container, maxHeight) {
				if (maxHeight < 100) {
					container.style.top = "";
					container.style.bottom = (window.innerHeight - inputRect.bottom + input.offsetHeight) + "px";
					container.style.maxHeight = "140px";
				}
			}
		});
    }

    sendFinishMessage() {
        let dataItems = [];
        if ((this._data.report) && (this._data.report.dataItems)) {
            for (let i=0; i<this._data.report.dataItems.length; i++) {
                let selFields = this._data.report.dataItems[i].selFields;
                if ((selFields) && (selFields.length > 0))
                    dataItems.push({
                        name: this._data.report.dataItems[i].name,
                        fields: selFields
                    });
            }
        }

        this.sendMessage({
            command: "finishClick",
            data: {
                objectId : this._data.objectId,
                objectName : this._data.objectName,
                baseReport : this._data.baseReport,
                dataItems: dataItems
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
        let prevBaseReport = this._data.baseReport;

        this._data.objectId = document.getElementById("objectid").value;
        this._data.objectName = document.getElementById("objectname").value;
        this._data.baseReport = document.getElementById("basereport").value;

        if (prevBaseReport != this._data.baseReport) {
            htmlHelper.clearChildrenById("srcfields");
            htmlHelper.clearChildrenById("destfields");
            if (!finishSelected)
                this.sendMessage({
                    command: 'selectReport',
                    baseReport: this._data.baseReport
                });    
        }
    }

    collectStep2Data(finishSelected) {
        this.saveDataItemFields();
    }

    canFinish() {
        if (!super.canFinish())
            return false;

        if ((!this._data.baseReport) || (this._data.baseReport == '')) {
            this.sendMessage({
                command: 'showError',
                message: 'Please enter a target object name.'
            });
            return false;
        }
        return true;
    }

    registerFieldsSelectionEvents() {
        document.getElementById('mselright').addEventListener('click', event => {
            this.onMoveFieldsRight();
        });
        document.getElementById('mselallright').addEventListener('click', event => {
            this.onMoveAllRight();
        });      
        document.getElementById('mselallleft').addEventListener('click', event => {
            this.onMoveAllLeft();
        });
        document.getElementById('mselleft').addEventListener('click', event => {
            this.onMoveFieldsLeft();
        });      
        document.getElementById('srcfields').addEventListener('dblclick', event => {
            this.onMoveFieldsRight();
        });
        document.getElementById('destfields').addEventListener('dblclick', event => {
            this.onMoveFieldsLeft();
        });
        document.getElementById('activedataitem').addEventListener('change', event => {
            this.onActiveDataItemChanged();
        });
    }

    onMoveFieldsRight() {
        this._destFields.add(this._srcFields.removeSelected());
    }

    onMoveFieldsLeft() {
        this._srcFields.add(this._destFields.removeSelected());
    }

    onMoveAllRight() {
        this._destFields.add(this._srcFields.removeFiltered());
    }

    onMoveAllLeft() {
        this._srcFields.add(this._destFields.removeFiltered());
    }

    getSelectedFields() {
        return this._destFields.getAll();
    }

    loadBaseReport() {
        if (!this._data.report)
            return;

        this._data.dataItemNames = [];
        if (this._data.report.dataItems) {
            for (let i=0; i<this._data.report.dataItems.length; i++) {
                let diName = this.getIndentText(this._data.report.dataItems[i].indent) + this._data.report.dataItems[i].name;
                if (this._data.report.dataItems[i].source)
                    diName = diName + " (" + this._data.report.dataItems[i].source + ")";

                this._data.dataItemNames.push(diName);
                this._data.report.dataItems[i].selFields = [];
            }
        }

        this._activeDataItem = 0;
        let ctList = document.getElementById("activedataitem");
        htmlHelper.setElementOptions(ctList, this._data.dataItemNames, true);            
        if (this._data.dataItemNames.length > 0) {
            this.loadDataItemFields();
            ctList.value = this._activeDataItem;
        }
    }

    getIndentText(indent) {
        if ((indent) && (indent > 0))
            return "".padStart(indent, "-");
        return "";
    }    

    loadDataItemFields() {
        let dataItem = this.getCurrDataItem();
        if (dataItem) {
            this._srcFields.setData(dataItem.availableTableFields);
            this._destFields.setData(dataItem.selFields);
        } else {
            this._srcFields.clear();
            this._destFields.clear();
        }
    }

    saveDataItemFields() {
        let dataItem = this.getCurrDataItem();
        if (dataItem) {
            dataItem.selFields = this._destFields.getAll();
            dataItem.availableTableFields = this._srcFields.getAll();
        }
    }

    selectDataItem(idx) {       
        this.saveDataItemFields();
        this._activeDataItem = parseInt(idx);
        this.loadDataItemFields();
    }

    onActiveDataItemChanged() {
        this.selectDataItem(document.getElementById("activedataitem").value);
    }

    getCurrDataItem() {
        if ((this._data) && (this._data.report) && (this._data.report.dataItems) && (this._activeDataItem >= 0) && (this._activeDataItem < this._data.report.dataItems.length))
            return this._data.report.dataItems[this._activeDataItem];
        return undefined;
    }

    sortSrcFieldsBy(name) {
        let dispField = 'name';
        //let dispField = name;
        //if (name == 'id')
        //    dispField = 'idname';
        this._srcFields.sortBy(name, dispField);
        document.getElementById('srcfldsortid').className = (name == 'id')?"mselcaptb mseltbbtnsel":"mselcaptb mseltbbtn";
        document.getElementById('srcfldsortname').className = (name == 'name')?"mselcaptb mseltbbtnsel":"mselcaptb mseltbbtn";
    }

}

var wizard;

window.onload = function() {
    wizard = new ReportExtWizard();
};
