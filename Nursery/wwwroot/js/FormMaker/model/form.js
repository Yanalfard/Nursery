var __spreadArray = (this && this.__spreadArray) || function (to, from) {
    for (var i = 0, il = from.length, j = to.length; i < il; i++, j++)
        to[j] = from[i];
    return to;
};
define(["require", "exports", "./field", "./inputType"], function (require, exports, field_1, inputType_1) {
    "use strict";
    exports.__esModule = true;
    exports.Form = void 0;
    var Form = /** @class */ (function () {
        function Form(tblForm) {
            var _this = this;
            this.Fields = [];
            this.Uploadables = [];
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
            this.submit.innerHTML = '<span>ثبت</span>';
            this.submit.addEventListener('click', function (e) { _this.submitClick(e); });
            this.footer.appendChild(this.submit);
            this.element.appendChild(this.header);
            this.element.appendChild(this.body);
            this.element.appendChild(this.footer);
        }
        Form.prototype.addField = function (tblField) {
            var _this = this;
            var newField = new field_1.Field(tblField);
            if (tblField.Type == inputType_1.InputType.file || tblField.Type == inputType_1.InputType.image) {
                this.Uploadables.push(newField);
            }
            else {
                this.Fields.push(newField);
            }
            newField.element.childNodes.forEach(function (node) { return _this.body.appendChild(node); });
            return;
        };
        ;
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
        Form.prototype.submitClick = function (e) {
            var _this = this;
            //e.preventDefault(); return null;
            e.preventDefault();
            if (!this.validate())
                return null;
            var res = [];
            this.Fields.forEach(function (i) {
                var val = i.getVal();
                val.FormFieldId = i.data.FieldId;
                val.FormId = _this.data.FormId;
                res.push(val);
            });
            eval('LoadingRun();');
            var formData = new FormData(this.element);
            console.log(formData.get('files'));
            try {
                // Upload files first
                fetch(this.uploadto, {
                    method: 'POST',
                    body: formData
                }).then(function (response) {
                    (response.json().then(function (filenames) {
                        // Recieve file names
                        // Put FormId and FieldId back to values
                        for (var file = 0; file < filenames.length; file++) {
                            filenames[file].FormFieldId = _this.Uploadables[file].data.FieldId;
                            filenames[file].FormId = _this.Uploadables[file].data.FormId;
                        }
                        // Combine values to send and fileName
                        res = __spreadArray(__spreadArray([], res), filenames);
                        // Then send data
                        fetch(_this.sendto, {
                            method: 'post',
                            mode: 'cors',
                            cache: 'no-cache',
                            credentials: 'same-origin',
                            headers: {
                                'Content-Type': 'application/json'
                            },
                            body: JSON.stringify(res)
                        }).then(function (response) {
                            console.log('send data response', response);
                            // Go to wherever after submission
                            window.location.href = _this.element.getAttribute('goto');
                        })["catch"](function () {
                            eval('LoadingEnd();');
                        });
                    }));
                })["catch"]();
            }
            catch (error) {
                console.error('Error:', error);
            }
            //this.element.submit();
        };
        return Form;
    }());
    exports.Form = Form;
});
//# sourceMappingURL=form.js.map