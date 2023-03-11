class InterfaceWizard extends BaseObjectWizard {

    constructor() {
        super(1);
    }

    onMessage(message) {
        super.onMessage(message);

        switch (message.command) {
            case 'setCodeunits':
                this.setCodeunits(message.data);
                break;
        }
    }

    setData(data) {
        super.setData(data);
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
   
    sendFinishMessage() {
        this.sendMessage({
            command: "finishClick",
            data: {
                objectName : this._data.objectName,
                baseCodeunitName : this._data.baseCodeunitName
            }
        });
    }

    collectStepData(finishSelected) {
        this._data.objectName = document.getElementById("objectname").value;
        this._data.baseCodeunitName = document.getElementById("srccodeunit").value;
    }

}

var wizard;

window.onload = function() {
    wizard = new InterfaceWizard();
};
