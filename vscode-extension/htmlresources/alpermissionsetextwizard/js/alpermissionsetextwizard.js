class PermissionSetExtensionWizard extends PermissionSetWizard {

    constructor() {
        super();
    }

    setData(data) {
        super.setData(data);
        if (this._data) {
            //initialize inputs
            document.getElementById("basepermext").value = this._data.basePermissionSet;
        }
    }

    collectStep1Data(finishSelected) {
        super.collectStep1Data(finishSelected);
        this._data.basePermissionSet = document.getElementById("basepermext").value;
    }

    sendFinishMessage() {
        this.sendMessage({
            command: "finishClick",
            data: {
                objectId : this._data.objectId,
                objectName : this._data.objectName,
                basePermissionSet: this._data.basePermissionSet,
                inclAllObjects : this._data.inclAllObjects,
                selectedPermissionSetList: this._data.selectedPermissionSetList
            }
        });
    }

    loadPermissionSets() {
        super.loadPermissionSets();
        this.initPermSetsAutoComplete();
    }

    initPermSetsAutoComplete() {
        let me = this;
        let allowedChars = new RegExp(/^[a-zA-Z\s]+$/);

        autocomplete({
			input: document.getElementById('basepermext'),
			minLength: 1,
			onSelect: function (item, inputfield) {
				inputfield.value = item;
			},
			fetch: function (text, callback) {
				let match = text.toLowerCase();
				callback(me._data.permissionSetList.filter(function(n) { return n.toLowerCase().indexOf(match) !== -1; }));
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
			emptyMsg: "No permission sets found",
			customize: function(input, inputRect, container, maxHeight) {
				if (maxHeight < 100) {
					container.style.top = "";
					container.style.bottom = (window.innerHeight - inputRect.bottom + input.offsetHeight) + "px";
					container.style.maxHeight = "140px";
				}
			}
		});
    }


}


