class PageExtWizard extends BaseObjectWizard {

    constructor() {
        super(1);
        this._basePageChanged = false;
    }

    onMessage(message) {
        super.onMessage(message);

        switch (message.command) {
            case 'setPages':
                this.setPages(message.data);
                break;
        }
    }

    setData(data) {
        super.setData(data);
       
        if (this._data) {
            //initialize inputs
            this.updateObjectIdControl();
            document.getElementById("objectname").value = this._data.objectName;
            document.getElementById("basepage").value = this._data.basePage?.name ?? "";
            this.updateControls();
        }

    }

    setPages(data) {
        if (!this._data) {
            this._data = {};
        }
        this._data.pageList = data;        
        this.loadPages();
    }

    loadPages() {
        if (this._data) {
            this.initPageAutoComplete();
        }
    }

    initPageAutoComplete() {
        let me = this;
        let allowedChars = new RegExp(/^[a-zA-Z\s]+$/);

        autocomplete({
			input: document.getElementById('basepage'),
			minLength: 1,
			onSelect: function (item, inputfield) {
				inputfield.value = item.name;
                me.selectBasePageByObject(item);
			},
			fetch: function (text, callback) {
				let match = text.toLowerCase();
				callback(me._data.pageList.filter(function(n) { return n.name.toLowerCase().indexOf(match) !== -1; }));
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
			emptyMsg: "No pages found",
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
                basePage : this._data.basePage,
            }
        });
    }

    collectStepData(finishSelected) {
        this.selectBasePageByName(pageNamedocument.getElementById("basepage").value);
        this.saveObjectIdControl();
        this._data.objectName = document.getElementById("objectname").value;
    }

    canFinish() {
        if (!super.canFinish()) {
            return false;
        }

        if ((!this._data.basePage) || (this._data.basePage === "")) {
            this.sendMessage({
                command: 'showError',
                message: 'Please enter a target object name.'
            });
            return false;
        }
        return true;
    }


    selectBasePageByName(name) {
        if (this._data.basePage?.name !== name) {
            this.selectBasePageByObject(this.findObjectListItemByName(this._data.pageList, name));
        }
    }

    selectBasePageByObject(obj) {
        this._basePageChanged = (this._data.basePage?.uid !== obj?.uid);
        this._data.basePage = obj;
    }

}

var wizard;

window.onload = function() {
    wizard = new PageExtWizard();
};
