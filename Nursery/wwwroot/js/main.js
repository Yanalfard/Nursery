define(["require", "exports", "./model/form"], function (require, exports, form_1) {
    "use strict";
    exports.__esModule = true;
    var forms = document.querySelectorAll('dform');
    for (var _i = 0, _a = forms; _i < _a.length; _i++) {
        var item = _a[_i];
        var stringModel = item.getAttribute('model');
        var model = JSON.parse(stringModel);
        var form = new form_1.Form(model);
        for (var _b = 0, _c = model.Fields; _b < _c.length; _b++) {
            var field = _c[_b];
            form.addField(field);
        }
        form.attachForm(item);
        console.log(form);
    }
});
//# sourceMappingURL=main.js.map