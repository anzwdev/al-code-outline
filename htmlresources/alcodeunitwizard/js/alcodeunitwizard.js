class CodeunitWizard extends TableBasedObjectWizard{

    constructor() {
        super(1, false);
        this._step = 1;
    }

    updateMainButtons() {
        document.getElementById("prevBtn").disabled = true;
        document.getElementById("nextBtn").disabled = true;
    }

    updateControls() {
        this.updateMainButtons();
    }

    onMessage(message) {     
        super.onMessage(message);

        switch (message.command) {
            case 'setInterfaces':
                this.setInterfaces(message.data);
                break;
        }
    }

    setTables(data) {
        super.setTables(data);
        this.sendMessage({
            command : "loadInterfaces"
        });
    }

    setData(data) {
        this._data = data;
        //initialize fields
        document.getElementById("objectid").value = this._data.objectId;
        document.getElementById("objectname").value = this._data.objectName;
        document.getElementById("srctable").value = this._data.selectedTable;
        document.getElementById("interfaceName").value = this._data.interfaceName;
        this.updateControls();
        this.loadTables();
    }

    setInterfaces(data) {
        if (!this._data)
            this._data = {};
        this._data.interfaceList = data;        
        this.loadInterfaces();
    }

    loadInterfaces() {
        if (this._data)        
            this.initInterfaceAutoComplete()
    }

    initInterfaceAutoComplete() {
        let me = this;
        let allowedChars = new RegExp(/^[a-zA-Z\s]+$/)

        document.getElementById('interfacePart').style.display = 'flex';

        autocomplete({
			input: document.getElementById('interfaceName'),
			minLength: 1,
			onSelect: function (item, inputfield) {
				inputfield.value = item
			},
			fetch: function (text, callback) {
				let match = text.toLowerCase();
				callback(me._data.interfaceList.filter(function(n) { return n.toLowerCase().indexOf(match) !== -1; }));
			},
			render: function(item, value) {
				let itemElement = document.createElement("div");
				if (allowedChars.test(value)) {
					let regex = new RegExp(value, 'gi');
					let inner = item.replace(regex, function(match) { return "<strong>" + match + "</strong>" });
					itemElement.innerHTML = inner;
				} else {
					itemElement.textContent = item;
				}
				return itemElement;
			},
			emptyMsg: "No interfaces found",
			customize: function(input, inputRect, container, maxHeight) {
				if (maxHeight < 100) {
					container.style.top = "";
					container.style.bottom = (window.innerHeight - inputRect.bottom + input.offsetHeight) + "px";
					container.style.maxHeight = "140px";
				}
			}
		})
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
                interfaceName : this._data.interfaceName
            }
        });
    }

    onCancel() {
        this.sendMessage({
            command : "cancelClick"
        })
    }

    collectStepData(finishSelected) {
        this._data.objectId = document.getElementById("objectid").value;
        this._data.objectName = document.getElementById("objectname").value;
        this._data.selectedTable = document.getElementById("srctable").value;
        this._data.interfaceName = document.getElementById("interfaceName").value;
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

}

var wizard;

window.onload = function() {
    wizard = new CodeunitWizard();
};
