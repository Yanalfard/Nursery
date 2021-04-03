define(["require", "exports", "./model/inputType", "./model/tool"], function (require, exports, inputType_1, tool_1) {
    "use strict";
    exports.__esModule = true;
    // Toolbox Element Recreation implementation
    var toolbox = document.getElementById('toolbox');
    var toolboxHTML = toolbox.innerHTML;
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
    prototype.addEventListener('moved', drag);
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
        toolModel.optionsList = toolModel.element.querySelector('[option-list]');
        // Option
        toolModel.element.querySelectorAll('[option]').forEach(function (div) {
            var optionModel = new tool_1.Option();
            optionModel.element = div;
            optionModel.name = div.querySelector('[option-name]').innerText;
            toolModel.options.push(optionModel);
        });
        // Regex
        var regexList = toolModel.element.querySelector('[regex-list]');
        if (regexList) {
            toolModel.regexList = regexList;
            toolModel.regexSelect = regexList.querySelector('[regex-select]');
            toolModel.regexSelect.addEventListener('change', function () {
                var val = toolModel.regexSelect.value;
                var template = "<div class=\"regex-item\">\n                    <label regex class=\"cell-label\">" + val + "</label>\n                    <button class=\"cell-btn\" uk-icon=\"times\"></button>\n                </div>";
                var list = new DOMParser().parseFromString(template, 'text/html');
                var label = list.querySelector('[regex]');
                var btn = list;
                toolModel.regexList.innerHTML += template;
            });
        }
        // Select
        var selectList = toolModel.element.querySelector('[select-list]');
        if (selectList) {
            toolModel.selectList = selectList;
            toolModel.selectBtn = selectList.querySelector('[select-btn]');
            toolModel.selectInput = selectList.querySelector('[select-input]');
        }
        componentList.push(toolModel);
        console.log(componentList);
    });
    prototype.addEventListener('removed', function (event) {
        var element = event.detail[1];
        var tool = componentList.filter(function (i) { return i.element == element; })[0];
        componentList.splice(componentList.indexOf(tool), 1);
        element.parentElement.removeChild(element);
    });
    function drag() {
    }
});
//# sourceMappingURL=builder.js.map