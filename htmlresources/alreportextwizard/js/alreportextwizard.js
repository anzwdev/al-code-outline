class ReportExtWizard {

    constructor() {
        this._step = 1;
        this._vscode = acquireVsCodeApi();

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

    updateMainButtons() {
        document.getElementById("prevBtn").disabled = true;
        document.getElementById("nextBtn").disabled = true;
    }

    updateControls() {
        this.updateMainButtons();
    }

    onMessage(message) {
        switch (message.command) {
            case 'setData':
                this.setData(message.data);
                break;
            case 'setReports':
                this.setReports(message.data);
                break;
        }
    }

    sendMessage(data) {
        this._vscode.postMessage(data);
    }

    setData(data) {
        this._data = data;

        if (this._data) {
            //initialize inputs
            document.getElementById("objectid").value = this._data.objectId;
            document.getElementById("objectname").value = this._data.objectName;
            document.getElementById("basereport").value = this._data.baseReport;

            this.updateControls();
        }

    }

    setReports(data) {
        if (!this._data) {
            this._data = {};
        }
        this._data.reportList = data;
        this.loadReports();
    }

    loadReports() {
        if (this._data) {
            this.initReportAutoComplete();
        }
    }

    initReportAutoComplete() {
        let me = this;
        let allowedChars = new RegExp(/^[a-zA-Z\s]+$/);

        autocomplete({
			input: document.getElementById('basereport'),
			minLength: 1,
			onSelect: function (item, inputfield) {
				inputfield.value = item;
			},
			fetch: function (text, callback) {
				let match = text.toLowerCase();
				callback(me._data.reportList.filter(function(n) { return n.toLowerCase().indexOf(match) !== -1; }));
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
			emptyMsg: "No reports found",
			customize: function(input, inputRect, container, maxHeight) {
				if (maxHeight < 100) {
					container.style.top = "";
					container.style.bottom = (window.innerHeight - inputRect.bottom + input.offsetHeight) + "px";
					container.style.maxHeight = "140px";
				}
			}
		});
    }

    onFinish() {
        this.collectStepData(true);

        if (!this.canFinish()) {
            return;
        }

        this.sendMessage({
            command: "finishClick",
            data: {
                objectId : this._data.objectId,
                objectName : this._data.objectName,
                baseReport : this._data.baseReport,
            }
        });
    }

    onCancel() {
        this.sendMessage({
            command : "cancelClick"
        });
    }

    collectStepData(finishSelected) {
        this._data.objectId = document.getElementById("objectid").value;
        this._data.objectName = document.getElementById("objectname").value;
        this._data.baseReport = document.getElementById("basereport").value;
    }

    canFinish() {
        if ((!this._data.objectName) || (this._data.objectName == '')) {
            this.sendMessage({
                command: 'showError',
                message: 'Please enter object name.'
            });
            return false;
        }

        if ((!this._data.baseReport) || (this._data.baseReport == '')) {
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
    wizard = new ReportExtWizard();
};
