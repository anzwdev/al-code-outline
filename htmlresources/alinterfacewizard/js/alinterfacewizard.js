class InterfaceWizard {

    constructor() {
        //initialize properties
        this._step = 1;
        this._vscode = acquireVsCodeApi();

        this.initNameLenUpdate();

        // Handle messages sent from the extension to the webview
        window.addEventListener('message', event => {
            this.onMessage(event.data);
        });

        document.getElementById('finishBtn').addEventListener('click', event => {
            this.onFinish();
        });

        document.getElementById('cancelBtn').addEventListener('click', event => {
            this.onCancel();
        });      

        this.sendMessage({
            command: 'documentLoaded'
        });
    }

    onMessage(message) {     
        switch (message.command) {
            case 'setData':
                this.setData(message.data);
                break;
            case 'setCodeunits':
                this.setCodeunits(message.data);
                break;
        }
    }

    sendMessage(data) {
        this._vscode.postMessage(data);    
    }

    setData(data) {
        this._data = data;
        //initialize fields
        document.getElementById("objectname").value = this._data.objectName;
        document.getElementById("srccodeunit").value = this._data.baseCodeunitName;
    }

    setCodeunits(data) {
        if (!this._data)
            this._data = {};
        this._data.codeunitList = data;        
        this.loadCodeunits();
    }

    loadCodeunits() {
        if (this._data)        
            this.initCodeunitAutoComplete()
    }

    initCodeunitAutoComplete() {
        let me = this;
        let allowedChars = new RegExp(/^[a-zA-Z\s]+$/)

        autocomplete({
			input: document.getElementById('srccodeunit'),
			minLength: 1,
			onSelect: function (item, inputfield) {
				inputfield.value = item
			},
			fetch: function (text, callback) {
				let match = text.toLowerCase();
				callback(me._data.codeunitList.filter(function(n) { return n.toLowerCase().indexOf(match) !== -1; }));
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
			emptyMsg: "No codeunits found",
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
                objectName : this._data.objectName,
                baseCodeunitName : this._data.baseCodeunitName
            }
        });
    }

    onCancel() {
        this.sendMessage({
            command : "cancelClick"
        })
    }

    collectStepData(finishSelected) {
        this._data.objectName = document.getElementById("objectname").value;
        this._data.baseCodeunitName = document.getElementById("srccodeunit").value;
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

var wizard;

window.onload = function() {
    wizard = new InterfaceWizard();
};
