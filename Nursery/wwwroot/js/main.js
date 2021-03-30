"use strict";
exports.__esModule = true;
var form_1 = require("./model/form");
//#region Data Definition
//const form1 = new tblForm();
//form1.Id = 0;
//form1.Name = 'این نام فرم می باشد';
//const exp = new RegExp("[0-9]{11}");
//const tblField1 = new tblField();
//tblField1.Id = 0;
//tblField1.FormId = 0;
//tblField1.IsRequired = true;
//tblField1.Type = Type.checkbox;
//tblField1.Label = "مرا به خاطر بسپار";
//const tblField2 = new tblField();
//tblField2.Id = 1;
//tblField2.FormId = 0;
//tblField2.IsRequired = false;
//tblField2.Type = Type.tel;
//tblField2.validations.push(new tblRegex(exp, 'شماره تلفن صحیح نمی باشد'));
//tblField2.Label = 'شماره تلفن';
//const tblField3 = new tblField();
//tblField3.Id = 2;
//tblField3.FormId = 0;
//tblField3.IsRequired = true;
//tblField3.Type = Type.radio;
//tblField3.Options = ['مرد', 'زن', 'ترجیح میدم نگم'];
//tblField3.Label = 'جنسیت';
//const tblField4 = new tblField();
//tblField4.Id = 4;
//tblField4.FormId = 0;
//tblField4.IsRequired = false;
//tblField4.Type = Type.combo;
//tblField4.Options = ['سیب', 'موز', 'هلو', 'آلبالو'];
//tblField4.Label = 'میوه مورد علاقه';
//const tblField5 = new tblField();
//tblField5.Id = 5;
//tblField5.FormId = 0;
//tblField5.IsRequired = true;
//tblField5.Type = Type.text;
//tblField5.Label = 'فیلد ضروری';
//const tblFields = [tblField1, tblField2, tblField3, tblField4, tblField5];
//const tblFields = [];
//for (var i = 0; i < 21; i++) {
//    const f = new tblField();
//    f.Id = i;
//    f.FormId = 0;
//    f.Type = i;
//    f.Options = ['سیب', 'موز', 'هلو', 'آلبالو'];
//    f.Label = Type[i];
//    f.validations.push(new tblRegex(exp, 'Error'));
//    tblFields.push(f);
//}
//#endregion
//-
var forms = document.querySelectorAll('dform');
for (var _i = 0, forms_1 = forms; _i < forms_1.length; _i++) {
    var item = forms_1[_i];
    var stringModel = item.getAttribute('model');
    var model = JSON.parse(stringModel);
    var form = new form_1.Form(model);
    for (var _a = 0, _b = model.Fields; _a < _b.length; _a++) {
        var field = _b[_a];
        form.addField(field);
    }
    form.attachForm(item);
    console.log(form);
}
//# sourceMappingURL=main.js.map