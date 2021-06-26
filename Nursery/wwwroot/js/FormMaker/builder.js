define(["require", "exports", "./model/db/tblField", "./model/db/tblForm", "./model/inputType", "./model/tool"], function (require, exports, tblField_1, tblForm_1, inputType_1, tool_1) {
    "use strict";
    exports.__esModule = true;
    // Toolbox Element Recreation implementation
    var toolbox = document.getElementById('toolbox');
    var toolboxHTML = toolbox.innerHTML;
    var btnFinish = document.getElementById('btn-finish');
    btnFinish.addEventListener('click', function () {
        var temp = [];
        componentList.forEach(function (tool) {
            var index = parseInt(tool.element.getAttribute('order'));
            temp[index] = tool;
        });
        componentList = temp;
        var body = new tblForm_1.TblForm();
        var formName = document.getElementById('txtFormName').value;
        var description = document.getElementById('txtDecsription').value;
        var priority = document.getElementById('txtPriority').value;
        //#region  validated 
        if (!formName) {
            alert('نام فرم را وارد کنید');
            return;
        }
        if (!description) {
            alert('توضیحات فرم را وارد کنید');
            return;
        }
        //#endregion
        eval('LoadingRun();');
        body.Name = formName;
        body.Body = description;
        body.Priority = parseInt(priority);
        componentList.forEach(function (f) {
            var _a, _b, _c, _d;
            var field = new tblField_1.TblField();
            // Options
            field.IsRequired = ((_a = f.options.filter(function (option) { return option.name == 'IsRequired'; })[0]) === null || _a === void 0 ? void 0 : _a.value) == 'true';
            field.Placeholder = (_b = f.options.filter(function (option) { return option.name == 'Placeholder'; })[0]) === null || _b === void 0 ? void 0 : _b.value;
            field.Label = (_c = f.options.filter(function (option) { return option.name == 'Label'; })[0]) === null || _c === void 0 ? void 0 : _c.value;
            field.Tooltip = (_d = f.options.filter(function (option) { return option.name == 'Tooltip'; })[0]) === null || _d === void 0 ? void 0 : _d.value;
            field.Type = f.type;
            // Select
            field.Options = f.selects.map(function (i) { return i.value; }).join(',');
            // Regex
            field.Validations = f.regexs.map(function (i) { return i.tblRegex; });
            body.Fields.push(field);
        });
        debugger;
        fetch('/Admin/Form/Create', {
            method: 'post',
            mode: 'cors',
            cache: 'no-cache',
            credentials: 'same-origin',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(body)
        }).then(function (response) {
            window.location.href = btnFinish.getAttribute('goto');
        })["catch"](function () {
            eval('LoadingEnd();');
        });
    });
    toolbox.addEventListener('removed', function () {
        toolbox.innerHTML = toolboxHTML;
    });
    //toolbox.addEventListener('added', () => {
    //    toolbox.innerHTML = toolboxHTML;
    //});
    var componentList = [];
    // Prototype Data Construction
    var prototype = document.getElementById('prototype');
    prototype.addEventListener('start', drag);
    prototype.addEventListener('stop', drag);
    prototype.addEventListener('moved', enumerateTools);
    function enumerateTools() {
        var counter = 0;
        prototype.childNodes.forEach(function (formElement) {
            if (formElement.nodeType === 1) {
                var i = formElement;
                i.setAttribute('order', counter.toString());
                counter++;
            }
        });
    }
    prototype.addEventListener('added', function (event) {
        var el = event.detail[1];
        var toolModel = new tool_1.Tool();
        toolModel.element = el;
        toolModel.type = inputType_1.InputType[el.querySelector('[tool]').getAttribute('tool').toLowerCase()];
        toolModel.label = el.querySelector('[tool-label]');
        toolModel.preview = el.querySelector('.tool-content');
        toolModel.btnDelete = toolModel.element.querySelector('[btnDelete]');
        toolModel.btnSettings = toolModel.element.querySelector('[btnSettings]');
        // btnSettings_Click
        toolModel.btnSettings.addEventListener('click', function () {
            toolModel.optionsList.style.display = toolModel.optionsList.style.display === 'none' ? 'block' : 'none';
        });
        // btnDelete_Click
        toolModel.btnDelete.addEventListener('click', function () {
            toolModel.element.style.transition = 'all ease 0.3s';
            toolModel.element.style.opacity = '0';
            setTimeout(function () {
                toolModel.element.parentElement.removeChild(toolModel.element);
                componentList = componentList.splice(componentList.indexOf(toolModel, 1));
            }, 300);
        });
        // option
        toolModel.optionsList = toolModel.element.querySelector('[option-list]');
        // Option
        toolModel.element.querySelectorAll('[option]').forEach(function (div) {
            var element = div;
            var mainElement = div.querySelector('[option-name]');
            var type = element.getAttribute("option");
            var name = mainElement.getAttribute("option-name");
            var optionModel = new tool_1.Option(element, tool_1.OptionType[type], name);
            if (name === 'Label') {
                var input_1 = element.querySelector('input');
                var def_1 = toolModel.label.innerText;
                input_1.addEventListener('input', function () {
                    toolModel.label.innerText = input_1.value || def_1;
                });
            }
            else if (name === 'Placeholder') {
                var input_2 = element.querySelector('input[type=text]');
                var output_1 = toolModel.preview.querySelector('input') || toolModel.preview.querySelector('textarea');
                if (output_1) {
                    input_2.addEventListener('input', function () {
                        output_1.placeholder = input_2.value;
                    });
                }
            }
            else if (name === 'IsRequired') {
                var col_1 = toolModel.preview.querySelector('.fg-col');
                var check_1 = element.querySelector('input[type=checkbox]');
                check_1.addEventListener('change', function () {
                    if (check_1.checked)
                        col_1.classList.add('fg-required');
                    else
                        col_1.classList.remove('fg-required');
                });
            }
            toolModel.options.push(optionModel);
        });
        // Regex
        var regexList = toolModel.element.querySelector('[regex-list]');
        if (regexList) {
            toolModel.regexList = regexList;
            toolModel.regexSelect = regexList.querySelector('[regex-select]');
            toolModel.regexAdder = regexList.querySelector('.regex-list');
            toolModel.regexSelect.addEventListener('change', function () {
                var val = JSON.parse(toolModel.regexSelect.value);
                var selectedOption = toolModel.regexSelect.selectedOptions[0];
                toolModel.regexSelect.selectedIndex = 0;
                if (!selectedOption.getAttribute('value'))
                    return;
                selectedOption.style.display = 'none';
                var template = "<div class=\"regex-item\">\n                    <label regex=\"" + val.Name + "\" class=\"cell-label\">" + selectedOption.innerText + "</label>\n                    <button type=\"button\" class=\"cell-btn\" uk-icon=\"times\"></button>\n                </div>";
                var doc = new DOMParser().parseFromString(template, 'text/html');
                var reg = new tool_1.Regex(val);
                reg.element = doc.querySelector('.regex-item');
                reg.label = doc.querySelector('[regex]');
                reg.button = doc.querySelector('button');
                reg.element.appendChild(reg.label);
                reg.element.append(reg.button);
                // delete
                reg.button.addEventListener('click', function () {
                    toolModel.regexAdder.removeChild(reg.element);
                    selectedOption.style.display = 'block';
                    toolModel.regexs.splice(toolModel.regexs.indexOf(reg), 1);
                });
                toolModel.regexs.push(reg);
                toolModel.regexAdder.appendChild(reg.element);
            });
        }
        // Select
        var selectElement = toolModel.element.querySelector('[select]');
        if (selectElement) {
            toolModel.selectElement = selectElement;
            toolModel.selectAdder = selectElement.querySelector('[select-adder]');
            toolModel.selectInput = toolModel.selectAdder.querySelector('[select-input]');
            toolModel.selectBtn = toolModel.selectAdder.querySelector('[select-btn]');
            toolModel.selectList = selectElement.querySelector('[select-list]');
            // add
            toolModel.selectBtn.addEventListener('click', function () {
                var val = toolModel.selectInput.value;
                toolModel.selectInput.value = '';
                if (!val)
                    return;
                var template = "<div class=\"select-item\">\n                    <label select-item=\"" + val + "\" class=\"cell-label\">" + val + "</label>\n                    <button btnSelect class=\"cell-btn cell-border-right\" uk-icon=\"times\"></button>\n                </div>";
                var doc = new DOMParser().parseFromString(template, 'text/html');
                var sel = new tool_1.Select();
                sel.value = val;
                sel.element = doc.querySelector('.select-item');
                sel.label = doc.querySelector('[select-item]');
                sel.button = doc.querySelector('[btnSelect]');
                // delete
                sel.button.addEventListener('click', function () {
                    toolModel.selects.splice(toolModel.selects.indexOf(sel), 1);
                    toolModel.selectList.removeChild(sel.element);
                });
                sel.element.appendChild(sel.label);
                sel.element.appendChild(sel.button);
                toolModel.selects.push(sel);
                toolModel.selectList.appendChild(sel.element);
            });
        }
        componentList.push(toolModel);
        enumerateTools();
    });
    prototype.addEventListener('removed', function (event) {
        var element = event.detail[1];
        var tool = componentList.filter(function (i) { return i.element == element; })[0];
        componentList.splice(componentList.indexOf(tool), 1);
        element.parentElement.removeChild(element);
        enumerateTools();
    });
    function drag() {
    }
});
//# sourceMappingURL=builder.js.map