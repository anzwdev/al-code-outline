class CodeunitWizard extends TableBasedObjectWizard{

    constructor() {
        super(1);
        this._step = 1;
    }

    updateMainButtons() {
        $("#prevBtn").prop("disabled", true);
        $("#nextBtn").prop("disabled", true);
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
        $("#objectid").val(this._data.objectId);
        $("#objectname").val(this._data.objectName);
        $("#srctable").val(this._data.selectedTable);
        $("#interfaceName").val(this._data.interfaceName);
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
				var match = text.toLowerCase();
				callback(me._data.interfaceList.filter(function(n) { return n.toLowerCase().indexOf(match) !== -1; }));
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
        this._data.objectId = $("#objectid").val();
        this._data.objectName = $("#objectname").val();
        this._data.selectedTable = $("#srctable").val();
        this._data.interfaceName = $("#interfaceName").val();
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

$(function() {
    wizard = new CodeunitWizard();
});
