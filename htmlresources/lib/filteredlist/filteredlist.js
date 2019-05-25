class FilteredList {

    constructor(filterId, listId) {
        this._filterId = filterId;
        this._listId = listId;
        this._filterValue = undefined;

        if (this._filterId) {
            let filterElement = document.getElementById(this._filterId);
            let that = this;
            if (filterElement)
                filterElement.addEventListener('input', function(e) { that.onFilterChanged(e); });
        }
    }

    setData(data) {
        if (data)
            this._data = data.slice();
        else
            this._data = [];
        this.updateListUI();
    }

    clear() {
        this._data = [];
        this.updateListUI();
    }

    onFilterChanged(e) {
        this.updateListUI();
    }

    getFilterValue() {
        this._filterValue = undefined;
        if (this._filterId) {
            let element = document.getElementById(this._filterId);
            if (element.value)
                this._filterValue = element.value.toLowerCase();
        }
    }

    valueInFilter(value) {
        if (!this._filterValue)
            return true;
        if (!value)
            return false;
        return (value.toLowerCase().indexOf(this._filterValue) !== -1);
    }

    updateListUI() {
        this.getFilterValue();
        //clear element        
        let element = document.getElementById(this._listId);
        while (element.firstChild) {
            element.removeChild(element.firstChild);
        }
        //build list of elements
        if (this._data) {
            let childItems = document.createDocumentFragment();
            for (let i=0; i<this._data.length; i++) {
                if (this.valueInFilter(this._data[i]))
                    childItems.appendChild(this.createOption(i, this._data[i]));
            }
            element.appendChild(childItems)
        }
    }

    getAll() {
        return this._data;
    }

    removeSelected() {
        return this.removeByCssSelector('option:checked');
    }

    removeFiltered() {
        return this.removeByCssSelector('option');
    }

    removeByCssSelector(selector) {
        let values = [];
        let element = document.getElementById(this._listId);
        let selectedNodes = element.querySelectorAll(selector);
        if ((selectedNodes) && (selectedNodes.length > 0)) {           
            for (let i=0; i<selectedNodes.length; i++) {
                values.push(this._data[selectedNodes[i].value]);
                this._data[selectedNodes[i].value] = undefined;
            }
            //buld new list of element
            let newData = [];
            for (let i=0; i<this._data.length; i++) {
                if (this._data[i])
                    newData.push(this._data[i]);
            }
            this._data = newData;
            this.updateListUI();
        }
        return values;
    }

    add(values) {
        if ((values) && (values.length > 0)) {
            let element = document.getElementById(this._listId);
            if (!this._data)
                this._data = [];
            let childNodes = document.createDocumentFragment();
            for (let i = 0; i<values.length; i++) {
                this._data.push(values[i]);
                if (this.valueInFilter(this._data[i]))
                    childNodes.appendChild(this.createOption(this._data.length - 1, values[i]));
            }
            element.appendChild(childNodes);
        }
    }

    createOption(newValue, newText) {
        let element = document.createElement('option');
        element.text = newText;
        element.value = newValue;
        return element;
    }

}