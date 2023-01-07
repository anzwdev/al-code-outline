class TableBasedObjectWizard {

    constructor(maxStepNo, flowFiltersSupported) {
        this._maxStepNo = maxStepNo;
        this._flowFiltersSupported = !!flowFiltersSupported;

        this._srcFields = new FilteredList('srcfieldsfilter', 'srcfields');
        this._destFields = new FilteredList('destfieldsfilter', 'destfields');
        this.initFieldsSelProp(this._srcFields, this._destFields);

        if (this._flowFiltersSupported) {
            this._srcFlowFilters = new FilteredList('srcflowfiltersfilter', 'srcflowfilters');
            this._destFlowFilters = new FilteredList('destflowfiltersfilter', 'destflowfilters');
            this.initFieldsSelProp(this._srcFlowFilters, this._destFlowFilters);
        }

        //initialize properties
        this._vscode = acquireVsCodeApi();

        let fldSortName = document.getElementById('srcfldsortname');
        let fldSortId = document.getElementById('srcfldsortid');

        if (fldSortName)
            fldSortName.className = "mselcaptb mseltbbtnsel";
        
        this.initNameLenUpdate();

        // Handle messages sent from the extension to the webview
        window.addEventListener('message', event => {
            this.onMessage(event.data);
        });

        document.getElementById('prevBtn').addEventListener('click', event => {
            this.onPrev();
        });

        document.getElementById('nextBtn').addEventListener('click', event => {
            this.onNext();
        });

        document.getElementById('finishBtn').addEventListener('click', event => {
            this.onFinish();
        });

        document.getElementById('cancelBtn').addEventListener('click', event => {
            this.onCancel();
        });      

        if (fldSortId)
            fldSortId.addEventListener('click', event => {
                this.sortSrcFieldsBy("id");
            });
        if (fldSortName)
            fldSortName.addEventListener('click', event => {
                this.sortSrcFieldsBy("name");
            });


        this.sendMessage({
            command: 'documentLoaded'
        });   
    }

    initFieldsSelProp(srcFlds, destFlds) {
        srcFlds._captionMember = 'name';
        srcFlds._sortByMember = 'name';
        srcFlds._descriptionMember = 'uiDesc';
        destFlds._captionMember = 'name';
        destFlds._descriptionMember = 'uiDesc';
    }

    registerFieldsSelectionEvents() {
        document.getElementById('mselright').addEventListener('click', event => {
            this.onMoveFieldsRight(this._srcFields, this._destFields);
        });
        document.getElementById('mselallright').addEventListener('click', event => {
            this.onMoveAllRight(this._srcFields, this._destFields);
        });      
        document.getElementById('mselallleft').addEventListener('click', event => {
            this.onMoveAllLeft(this._srcFields, this._destFields);
        });
        document.getElementById('mselleft').addEventListener('click', event => {
            this.onMoveFieldsLeft(this._srcFields, this._destFields);
        });      
        document.getElementById('srcfields').addEventListener('dblclick', event => {
            this.onMoveFieldsRight(this._srcFields, this._destFields);
        });
        document.getElementById('destfields').addEventListener('dblclick', event => {
            this.onMoveFieldsLeft(this._srcFields, this._destFields);
        });
    }

    registerFlowFiltersSelectionEvents() {
        document.getElementById('mffselright').addEventListener('click', event => {
            this.onMoveFieldsRight(this._srcFlowFilters, this._destFlowFilters);
        });
        document.getElementById('mffselallright').addEventListener('click', event => {
            this.onMoveAllRight(this._srcFlowFilters, this._destFlowFilters);
        });      
        document.getElementById('mffselallleft').addEventListener('click', event => {
            this.onMoveAllLeft(this._srcFlowFilters, this._destFlowFilters);
        });
        document.getElementById('mffselleft').addEventListener('click', event => {
            this.onMoveFieldsLeft(this._srcFlowFilters, this._destFlowFilters);
        });      
        document.getElementById('srcflowfilters').addEventListener('dblclick', event => {
            this.onMoveFieldsRight(this._srcFlowFilters, this._destFlowFilters);
        });
        document.getElementById('destflowfilters').addEventListener('dblclick', event => {
            this.onMoveFieldsLeft(this._srcFlowFilters, this._destFlowFilters);
        });
    }

    onMessage(message) {     
        switch (message.command) {
            case 'setData':
                this.setData(message.data);
                break;
            case 'setTables':
                this.setTables(message.data);
                break;
            case 'setFields':
                this.setFields(message.data);
                break;
        }
    }

    sendMessage(data) {
        this._vscode.postMessage(data);
    }

    setData(data) {   
        this._data = data;
    }

    loadFields() {
        this._srcFields.setData(this._data.fieldList);
        this._destFields.clear();
    }

    loadFlowFilters() {
        if (this._flowFiltersSupported) {
            if (!this._data.flowFilterList)
                this._data.flowFilterList = [];
            this._srcFlowFilters.setData(this._data.flowFilterList);
            this._destFlowFilters.clear();
        }
    }

    loadFieldsAdv(selFlds) {
        let flds = this._data.fieldList;
        if ((flds) && (flds.length > 0) && (selFlds) && (selFlds.length > 0))
            flds = flds.filter(item => {
                return (selFlds.indexOf(item) < 0);
            });
        this._srcFields.setData(flds);
        this._destFields.clear();
    }

    loadTables() {
        if (this._data)        
            this.initAutoComplete()
    }

    initAutoComplete() {
        let me = this;
        let allowedChars = new RegExp(/^[a-zA-Z\s]+$/)

        autocomplete({
			input: document.getElementById('srctable'),
			minLength: 1,
			onSelect: function (item, inputfield) {
				inputfield.value = item
			},
			fetch: function (text, callback) {
				var match = text.toLowerCase();
				callback(me._data.tableList.filter(function(n) { return n.toLowerCase().indexOf(match) !== -1; }));
			},
			render: function(item, value) {
				var itemElement = document.createElement("div");
				if (allowedChars.test(value)) {
					var regex = new RegExp(value, 'gi');
					var inner = item.replace(regex, function(match) { return "<strong>" + match + "</strong>" });
					itemElement.innerHTML = inner;
				} else {
					itemElement.textContent = item;
				}
				return itemElement;
			},
			emptyMsg: "No tables found",
			customize: function(input, inputRect, container, maxHeight) {
				if (maxHeight < 100) {
					container.style.top = "";
					container.style.bottom = (window.innerHeight - inputRect.bottom + input.offsetHeight) + "px";
					container.style.maxHeight = "140px";
				}
			}
		})
    }

    setFields(data) {
        if (!this._data)
            this._data = {};

        this._data.fieldList = data.fieldList;
        this._data.flowFilterList = data.flowFilterList;

        this.loadFields();
        this.loadFlowFilters();
    }

    setTables(data) {
        if (!this._data)
            this._data = {};
        this._data.tableList = data;
        this.loadTables();
    }

    onMoveFieldsRight(srcFlds, destFlds) {
        destFlds.add(srcFlds.removeSelected());
    }

    onMoveFieldsLeft(srcFlds, destFlds) {
        srcFlds.add(destFlds.removeSelected());
    }

    onMoveAllRight(srcFlds, destFlds) {
        destFlds.add(srcFlds.removeFiltered());
    }

    onMoveAllLeft(srcFlds, destFlds) {
        srcFlds.add(destFlds.removeFiltered());
    }

    getSelectedFields() {
        return this._destFields.getAll();
    }    

    setSelectedFields(list) {
        this._destFields.setData(list);
    }

    getSelectedFlowFilters() {
        return this._destFlowFilters.getAll();
    }

    setSelectedFlowFilters(list) {
        this._destFlowFilters.setData(list);
    }

    onFinish() {
    }

    onCancel() {
        this.sendMessage({
            command : "cancelClick"
        })
    }

    onPrev() {
        if (this._step > 1) {
            this.collectStepData(false);
            this.setStep(this._step - 1);
        }
    }

    onNext() {
        if (this._step < this._maxStepNo) {
            this.collectStepData(false);
            this.setStep(this._step + 1);
        }
    }

    collectStepData(finishSelected) {
    }

    setStep(newStep) {
    }

    canFinish() {
        if ((!this._data.objectName) || (this._data.objectName == '')) {
            this.sendMessage({
                command: 'showError',
                message: 'Please enter object name.'
            });
            return false;
        }
        return true;
    }

    sortSrcFieldsBy(name) {
        let dispField = 'name';
        this._srcFields.sortBy(name, dispField);
        document.getElementById('srcfldsortid').className = (name == 'id')?"mselcaptb mseltbbtnsel":"mselcaptb mseltbbtn";
        document.getElementById('srcfldsortname').className = (name == 'name')?"mselcaptb mseltbbtnsel":"mselcaptb mseltbbtn";
    }

    initNameLenUpdate() {
        this._ctName = document.getElementById('objectname');
        this._ctNameLen = document.getElementById('objectnamelen');
        if ((this._ctName) && (this._ctNameLen)) {
            this.updateNameLen();
            this._ctName.addEventListener('input', event => {
                this.updateNameLen();
            });   
        }
    }

    updateNameLen() {
        this._ctNameLen.innerText = this._ctName.value.length.toString();
    }  

}