var objectBrowser;

$(function() {
    $('#searchtype').multiselect({
        selectAll: true,
        maxPlaceholderOpts: 1,
        texts: {
            placeholder: 'Object type filter',
            selectedOptions: ' types selected'
        }
    });
    objectBrowser = new ObjectBrowser();
});
