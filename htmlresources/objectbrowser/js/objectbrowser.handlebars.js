(function() {
  var template = Handlebars.template, templates = Handlebars.templates = Handlebars.templates || {};
templates['objectlist'] = template({"1":function(container,depth0,helpers,partials,data,blockParams,depths) {
    var stack1;

  return ((stack1 = helpers["if"].call(depth0 != null ? depth0 : (container.nullContext || {}),(depth0 != null ? depth0.visible : depth0),{"name":"if","hash":{},"fn":container.program(2, data, 0, blockParams, depths),"inverse":container.noop,"data":data})) != null ? stack1 : "");
},"2":function(container,depth0,helpers,partials,data,blockParams,depths) {
    var stack1;

  return ((stack1 = helpers.each.call(depth0 != null ? depth0 : (container.nullContext || {}),(depth0 != null ? depth0.objects : depth0),{"name":"each","hash":{},"fn":container.program(3, data, 0, blockParams, depths),"inverse":container.noop,"data":data})) != null ? stack1 : "");
},"3":function(container,depth0,helpers,partials,data,blockParams,depths) {
    var helper, alias1=container.lambda, alias2=container.escapeExpression, alias3=depth0 != null ? depth0 : (container.nullContext || {}), alias4=helpers.helperMissing, alias5="function";

  return "                <tr data-objt=\""
    + alias2(alias1((depths[1] != null ? depths[1].objectType : depths[1]), depth0))
    + "\" data-objid=\""
    + alias2(((helper = (helper = helpers.Id || (depth0 != null ? depth0.Id : depth0)) != null ? helper : alias4),(typeof helper === alias5 ? helper.call(alias3,{"name":"Id","hash":{},"data":data}) : helper)))
    + "\" data-objname=\""
    + alias2(((helper = (helper = helpers.Name || (depth0 != null ? depth0.Name : depth0)) != null ? helper : alias4),(typeof helper === alias5 ? helper.call(alias3,{"name":"Name","hash":{},"data":data}) : helper)))
    + "\" onclick=\"objclick(this)\"><td>"
    + alias2(alias1((depths[1] != null ? depths[1].objectType : depths[1]), depth0))
    + "</td><td>"
    + alias2(((helper = (helper = helpers.OId || (depth0 != null ? depth0.OId : depth0)) != null ? helper : alias4),(typeof helper === alias5 ? helper.call(alias3,{"name":"OId","hash":{},"data":data}) : helper)))
    + "</td><td>"
    + alias2(((helper = (helper = helpers.Name || (depth0 != null ? depth0.Name : depth0)) != null ? helper : alias4),(typeof helper === alias5 ? helper.call(alias3,{"name":"Name","hash":{},"data":data}) : helper)))
    + "</td></tr>\r\n";
},"compiler":[7,">= 4.0.0"],"main":function(container,depth0,helpers,partials,data,blockParams,depths) {
    var stack1;

  return "<table id=\"objlisttab\">\r\n    <tr><th>Type</th><th>ID</th><th>Name</th></tr>\r\n\r\n"
    + ((stack1 = helpers.each.call(depth0 != null ? depth0 : (container.nullContext || {}),(depth0 != null ? depth0.objectCollections : depth0),{"name":"each","hash":{},"fn":container.program(1, data, 0, blockParams, depths),"inverse":container.noop,"data":data})) != null ? stack1 : "")
    + "\r\n</table>\r\n";
},"useData":true,"useDepths":true});
})();