class FilteredList {

    constructor(filterId, listId) {
        this._filterId = filterId;
        this._listId = listId;
        this._filterValue = undefined;
        this._captionMember = undefined;
        this._sortByMember = undefined;

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
        if (this._data) {
            for (let i=0; i<this._data.length; i++) {
                if (this._data[i].id)
                    this._data[i].idname = this._data[i].id.toString() + ": " + this._data[i].name;
            }
        }
        this.sort();
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
                this.appendEntryHtml(childItems, i);
            }
            element.appendChild(childItems)
        }
        
    }

    getDataCaption(idx) {
        let value = this._data[idx];
        if ((value) && (this._captionMember))
            value = value[this._captionMember];
        return value;
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
                this.appendEntryHtml(childNodes, this._data.length - 1);
            }
            element.appendChild(childNodes);
        }
    }

    appendEntryHtml(childItems, idx) {
        let caption = this.getDataCaption(idx);
        if (this.valueInFilter(caption))
            childItems.appendChild(this.createOption(idx, caption));
    }

    createOption(newValue, newText) {
        let element = document.createElement('option');
        element.text = newText;
        element.value = newValue;
        return element;
    }

    sort() {
        if ((this._data) && (this._sortByMember)) {
            this._data.sort((a,b) => {
                let valA = a[this._sortByMember];
                let valB = b[this._sortByMember];
                if (valA > valB)
                    return 1;
                if (valA < valB)
                    return -1;
                return 0;
            });
        }
    }

    sortBy(name, caption) {
        if (caption)
            this._captionMember = caption;
        this._sortByMember = name;
        this.sort();
        this.updateListUI();
    }

    setCaptionMember(value) {
        this._captionMember = value;
        this.updateListUI();
    }

}