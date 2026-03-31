class CodeunitWizard extends TableBasedObjectWizard{

    constructor() {
        super(1, false);
        this._step = 1;
        this._selectedInterfaceChanged = false;
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
        super.setData(data);

        //initialize fields
        this.updateObjectIdControl();
        document.getElementById("objectname").value = this._data.objectName;
        document.getElementById("srctable").value = this._data.selectedTable?.name ?? "";
        document.getElementById("interfaceName").value = this._data.interface?.name ?? "";
        this.updateControls();
        this.loadTables();
    }

    setInterfaces(data) {
        if (!this._data) {
            this._data = {};
        }
        this._data.interfaceList = data;        
        this.loadInterfaces();
    }

    loadInterfaces() {
        if (this._data) {
            this.initInterfaceAutoComplete();
        }
    }

    initInterfaceAutoComplete() {
        let me = this;
        let allowedChars = new RegExp(/^[a-zA-Z\s]+$/);

        document.getElementById('interfacePart').style.display = 'flex';

        autocomplete({
			input: document.getElementById('interfaceName'),
			minLength: 1,
			onSelect: function (item, inputfield) {
				inputfield.value = item.name;
                me.selectInterfaceByObject(item);
			},
			fetch: function (text, callback) {
				let match = text.toLowerCase();
				callback(me._data.interfaceList.filter(function(n) { return n.name.toLowerCase().indexOf(match) !== -1; }));
			},
			render: function(item, value) {
				let itemElement = document.createElement("div");
				if (allowedChars.test(value)) {
					let regex = new RegExp(value, 'gi');
					let inner = item.name.replace(regex, function(match) { return "<strong>" + match + "</strong>"; });
					itemElement.innerHTML = inner;
				} else {
					itemElement.textContent = item.name;
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
		});
    }
   
    sendFinishMessage() {
        this.sendMessage({
            command: "finishClick",
            data: {
                objectId : this._data.objectId,
                objectName : this._data.objectName,
                selectedTable : this._data.selectedTable,
                interface : this._data.interface
            }
        });
    }

    collectStepData(finishSelected) {
        this.selectTableByName(document.getElementById("srctable").value);
        this.selectInterfaceByName(document.getElementById("interfaceName").value);

        this.saveObjectIdControl();
        this._data.objectName = document.getElementById("objectname").value;
    }

    selectInterfaceByName(name) {
        if (this._data.interface?.name !== name) {
            this.selectInterfaceByObject(this.findObjectListItemByName(this._data.interfaceList, name));
        }
    }

    selectInterfaceByObject(obj) {
        this._selectedInterfaceChanged = (this._data.interface?.uid !== obj?.uid);
        this._data.interface = obj;
    }

}

var wizard;

window.onload = function() {
    wizard = new CodeunitWizard();
};
