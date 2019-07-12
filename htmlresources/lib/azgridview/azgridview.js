class AZGridView {

    constructor(elementId, columns, leftElementId, rightElementId, loadingText) {
        this._elementId = elementId;
        this._columns = columns;
        this._container = document.getElementById(this._elementId);
        this._container.className = 'azgridviewcont';
        this._selStyle = 'azgridviewrowsel';
        this._currRowStyle = this._selStyle + ' azgridviewrowactive'; 
        this._leftElementId = leftElementId;
        this._rightElementId = rightElementId;
        this._loadingText = loadingText;

        this._currRow = -1;
        this._currDataIdx = -1;
        this._minRowIndex = 1;

        this.renderTable();

        let that = this;
        this._mouseMoveFunction = function(e) { that.onMouseMove(e); };
        this._table.addEventListener('pointerdown', function(e) { that.onMouseDown(e); }, true);
        this._container.addEventListener('keydown', function(e) { that.onKeyDown(e); });
        this._table.addEventListener('pointerup', function(e) { that.onMouseUp(e); });

/*
        this._container.addEventListener('keydown', function(e) {
            if ((!e.ctrlKey) || (e.keyCode !== 65))
                return;
            e.preventDefault();
            return false;
        }, true);
        */
    }

    setData(data) {
        if (this._loadingDiv) {
            this._container.removeChild(this._loadingDiv);
            this._loadingDiv = undefined;
        }
        this._data = data;
        this.renderTableHeader();
        this.renderData();
    }

    renderData() {
        let newIdx = -1;
        let rowCount = 0;

        let tbody = document.createElement('tbody');
        for (let i=0; i<this._data.length; i++) {
            if (this.checkRowFilter(i)) {
                rowCount++;
                tbody.appendChild(this.renderRow(i));
                if (i == this._currDataIdx)
                    newIdx = rowCount + this._minRowIndex - 1;
            }
        }
        if ((newIdx < 0) && (rowCount > 0))
            newIdx = this._minRowIndex;

        if (this._tabBody)
            this._table.replaceChild(tbody, this._tabBody);
        else
            this._table.appendChild(tbody);
        this._tabBody = tbody;

        this._currRow = -1;
        this.setCurrRow(newIdx, false, false, true);
    }

    renderRow(index) {
        let row = document.createElement('tr');
        row.tabData = this._data[index];
        for (let i=0; i<this._columns.length; i++) {
            if (!this._columns[i].hidden) {
                let value = this._data[index][this._columns[i].name];
                let col = document.createElement('td');
                col.innerText = value;
                row.appendChild(col);
                if (this._columns[i].data)
                    row.dataset[this._columns[i].data] = value;
            }

        }
        return row;
    }

    renderTable() {
        //clear container element
        while (this._container.firstChild) {
            this._container.removeChild(this._container.firstChild);
        }
        //create table
        this._table = document.createElement('table');
        this._table.className = 'azgridview';
        //create headers
        this._tabHead = document.createElement('thead');                
        this._table.appendChild(this._tabHead);
        this._container.appendChild(this._table);

        if (this._loadingText) {
            this._loadingDiv = document.createElement('div');
            this._loadingDiv.className = 'loading';
            this._loadingDiv.innerText = this._loadingText;
            this._container.appendChild(this._loadingDiv);
        }
    }

    renderTableHeader() {
        let row = document.createElement('tr');
        for (let i=0; i<this._columns.length; i++) {
            if (!this._columns[i].hidden) {
                let col = document.createElement('th');
                col.innerText = this._columns[i].caption;
                if (this._columns[i].style)
                    col.style = this._columns[i].style;
                row.appendChild(col);
            }
        }
        this._tabHead.appendChild(row);
    }

    setCurrRow(index, clearSel, invertSel, setStart) {
        if (index < this._minRowIndex)
            index = this._minRowIndex;
        if (index >= this._table.rows.length)
            index = this._table.rows.length - 1;

        if ((setStart) || (!this._startRow))
            this._startRow = index;

        if ((this._currRow != index) || (invertSel)) {
            if (setStart) {
                this.selectRange(index, index, invertSel, clearSel);
            } else
                this.selectRange(Math.min(this._startRow, index), Math.max(this._startRow, index), invertSel, clearSel);
            
            if (this._currRow != index) {
                this.refreshRowStyle(this._currRow, false);
                this._currRow = index;
                this.refreshRowStyle(this._currRow, true);
            }
            
        }
        if (this._currRow >= 0)
            this.scrollToElement(this._table.rows[index]);

        if (this.onCurrRowChanged)
            this.onCurrRowChanged(this._table.rows[index].tabData);
    }

    refreshRowStyle(rowIndex, setCurrentStyle) {
        if ((rowIndex >= this._minRowIndex) && (rowIndex < this._table.rows.length)) {
            if (setCurrentStyle)
                this._table.rows[rowIndex].classList = this._currRowStyle;
            else if (this._table.rows[rowIndex].classList == this._currRowStyle)
                this._table.rows[rowIndex].classList = this._selStyle;
        }
    }

    selectRange(startIndex, endIndex, invertSel, clearSel) {
        if (startIndex < this._minRowIndex)
            startIndex = this._minRowIndex;
        if (endIndex >= this._table.rows.length)
            endIndex = this._tabBody.rows.length - 1;
        for (let i=startIndex; i<=endIndex; i++) {
            let row = this._table.rows[i];
            if ((invertSel) && ((row.className == this._selStyle) || (row.className == this._currRowStyle)))
                row.className = '';
            else if (i == this._currRow)
                row.className = this._currRowStyle;
            else
                row.className = this._selStyle;
        }
        if (clearSel) {
            for (let i=0; i<startIndex; i++) {
                this._table.rows[i].className = '';
            }
            for (let i=endIndex+1; i<this._table.rows.length; i++) {
                this._table.rows[i].className = '';
            }
        }

    }

    selectAll() {
        this.selectRange(this._minRowIndex, this._table.rows.length - 1, false, false);
    }
    
    nextPageRowIndex() {
        if ((this._currRow < this._minRowIndex) || (this._currRow >= this._table.rows.length))
            return this._minRowIndex;
        let row = this._table.rows[this._currRow];
        let targetTop = row.offsetTop - row.offsetHeight + this._container.clientHeight;
        for (let i = this._currRow + 1; i<this._table.rows.length; i++) {
            if (this._table.rows[i].offsetTop >= targetTop)
                return i;
        }
        return this._table.rows.length - 1;
    }

    prevPageRowIndex() {
        if ((this._currRow < this._minRowIndex) || (this._currRow >= this._table.rows.length))
            return this._minRowIndex;
        let row = this._table.rows[this._currRow];
        let targetTop = row.offsetTop + row.offsetHeight - this._container.clientHeight;
        for (let i = this._currRow - 1; i >= this._minRowIndex; i--) {
            if (this._table.rows[i].offsetTop <= targetTop)
                return i;
        }
        return this._minRowIndex;
    }

    onMouseDown(e) {
        if (e.button == 0) {
            let node = e.target;
            let row = node.closest('tr');
            if (row) {
                this.setCurrRow(row.rowIndex, !e.ctrlKey, ((e.ctrlKey) && (!e.shiftKey)), !e.shiftKey);
            }
            this._table.setPointerCapture(e.pointerId);
            this._table.addEventListener('pointermove', this._mouseMoveFunction);
        }
    }

    onMouseUp(e) {
        if (e.button == 0) {
            this._table.removeEventListener('pointermove', this._mouseMoveFunction);
            this._table.releasePointerCapture(e.pointerId);
        }
    }

    onMouseMove(e) {
        let rect = this._table.getBoundingClientRect();
        let posY = e.clientY - rect.top;
        let row = this.findRowByYPos(posY);
    	    //scrollTop = window.pageYOffset || document.documentElement.scrollTop;
	        //return { top: rect.top + scrollTop, left: rect.left + scrollLeft }
        //!        let node = document.elementFromPoint(e.clientX, e.clientY);
        //let node = e.target;
        //!        let row = node.closest('tr');
        if (row)
            this.setCurrRow(row.rowIndex, !e.ctrlKey, false, false);
        e.preventDefault();
    }

    onKeyDown(e) {    
        let handled = false;
        let nodeList;
        let clearSel = !e.ctrlKey;
        let invSel = ((e.ctrlKey) && (!e.shiftKey));
        let setSel = !e.shiftKey;

        switch (e.which) {
            case 65:    //A
                if (e.ctrlKey) {
                    this.selectAll();
                    handled = true;
                }
                break;
            case 37:    //left
                if (this._leftElementId)
                    document.getElementById(this._leftElementId).focus();
                handled = true;
                break;
            case 39:    //right
                if (this._rightElementId)
                    document.getElementById(this._rightElementId).focus();
                handled = true;
                break;
            case 38:    //up
                this.setCurrRow(this._currRow - 1, clearSel, invSel, setSel);
                handled = true;
                break;
            case 40:    //down
                this.setCurrRow(this._currRow + 1, clearSel, invSel, setSel);
                handled = true;
                break;
            case 33:    //page up
                this.setCurrRow(this.prevPageRowIndex(), clearSel, invSel, setSel);
                handled = true;
                break;
            case 34:    //page down
                this.setCurrRow(this.nextPageRowIndex(), clearSel, invSel, setSel);
                handled = true;
                break;
            case 36:    //home
                this.setCurrRow(0, clearSel, invSel, setSel);
                handled = true;
                break;
            case 35:    //end
                this.setCurrRow(this._table.rows.length - 1, clearSel, invSel, setSel);
                handled = true;
                break;
        }

        if (handled) {
            e.preventDefault();
            e.stopPropagation();
            return false;
        }

        return true;
    }

    scrollToElement(element) {
        let mainTop = 0; //this._container.offsetTop;
        let viewTop = this._container.scrollTop;
        let viewBottom = viewTop + this._container.clientHeight;
        let elementTop = element.offsetTop - mainTop;
        let elementBottom = elementTop + element.offsetHeight;

        if (elementTop < viewTop)
            element.scrollIntoView(true);
        else if (elementBottom > viewBottom)
            element.scrollIntoView(false);
    }

    getSelected() {
        let sel = [];
        for (let i=this._minRowIndex; i<this._table.rows.length; i++) {
            if ((this._table.rows[i].className == this._selStyle) || (this._table.rows[i].className == this._currRowStyle))
                sel.push(this._table.rows[i].tabData);
        }
        return sel;
    }

    findRowByYPos(yPos) {
        let min = this._minRowIndex;
        let max = this._table.rows.length - 1;

        while (min < max) {
            let mid = Math.trunc((min + max) / 2);
            let row = this._table.rows[mid];
            if (row.offsetTop <= yPos) {
                if (row.offsetTop + row.offsetHeight > yPos)
                    return row;
                min = mid + 1;

            } else
                max = mid - 1;
        }

        if (min == max)
            return this._table.rows[min];
        return undefined;
    }

    checkRowFilter(index) {
        for (let i=0; i<this._columns.length; i++) {
            if (!this.checkColFilter(index, i))
                return false;
        }
        return true;
    }

    checkColFilter(index, column) {
        if ((!this._columns[column].appFilter) && 
            (!this._columns[column].userFilter) && 
            ((!this._columns[column].userFilterArray) || (this._columns[column].userFilterArray.length == 0)))
            return true;
        let value = this._data[index][this._columns[column].name];
        
        if ((this._columns[column].appFilter) && (value != this._columns[column].appFilter))
            return false;
        
        if (this._columns[column].compiledFilter) {
            if (this._columns[column].isNumVal) {
                if (!this._columns[column].compiledFilter({INT: value}))
                    return false;
            } else {
                if (!this._columns[column].compiledFilter({TEXT: value}))
                    return false;
            }
        }

        if ((this._columns[column].userFilterArray) && (this._columns[column].userFilterArray.length > 0)) {
            if (this._columns[column].userFilterArray.indexOf(value) < 0)
                return false;
        }

        return true;
    }

    compileFilters() {
        for (let i=0; i<this._columns.length; i++) {
            if (this._columns[i].userFilter != this._columns[i].applUserFilter) {
                if (this._columns[i].userFilter) {
                    try {
                        if (this._columns[i].isNumVal)
                            this._columns[i].compiledFilter = compileFilter('int', this._columns[i].userFilter);
                        else    
                            this._columns[i].compiledFilter = compileFilter('text', this._columns[i].userFilter);
                    }
                    catch (e) {                        
                    }
                } else {
                    this._columns[i].compiledFilter = undefined;
                }
                this._columns[i].applUserFilter = this._columns[i].userFilter;
            }
        }
    }

}