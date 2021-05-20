define(["require", "exports", "./model/form"], function (require, exports, form_1) {
    "use strict";
    exports.__esModule = true;
    var forms = document.querySelectorAll('dform');
    for (var _i = 0, _a = forms; _i < _a.length; _i++) {
        var item = _a[_i];
        var stringModel = item.getAttribute('model');
        var goto = item.getAttribute('goto');
        var sendto = item.getAttribute('sendto');
        var uploadto = item.getAttribute('uploadto');
        if (!sendto) {
            throw "attribute [sendto] was not set";
        }
        if (!uploadto) {
            throw 'attribute [uploadto] was not set';
        }
        if (!goto) {
            console.warn('attribute [goto] was not set, no redirection will be made after form submission');
        }
        var model = JSON.parse(stringModel);
        var form = new form_1.Form(model);
        form.goto = goto;
        form.sendto = sendto;
        form.uploadto = uploadto;
        for (var _b = 0, _c = model.Fields; _b < _c.length; _b++) {
            var field = _c[_b];
            form.addField(field);
        }
        form.element.enctype = 'multipart/form-data';
        form.element.method = 'post';
        form.attachForm(item);
        console.log(form);
        form.submitClick;
    }
});
//# sourceMappingURL=main.js.map