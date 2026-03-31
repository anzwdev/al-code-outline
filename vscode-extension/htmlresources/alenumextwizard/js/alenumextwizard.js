class EnumExtWizard extends BaseObjectWizard {

    constructor() {
        super(1);

        this._selectedBaseEnumChanged = false;
    }

    onMessage(message) {
        super.onMessage(message);

        switch (message.command) {
            case 'setEnums':
                this.setEnums(message.data);
                break;
        }
    }

    setData(data) {
        super.setData(data);

        //initialize fields
        this.updateObjectIdControl();
        document.getElementById("objectname").value = this._data.objectName;
        document.getElementById("baseenum").value = this._data.baseEnum?.name ?? "";
        document.getElementById("firstvalueid").value = this._data.firstValueId;
        document.getElementById("valuelist").value = this._data.valueList;
        document.getElementById("captionlist").value = this._data.captionList;
        this.loadEnums();
    }
   
    setEnums(data) {
        if (!this._data) {
            this._data = {};
        }
        this._data.baseEnumList = data;
        this.loadEnums();
    }

    loadEnums() {
        if (this._data) {
            this.initAutoComplete();
        }
    }

    initAutoComplete() {
        let me = this;
        let allowedChars = new RegExp(/^[a-zA-Z\s]+$/);

        autocomplete({
			input: document.getElementById('baseenum'),
			minLength: 1,
			onSelect: function (item, inputfield) {
				inputfield.value = item.name;
                me.selectBaseEnumByObject(item);
			},
			fetch: function (text, callback) {
				let match = text.toLowerCase();
				callback(me._data.baseEnumList.filter(function(n) { return n.name.toLowerCase().indexOf(match) !== -1; }));
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
			emptyMsg: "No enums found",
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
                baseEnum : this._data.baseEnum,
                firstValueId : this._data.firstValueId,
                valueList : this._data.valueList,
                captionList : this._data.captionList
            }
        });
    }

    collectStepData(finishSelected) {
        this.selectBaseEnumByName(document.getElementById("baseenum").value);

        this.saveObjectIdControl();
        this._data.objectName = document.getElementById("objectname").value;
        this._data.firstValueId = document.getElementById("firstvalueid").value;
        this._data.valueList = document.getElementById("valuelist").value;
        this._data.captionList = document.getElementById("captionlist").value;
    }

    selectBaseEnumByName(name) {
        if (this._data.baseEnum?.name !== name) {
            this.selectBaseEnumByObject(this.findObjectListItemByName(this._data.baseEnumList, name));
        }       
    }

    selectBaseEnumByObject(baseEnumObject) {
        this._selectedBaseEnumChanged = (this._data.baseEnum?.uid !== baseEnumObject?.uid);
        this._data.baseEnum = baseEnumObject;
    }
}

var wizard;

window.onload = function() {
    wizard = new EnumExtWizard();
};
