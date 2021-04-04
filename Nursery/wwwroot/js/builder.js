define(["require", "exports", "./model/inputType", "./model/tool"], function (require, exports, inputType_1, tool_1) {
    "use strict";
    exports.__esModule = true;
    // Toolbox Element Recreation implementation
    var toolbox = document.getElementById('toolbox');
    var toolboxHTML = toolbox.innerHTML;
    var btnFinish = document.getElementById('btn-finish');
    btnFinish.addEventListener('click', function () {
        var temp;
        componentList.forEach(function (tool) {
            var index = parseInt(tool.element.getAttribute('order'));
            temp[index] = tool;
        });
        componentList = temp;
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
                delete toolModel.element;
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
            toolModel.options.push(optionModel);
        });
        // Regex
        var regexList = toolModel.element.querySelector('[regex-list]');
        if (regexList) {
            toolModel.regexList = regexList;
            toolModel.regexSelect = regexList.querySelector('[regex-select]');
            toolModel.regexAdder = regexList.querySelector('.regex-list');
            toolModel.regexSelect.addEventListener('change', function () {
                var val = toolModel.regexSelect.value;
                var selectedOption = toolModel.regexSelect.selectedOptions[0];
                toolModel.regexSelect.selectedIndex = 0;
                if (!selectedOption.getAttribute('value'))
                    return;
                selectedOption.style.display = 'none';
                var template = "<div class=\"regex-item\">\n                    <label regex=\"" + val + "\" class=\"cell-label\">" + selectedOption.innerText + "</label>\n                    <button type=\"button\" class=\"cell-btn\" uk-icon=\"times\"></button>\n                </div>";
                var doc = new DOMParser().parseFromString(template, 'text/html');
                var reg = new tool_1.Regex();
                reg.element = doc.querySelector('.regex-item');
                reg.label = doc.querySelector('[regex]');
                reg.button = doc.querySelector('button');
                reg.element.appendChild(reg.label);
                reg.element.append(reg.button);
                reg.button.addEventListener('click', function () {
                    toolModel.regexs.splice(toolModel.regexs.indexOf(reg), 1);
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
                if (!val)
                    return;
                var template = "<div class=\"select-item\">\n                    <label select-item=\"" + val + "\" class=\"cell-label\">" + val + "</label>\n                    <button btnSelect class=\"cell-btn cell-border-right\" uk-icon=\"times\"></button>\n                </div>";
                var doc = new DOMParser().parseFromString(template, 'text/html');
                var sel = new tool_1.Select();
                sel.value = val;
                sel.element = doc.querySelector('.select-item');
                sel.label = doc.querySelector('[select-item]');
                sel.button = doc.querySelector('[btnSelect]');
                sel.button.addEventListener('click', function () {
                    toolModel.selects.splice(toolModel.selects.indexOf(sel, 1));
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