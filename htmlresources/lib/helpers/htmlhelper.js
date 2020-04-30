var htmlHelper = {
    setElementOptions : function (element, values, indexAsValue) {
        htmlHelper.clearChildren(element);

        if (data) {
            for (let i=0; i<data.length; i++) {
                option.label = data[i].label;
                option.value = data[i].value;
                option.innerText = data[i].label;
                option.selected = data[i].selected
                this._analyzersSel.appendChild(option);
            }
        }
        if (values) {
            for (var i=0; i<values.length;i++) {
                let option = document.createElement('option');
                if (indexAsValue) {
                    option.label = values[i];
                    option.value = i;
                } else {
                    option.label = values[i];
                    option.value = values[i];
                }
                option.innerText = option.label;
                element.appendChild(option);
            }
        }
    },
    hide : function(element) {
        if (element) {
            element.dataset.hideOldDisplay = element.style.display;
            element.style.display = 'none';
        }
    },
    show : function(element) {
        if (element)
            element.style.display = element.dataset.hideOldDisplay;
    },
    hideById : function(id) {
        htmlHelper.hide(document.getElementById(id));
    },
    showById : function(id) {
        htmlHelper.show(document.getElementById(id));
    },
    clearChildren : function(element) {
        if (element) {
            while (element.firstChild) {
                element.removeChild(element.firstChild);
            }
        }
    },
    clearChildrenById : function(id) {
        htmlHelper.clearChildren(document.getElementById(id));
    }

}



