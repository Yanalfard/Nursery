var tblForm = /** @class */ (function () {
    function tblForm() {
    }
    return tblForm;
}());
var tblRegex = /** @class */ (function () {
    function tblRegex(regex, label) {
        this.Regex = regex;
        this.Label = label !== null && label !== void 0 ? label : '';
    }
    return tblRegex;
}());
var tblField = /** @class */ (function () {
    function tblField() {
        this.Options = [];
        this.validations = [];
    }
    return tblField;
}());
;
var tblValue = /** @class */ (function () {
    function tblValue() {
    }
    return tblValue;
}());
var Type;
(function (Type) {
    Type[Type["combo"] = 0] = "combo";
    Type[Type["checkbox"] = 1] = "checkbox";
    Type[Type["color"] = 2] = "color";
    Type[Type["date"] = 3] = "date";
    Type[Type["dateTime"] = 4] = "dateTime";
    Type[Type["email"] = 5] = "email";
    Type[Type["file"] = 6] = "file";
    Type[Type["hidden"] = 7] = "hidden";
    Type[Type["image"] = 8] = "image";
    Type[Type["month"] = 9] = "month";
    Type[Type["number"] = 10] = "number";
    Type[Type["password"] = 11] = "password";
    Type[Type["radio"] = 12] = "radio";
    Type[Type["range"] = 13] = "range";
    Type[Type["reset"] = 14] = "reset";
    Type[Type["search"] = 15] = "search";
    Type[Type["submit"] = 16] = "submit";
    Type[Type["tel"] = 17] = "tel";
    Type[Type["text"] = 18] = "text";
    Type[Type["time"] = 19] = "time";
    Type[Type["url"] = 20] = "url";
    Type[Type["week"] = 21] = "week";
})(Type || (Type = {}));
//#region Data Definition
var form1 = new tblForm();
form1.Id = 0;
form1.Name = 'این نام فرم می باشد';
var exp = new RegExp("[0-9]{11}");
var tblField1 = new tblField();
tblField1.Id = 0;
tblField1.FormId = 0;
tblField1.Type = Type.checkbox;
tblField1.Label = "مرا به خاطر بسپار";
var tblField2 = new tblField();
tblField2.Id = 1;
tblField2.FormId = 0;
tblField2.Type = Type.tel;
tblField2.validations.push(new tblRegex(exp));
tblField2.Label = 'شماره تلفن';
var tblField3 = new tblField();
tblField3.Id = 2;
tblField3.FormId = 0;
tblField3.Type = Type.radio;
tblField3.Options = ['مرد', 'زن', 'ترجیح میدم نگم'];
tblField3.Label = 'جنسیت';
var tblField4 = new tblField();
tblField4.Id = 4;
tblField4.FormId = 0;
tblField4.Type = Type.combo;
tblField4.Options = ['سیب', 'موز', 'هلو', 'آلبالو'];
tblField4.Label = 'میوه مورد علاقه';
var tblFields = [tblField1, tblField2, tblField3, tblField4];
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
        var newField = new Field(tblField);
        this.Fields.push(newField);
        this.body.innerHTML += newField.element;
        return;
    };
    Form.prototype.attachForm = function (element) { return element.appendChild(this.element); };
    Form.prototype.getInputValues = function () {
        var ans = [];
        for (var _i = 0, _a = this.Fields; _i < _a.length; _i++) {
            var f = _a[_i];
            ans.push(f.getVal());
        }
        //return this.Fields.map(f => f.getVal());
        return ans;
    };
    return Form;
}());
var Field = /** @class */ (function () {
    function Field(data) {
        this.data = data;
        switch (data.Type) {
            case Type.checkbox:
                this.element =
                    "<div class=\"fg-col\">\n                        <label class=\"fg-label\" for=\"" + data.Label + "\">" + data.Label + "</label>\n                        <input class=\"entry\" name=\"" + data.Label + "\" type=\"checkbox\">\n                        <span class=\"text-danger\"></span>\n                    </div>";
                break;
            case Type.color:
                this.element =
                    "<div class=\"fg-col\">\n                <label class=\"fg-label\" for=\"" + data.Label + "\">" + data.Label + "</label>\n                <input class=\"entry\" name=\"" + data.Label + "\" type=\"color\">\n                <span class=\"text-danger\"></span>\n            </div>";
                break;
            case Type.date:
                this.element =
                    "<div class=\"fg-col\">\n                <label class=\"fg-label\" for=\"" + data.Label + "\">" + data.Label + "</label>\n                <input class=\"entry\" name=\"" + data.Label + "\" type=\"date\" placeholder=\"" + data.Label + "\" pattern=\"" + data.validations[0].Regex + "\">\n                <span class=\"text-danger\"></span>\n            </div>";
                break;
            case Type.dateTime:
                this.element =
                    "<div class=\"fg-col\">\n                <label class=\"fg-label\" for=\"" + data.Label + "\">" + data.Label + "</label>\n                <input class=\"entry\" name=\"" + data.Label + "\" type=\"dateTime\" placeholder=\"" + data.Label + "\" pattern=\"" + data.validations[0].Regex + "\">\n                <span class=\"text-danger\"></span>\n            </div>";
                break;
            case Type.email:
                this.element =
                    "<div class=\"fg-col\">\n                <label class=\"fg-label\" for=\"" + data.Label + "\">" + data.Label + "</label>\n                <input class=\"entry\" name=\"" + data.Label + "\" type=\"email\" placeholder=\"" + data.Label + "\" pattern=\"" + data.validations[0].Regex + "\">\n                <span class=\"text-danger\"></span>\n            </div>";
                break;
            case Type.file:
                this.element =
                    "<div class=\"fg-col\">\n                <label class=\"fg-label\" for=\"" + data.Label + "\">" + data.Label + "</label>\n                <input class=\"entry\" name=\"" + data.Label + "\" type=\"file\">\n                <span class=\"text-danger\"></span>\n            </div>";
                break;
            case Type.hidden:
                this.element =
                    "<div class=\"fg-col\">\n                <label class=\"fg-label\" for=\"" + data.Label + "\">" + data.Label + "</label>\n                <input class=\"entry\" name=\"" + data.Label + "\" type=\"hidden\">\n                <span class=\"text-danger\"></span>\n            </div>";
                break;
            case Type.image:
                this.element =
                    "<div class=\"fg-col\">\n                <label class=\"fg-label\" for=\"" + data.Label + "\">" + data.Label + "</label>\n                <input class=\"entry\" name=\"" + data.Label + "\" type=\"image\">\n                <span class=\"text-danger\"></span>\n            </div>";
                break;
            case Type.month:
                this.element =
                    "<div class=\"fg-col\">\n                <label class=\"fg-label\" for=\"" + data.Label + "\">" + data.Label + "</label>\n                <input class=\"entry\" name=\"" + data.Label + "\" type=\"month\" placeholder=\"" + data.Label + "\">\n                <span class=\"text-danger\"></span>\n            </div>";
                break;
            case Type.number:
                this.element =
                    " <div class=\"fg-col\">\n                <label class=\"fg-label\" for=\"" + data.Label + "\">" + data.Label + "</label>\n                <input class=\"entry\" name=\"" + data.Label + "\" type=\"number\" placeholder=\"" + data.Label + "\" pattern=\"" + data.validations[0].Regex + "\">\n                <span class=\"text-danger\"></span>\n            </div>";
                break;
            case Type.password:
                this.element =
                    "<div class=\"fg-col\">\n                <label class=\"fg-label\" for=\"" + data.Label + "\">" + data.Label + "</label>\n                <input class=\"entry\" name=\"" + data.Label + "\" type=\"password\" placeholder=\"" + data.Label + "\" pattern=\"" + data.validations[0].Regex + "\">\n                <span class=\"text-danger\"></span>\n            </div>";
                break;
            case Type.radio:
                this.element =
                    "<div class=\"fg-col\">\n                        <label class=\"fg-label\" for=\"" + data.Label + "\">" + data.Label + "</label>\n                        " + data.Options.map(function (o) { return "<label class=\"radio\"><span>" + o + "</span><input name=\"" + data.Label + "\" value=\"" + o + "\" type=\"radio\"></label>"; }).join('') + "\n                        <span class=\"text-danger\"></span>\n                    </div>";
                break;
            case Type.range:
                this.element =
                    "<div class=\"fg-col\">\n                        <label class=\"fg-label\" for=\"" + data.Label + "\">" + data.Label + "</label>\n                        <input class=\"entry\" name=\"" + data.Label + "\" type=\"range\" placeholder=\"" + data.Label + "\" pattern=\"" + data.validations[0].Regex + "\">\n                        <span class=\"text-danger\"></span>\n                    </div>";
                break;
            case Type.reset:
                this.element =
                    "<div class=\"fg-col\">\n                <label class=\"fg-label\" for=\"" + data.Label + "\">" + data.Label + "</label>\n                <input class=\"entry\" name=\"" + data.Label + "\" type=\"reset\" placeholder=\"" + data.Label + "\" pattern=\"" + data.validations[0].Regex + "\">\n                <span class=\"text-danger\"></span>\n            </div>";
                break;
            case Type.search:
                this.element =
                    "<div class=\"fg-col\">\n                <label class=\"fg-label\" for=\"" + data.Label + "\">" + data.Label + "</label>\n                <input class=\"entry\" name=\"" + data.Label + "\" type=\"search\" placeholder=\"" + data.Label + "\" pattern=\"" + data.validations[0].Regex + "\">\n                <span class=\"text-danger\"></span>\n            </div>";
                break;
            case Type.submit:
                this.element =
                    "<div class=\"fg-col\">\n                <label class=\"fg-label\" for=\"" + data.Label + "\">" + data.Label + "</label>\n                <input class=\"entry\" name=\"" + data.Label + "\" type=\"submit\" placeholder=\"" + data.Label + "\" pattern=\"" + data.validations[0].Regex + "\">\n                <span class=\"text-danger\"></span>\n            </div>";
                break;
            case Type.tel:
                this.element =
                    "<div class=\"fg-col\">\n                <label class=\"fg-label\" for=\"" + data.Label + "\">" + data.Label + "</label>\n                <input class=\"entry\" name=\"" + data.Label + "\" type=\"tel\" placeholder=\"" + data.Label + "\" pattern=\"" + data.validations[0].Regex + "\">\n                <span class=\"text-danger\"></span>\n            </div>";
                break;
            case Type.text:
                this.element =
                    "<div class=\"fg-col\">\n                <label class=\"fg-label\" for=\"" + data.Label + "\">" + data.Label + "</label>\n                <input class=\"entry\" name=\"" + data.Label + "\" type=\"text\" placeholder=\"" + data.Label + "\" pattern=\"" + data.validations[0].Regex + "\">\n                <span class=\"text-danger\"></span>\n            </div>";
                break;
            case Type.time:
                this.element =
                    "<div class=\"fg-col\">\n                <label class=\"fg-label\" for=\"" + data.Label + "\">" + data.Label + "</label>\n                <input class=\"entry\" name=\"" + data.Label + "\" type=\"time\" placeholder=\"" + data.Label + "\" pattern=\"" + data.validations[0].Regex + "\">\n                <span class=\"text-danger\"></span>\n            </div>";
                break;
            case Type.url:
                this.element =
                    "<div class=\"fg-col\">\n                <label class=\"fg-label\" for=\"" + data.Label + "\">" + data.Label + "</label>\n                <input class=\"entry\" name=\"" + data.Label + "\" type=\"url\" placeholder=\"" + data.Label + "\" pattern=\"" + data.validations[0].Regex + "\">\n                <span class=\"text-danger\"></span>\n            </div>";
                break;
            case Type.week:
                this.element =
                    "<div class=\"fg-col\">\n                <label class=\"fg-label\" for=\"" + data.Label + "\">" + data.Label + "</label>\n                <input class=\"entry\" name=\"" + data.Label + "\" type=\"week\" placeholder=\"" + data.Label + "\" pattern=\"" + data.validations[0].Regex + "\">\n                <span class=\"text-danger\"></span>\n            </div>";
                break;
            case Type.combo:
                this.element =
                    "<div class=\"fg-col\">\n                <label class=\"fg-label\" for=\"" + data.Label + "\">" + data.Label + "</label>\n                <select class=\"entry\" name=\"" + data.Label + "\">\n                    " + data.Options.map(function (o) { return ("<option>" + o + "</option>"); }).join('') + "\n                </select>\n                <span class=\"text-danger\"></span>\n            </div>";
                break;
            default:
                console.log('type unknown');
                break;
        }
    }
    Field.prototype.getVal = function () {
        var ans = new tblValue();
        var input;
        switch (this.data.Type) {
            case Type.checkbox:
                input = document.querySelector("input[name=\"" + this.data.Label + "\"][type=\"checkbox\"]");
                ans.Value = input.checked;
                console.log(input);
                break;
            case Type.combo:
                input = document.querySelector("select[name=\"" + this.data.Label + "\"]");
                ans.Value = input.value;
                console.log(input);
                break;
            case Type.radio:
                input = document.querySelector("input[name=\"" + this.data.Label + "\"][type=\"radio\"]");
                ans.Value = input.value;
                console.log(input);
                break;
            case Type.file:
                input = document.querySelector("input[name=\"" + this.data.Label + "\"][type=\"file\"]");
                ans.Value = input.value;
                console.log(input);
                break;
            case Type.image:
                input = document.querySelector("input[name=\"" + this.data.Label + "\"][type=\"image\"]");
                ans.Value = input.value;
                console.log(input);
                break;
            default:
        }
        return;
    };
    return Field;
}());
var oi = new Form(form1);
tblFields.forEach(function (f) { return oi.addField(f); });
oi.attachForm(document.getElementById('container'));
oi.submit.addEventListener('click', function () { oi.Fields.forEach(function (i) { return console.log(i.getVal()); }); });
//# sourceMappingURL=main.js.map