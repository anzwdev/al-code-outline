class PermissionSetExtensionWizard extends PermissionSetWizard {

    constructor() {
        super();
        this._basePermissionSetChanged = false;
    }

    setData(data) {
        super.setData(data);
        if (this._data) {
            //initialize inputs
            document.getElementById("basepermext").value = this._data.basePermissionSet?.name ?? "";
        }
    }

    collectStep1Data(finishSelected) {
        super.collectStep1Data(finishSelected);
        this.selectBasePermissionSetByName(document.getElementById("basepermext").value);
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
				inputfield.value = item.name;
                me.selectBasePermSetByObject(item);
			},
			fetch: function (text, callback) {
				let match = text.toLowerCase();
				callback(me._data.permissionSetList.filter(function(n) { return n.name.toLowerCase().indexOf(match) !== -1; }));
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

    selectBasePermissionSetByName(name) {
        if (this._data.basePermissionSet?.name !== name) {
            this.selectBasePermissionSetByObject(this.findObjectListItemByName(this._data.permissionSetList, name));
        }
    }

    selectBasePermissionSetByObject(obj) {
        this._basePermissionSetChanged = (this._data.basePermissionSet?.uid !== obj?.uid);
        this._data.basePermissionSet = obj;
    }    

}


