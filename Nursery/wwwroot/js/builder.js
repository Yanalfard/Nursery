define(["require", "exports", "./model/tool"], function (require, exports, tool_1) {
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
        toolModel.regexList = toolModel.element.querySelector('[regex-list]');
        toolModel.optionsList = toolModel.element.querySelector('[option-list]');
        toolModel.element.querySelectorAll('[option]').forEach(function (div) {
            var optionModel = new tool_1.Option();
            optionModel.element = div;
            optionModel.name = div.querySelector('[option-name]').innerText;
            toolModel.options.push(optionModel);
        });
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