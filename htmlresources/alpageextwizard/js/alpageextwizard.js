class PageExtWizard extends BaseObjectWizard {

    constructor() {
        super(1);
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
            document.getElementById("objectid").value = this._data.objectId;
            document.getElementById("objectname").value = this._data.objectName;
            document.getElementById("basepage").value = this._data.basePage;
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
				inputfield.value = item;
			},
			fetch: function (text, callback) {
				let match = text.toLowerCase();
				callback(me._data.pageList.filter(function(n) { return n.toLowerCase().indexOf(match) !== -1; }));
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
        this._data.objectId = document.getElementById("objectid").value;
        this._data.objectName = document.getElementById("objectname").value;
        this._data.basePage = document.getElementById("basepage").value;
    }

    canFinish() {
        if (!super.canFinish())
            return false;

        if ((!this._data.basePage) || (this._data.basePage == '')) {
            this.sendMessage({
                command: 'showError',
                message: 'Please enter a target object name.'
            });
            return false;
        }
        return true;
    }

}

var wizard;

window.onload = function() {
    wizard = new PageExtWizard();
};
