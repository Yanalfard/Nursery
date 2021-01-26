var tblForm = /** @class */ (function () {
    function tblForm() {
        this.Id = -1;
        this.Name = '';
        this.Body = '';
        this.IsDeleted = false;
    }
    return tblForm;
}());
var tblRegex = /** @class */ (function () {
    function tblRegex(regex, label) {
        this.Id = -1;
        this.Regex = new RegExp('');
        this.Label = '';
        this.IsDeleted = false;
        this.Regex = regex;
        this.Label = label !== null && label !== void 0 ? label : '';
    }
    return tblRegex;
}());
var tblField = /** @class */ (function () {
    function tblField() {
        this.Id = -1;
        this.FormId = -1;
        this.Label = '';
        this.Type = Type.text;
        this.IsRequired = false;
        this.Options = [];
        this.Placeholder = '';
        this.Tooltip = '';
        this.IsDeleted = false;
        this.validations = [];
    }
    return tblField;
}());
var tblValue = /** @class */ (function () {
    function tblValue() {
        this.Id = -1;
        this.UserId = -1;
        this.Value = '';
        this.IsDeleted = false;
    }
    return tblValue;
}());
var Type;
(function (Type) {
    Type[Type["combo"] = 0] = "combo";
    Type[Type["textarea"] = 1] = "textarea";
    Type[Type["checkbox"] = 2] = "checkbox";
    Type[Type["color"] = 3] = "color";
    Type[Type["date"] = 4] = "date";
    Type[Type["dateTime"] = 5] = "dateTime";
    Type[Type["email"] = 6] = "email";
    Type[Type["file"] = 7] = "file";
    Type[Type["hidden"] = 8] = "hidden";
    Type[Type["image"] = 9] = "image";
    Type[Type["month"] = 10] = "month";
    Type[Type["number"] = 11] = "number";
    Type[Type["password"] = 12] = "password";
    Type[Type["radio"] = 13] = "radio";
    Type[Type["range"] = 14] = "range";
    Type[Type["reset"] = 15] = "reset";
    Type[Type["search"] = 16] = "search";
    Type[Type["submit"] = 17] = "submit";
    Type[Type["tel"] = 18] = "tel";
    Type[Type["text"] = 19] = "text";
    Type[Type["time"] = 20] = "time";
    Type[Type["url"] = 21] = "url";
    Type[Type["week"] = 22] = "week";
})(Type || (Type = {}));
//#region Data Definition
var form1 = new tblForm();
form1.Id = 0;
form1.Name = 'این نام فرم می باشد';
var exp = new RegExp("[0-9]{11}");
var tblField1 = new tblField();
tblField1.Id = 0;
tblField1.FormId = 0;
tblField1.IsRequired = true;
tblField1.Type = Type.checkbox;
tblField1.Label = "مرا به خاطر بسپار";
var tblField2 = new tblField();
tblField2.Id = 1;
tblField2.FormId = 0;
tblField2.IsRequired = false;
tblField2.Type = Type.tel;
tblField2.validations.push(new tblRegex(exp, 'شماره تلفن صحیح نمی باشد'));
tblField2.Label = 'شماره تلفن';
var tblField3 = new tblField();
tblField3.Id = 2;
tblField3.FormId = 0;
tblField3.IsRequired = true;
tblField3.Type = Type.radio;
tblField3.Options = ['مرد', 'زن', 'ترجیح میدم نگم'];
tblField3.Label = 'جنسیت';
var tblField4 = new tblField();
tblField4.Id = 4;
tblField4.FormId = 0;
tblField4.IsRequired = false;
tblField4.Type = Type.combo;
tblField4.Options = ['سیب', 'موز', 'هلو', 'آلبالو'];
tblField4.Label = 'میوه مورد علاقه';
var tblField5 = new tblField();
tblField5.Id = 5;
tblField5.FormId = 0;
tblField5.IsRequired = true;
tblField5.Type = Type.text;
tblField5.Label = 'فیلد ضروری';
var tblFields = [tblField1, tblField2, tblField3, tblField4, tblField5];
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
var Form = /** @class */ (function () {
    function Form(tblForm) {
        this.Fields = [];
        this.data = tblForm;
        this.element = document.createElement('form');
        this.element.id = 'form';
        this.element.classList.add('form');
        //- header
        this.header = document.createElement('div');
        this.header.classList.add('form-header');
        this.header.innerHTML += "<h3>" + this.data.Name + "</h3>";
        //- body
        this.body = document.createElement('div');
        this.body.classList.add('form-body');
        //- footer
        this.footer = document.createElement('div');
        this.footer.classList.add('form-footer');
        //- submit
        this.submit = document.createElement('button');
        this.submit.classList.add('btn');
        this.submit.classList.add('btn-primary');
        this.submit.innerText = 'ثبت';
        this.submit.addEventListener('click', function (e) { e.preventDefault(); return null; });
        this.footer.appendChild(this.submit);
        this.element.appendChild(this.header);
        this.element.appendChild(this.body);
        this.element.appendChild(this.footer);
    }
    Form.prototype.addField = function (tblField) {
        var _this = this;
        var newField = new Field(tblField);
        this.Fields.push(newField);
        newField.element.childNodes.forEach(function (node) { return _this.body.appendChild(node); });
        return;
    };
    Form.prototype.attachForm = function (element) { return element.appendChild(this.element); };
    Form.prototype.validate = function () {
        var ans = true;
        for (var _i = 0, _a = this.Fields; _i < _a.length; _i++) {
            var field = _a[_i];
            if (!field.validate())
                ans = false;
        }
        return ans;
    };
    return Form;
}());
var Field = /** @class */ (function () {
    function Field(data) {
        var _this = this;
        var _a, _b, _c, _d, _e, _f, _g, _h, _j, _k, _l, _m, _o, _p, _q;
        this.data = data;
        var templateString = '';
        this.id = "I" + data.Label.split('').map(function (i) { return (i.charCodeAt(0) - 1700); }).join('') + "O";
        switch (data.Type) {
            case Type.checkbox:
                templateString =
                    "\n                <label class=\"fg-label uk-margin-auto-left row\" for=\"" + this.id + "\">" + data.Label + "\n                <input class=\"uk-checkbox\" id=\"" + this.id + "\" " + (data.IsRequired ? 'required' : '') + " type=\"checkbox\">\n                </label>\n                   ";
                break;
            case Type.textarea:
                templateString =
                    "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <textarea class=\"entry\" id=\"" + this.id + "\" " + (data.IsRequired ? 'required' : '') + " placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_a = data.validations[0]) === null || _a === void 0 ? void 0 : _a.Regex) + "\"></textarea>\n                <span class=\"text-danger\"></span>\n                   ";
                break;
            case Type.color:
                templateString =
                    "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" " + (data.IsRequired ? 'required' : '') + " type=\"color\">\n                   ";
                break;
            case Type.date:
                templateString =
                    "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" " + (data.IsRequired ? 'required' : '') + " type=\"date\" placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_b = data.validations[0]) === null || _b === void 0 ? void 0 : _b.Regex) + "\">\n                <span class=\"text-danger\"></span>\n                   ";
                break;
            case Type.dateTime:
                templateString =
                    "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" " + (data.IsRequired ? 'required' : '') + " type=\"dateTime\" placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_c = data.validations[0]) === null || _c === void 0 ? void 0 : _c.Regex) + "\">\n                <span class=\"text-danger\"></span>\n                   ";
                break;
            case Type.email:
                templateString =
                    "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" " + (data.IsRequired ? 'required' : '') + " type=\"email\" placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_d = data.validations[0]) === null || _d === void 0 ? void 0 : _d.Regex) + "\">\n                <span class=\"text-danger\"></span>\n                   ";
                break;
            case Type.file:
                templateString =
                    "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" " + (data.IsRequired ? 'required' : '') + " type=\"file\">\n                <span class=\"text-danger\"></span>\n                   ";
                break;
            case Type.hidden:
                templateString =
                    "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" " + (data.IsRequired ? 'required' : '') + " type=\"hidden\">\n                <span class=\"text-danger\"></span>\n                   ";
                break;
            case Type.image:
                templateString =
                    "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" " + (data.IsRequired ? 'required' : '') + " alt=\"" + data.Label + "\" type=\"image\">\n                <span class=\"text-danger\"></span>\n                   ";
                break;
            case Type.month:
                templateString =
                    "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" " + (data.IsRequired ? 'required' : '') + " type=\"month\" placeholder=\"" + data.Placeholder + "\">\n                <span class=\"text-danger\"></span>\n                   ";
                break;
            case Type.number:
                templateString =
                    "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" " + (data.IsRequired ? 'required' : '') + " type=\"number\" placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_e = data.validations[0]) === null || _e === void 0 ? void 0 : _e.Regex) + "\">\n                <span class=\"text-danger\"></span>\n                   ";
                break;
            case Type.password:
                templateString =
                    "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" " + (data.IsRequired ? 'required' : '') + " type=\"password\" placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_f = data.validations[0]) === null || _f === void 0 ? void 0 : _f.Regex) + "\">\n                <span class=\"text-danger\"></span>\n                   ";
                break;
            case Type.radio:
                templateString =
                    "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                " + data.Options.map(function (o) { return "<label class=\"radio\"><span>" + o + "</span><input id=\"" + (data.Label + data.Options.indexOf(o)) + "\" name=\"" + _this.id + "\" class=\"uk-radio\" value=\"" + o + "\" type=\"radio\"></label>"; }).join('') + "\n                    ";
                break;
            case Type.range:
                templateString =
                    "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"uk-range\" id=\"" + data.Label.toString() + "\" " + (data.IsRequired ? 'required' : '') + " min=\"" + data.Options[0] + "\" max=\"" + data.Options[1] + "\" type=\"range\" placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_g = data.validations[0]) === null || _g === void 0 ? void 0 : _g.Regex) + "\">\n                   ";
                break;
            case Type.reset:
                templateString =
                    "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" type=\"reset\" " + (data.IsRequired ? 'required' : '') + " placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_h = data.validations[0]) === null || _h === void 0 ? void 0 : _h.Regex) + "\">\n                   ";
                break;
            case Type.search:
                templateString =
                    "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" type=\"search\" " + (data.IsRequired ? 'required' : '') + " placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_j = data.validations[0]) === null || _j === void 0 ? void 0 : _j.Regex) + "\">\n                <span class=\"text-danger\"></span>\n                   ";
                break;
            case Type.submit:
                templateString =
                    "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" type=\"submit\" " + (data.IsRequired ? 'required' : '') + " placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_k = data.validations[0]) === null || _k === void 0 ? void 0 : _k.Regex) + "\">\n                   ";
                break;
            case Type.tel:
                templateString =
                    "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" type=\"tel\" " + (data.IsRequired ? 'required' : '') + " placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_l = data.validations[0]) === null || _l === void 0 ? void 0 : _l.Regex) + "\">\n                <span class=\"text-danger\"></span>\n                   ";
                break;
            case Type.text:
                templateString =
                    "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" type=\"text\" " + (data.IsRequired ? 'required' : '') + " placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_m = data.validations[0]) === null || _m === void 0 ? void 0 : _m.Regex) + "\">\n                <span class=\"text-danger\"></span>\n                   ";
                break;
            case Type.time:
                templateString =
                    "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" type=\"time\" " + (data.IsRequired ? 'required' : '') + " placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_o = data.validations[0]) === null || _o === void 0 ? void 0 : _o.Regex) + "\">\n                <span class=\"text-danger\"></span>\n                   ";
                break;
            case Type.url:
                templateString =
                    "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" type=\"url\" " + (data.IsRequired ? 'required' : '') + " placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_p = data.validations[0]) === null || _p === void 0 ? void 0 : _p.Regex) + "\">\n                <span class=\"text-danger\"></span>\n                   ";
                break;
            case Type.week:
                templateString =
                    "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <input class=\"entry\" id=\"" + this.id + "\" " + (data.IsRequired ? 'required' : '') + " type=\"week\" placeholder=\"" + data.Placeholder + "\" pattern=\"" + ((_q = data.validations[0]) === null || _q === void 0 ? void 0 : _q.Regex) + "\">\n                <span class=\"text-danger\"></span>\n                   ";
                break;
            case Type.combo:
                templateString =
                    "\n                <label class=\"fg-label\" for=\"" + this.id + "\">" + data.Label + "</label>\n                <select class=\"entry\" id=\"" + this.id + "\">\n                    " + data.Options.map(function (o) { return ("<option>" + o + "</option>"); }).join('') + "\n                </select>\n                   ";
                break;
            default:
                console.log('type unknown');
                break;
        }
        //fg-col
        templateString = "<div class=\"fg-col " + (data.IsRequired ? 'fg-required' : '') + "\">" + templateString + "</div>";
        var parser = new DOMParser();
        this.element = (parser.parseFromString(templateString, 'text/html').body);
        this.lblError = this.element.querySelector('.text-danger');
    }
    /** Gets the field's Value.*/
    Field.prototype.getVal = function () {
        var ans = new tblValue();
        ans.Value = null;
        var input;
        switch (this.data.Type) {
            case Type.checkbox:
                input = document.querySelector("input#" + this.id + "[type=\"checkbox\"]");
                ans.Value = input.checked;
                break;
            case Type.combo:
                input = document.querySelector("select#" + this.id);
                ans.Value = input.value;
                break;
            case Type.radio:
                input = document.querySelector("input[name=\"" + this.id + "\"][type=\"radio\"]:checked");
                ans.Value = input === null || input === void 0 ? void 0 : input.value;
                break;
            case Type.file:
                input = document.querySelector("input#" + this.id + "[type=\"file\"]");
                ans.Value = input.value;
                break;
            case Type.image:
                input = document.querySelector("input#" + this.id + "[type=\"image\"]");
                ans.Value = input.value;
                break;
            default:
                input = document.querySelector("#" + this.id);
                ans.Value = input.value;
                break;
        }
        return ans;
    };
    /** Validates the field's Value.
     * Shows the first error in it's validation list.
     * Clears the Errorprovider if there are no errors */
    Field.prototype.validate = function () {
        if (this.data.IsRequired) {
            if (this.getVal() === null
                || this.getVal() === undefined
                || this.getVal().Value === null
                || this.getVal().Value === ''
                || this.getVal().Value === undefined) {
                if (this.lblError)
                    this.lblError.innerText = 'این فیلد اجباری است';
                return false;
            }
        }
        for (var _i = 0, _a = this.data.validations; _i < _a.length; _i++) {
            var validationRule = _a[_i];
            if (!validationRule.Regex.test(this.getVal().Value)) {
                if (this.lblError)
                    this.lblError.innerText = validationRule.Label;
                return false;
            }
        }
        if (this.lblError)
            this.lblError.innerText = '';
        return true;
    };
    return Field;
}());
var oi = new Form(form1);
tblFields.forEach(function (f) { return oi.addField(f); });
oi.attachForm(document.getElementById('container'));
oi.submit.addEventListener('click', submit);
function submit() {
    //oi.Fields.forEach(i => console.log(i.getVal()))
    //oi.validate();
    if (!oi.validate())
        return;
    oi.Fields.forEach(function (i) { return console.log(i.getVal()); });
    //console.log(oi.Fields[0].getVal());
}
//# sourceMappingURL=main.js.map