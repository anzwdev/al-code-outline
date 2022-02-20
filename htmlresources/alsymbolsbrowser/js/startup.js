var objectBrowser;

$(function() {
    $('#searchtype').multiselect({
        selectAll: true,
        maxPlaceholderOpts: 1,
        maxWidth: 200,
        texts: {
            placeholder: 'Object type filter',
            selectedOptions: ' types selected'
        }
    });
    objectBrowser = new ObjectBrowser();
});
