import { TblField } from "./model/db/tblField";
import { TblForm } from "./model/db/tblForm";
import { TblRegex } from "./model/db/tblRegex";
import { TblValue } from "./model/db/tblValue";
import { Tool, Option, Regex } from "./model/tool";

// Toolbox Element Recreation implementation
const toolbox = document.getElementById('toolbox');
const toolboxHTML = toolbox.innerHTML;
toolbox.addEventListener('removed', () => {
    toolbox.innerHTML = toolboxHTML;
});
//toolbox.addEventListener('added', () => {
//    toolbox.innerHTML = toolboxHTML;
//});

let componentList: Tool[] = [];

// Prototype Data Construction
const prototype = document.getElementById('prototype');
prototype.addEventListener('start', drag);
prototype.addEventListener('stop', drag);
prototype.addEventListener('moved', drag);
prototype.addEventListener('added', (event: any) => {

    const el: HTMLElement = event.detail[1];

    let toolModel = new Tool();
    toolModel.element = el;
    toolModel.btnDelete = toolModel.element.querySelector('[btnDelete]') as HTMLButtonElement;
    toolModel.btnSettings = toolModel.element.querySelector('[btnSettings]') as HTMLButtonElement;

    // btnSettings_Click
    toolModel.btnSettings.addEventListener('click', () => {
        toolModel.optionsList.style.display = toolModel.optionsList.style.display === 'none' ? 'block' : 'none';
    });

    // btnDelete_Click
    toolModel.btnDelete.addEventListener('click', () => {
        toolModel.element.style.transition = 'all ease 0.3s';
        toolModel.element.style.opacity = '0';

        setTimeout(() => {
            toolModel.element.parentElement.removeChild(toolModel.element);
            componentList = componentList.splice(componentList.indexOf(toolModel, 1));
            delete toolModel.element;
        }, 300);
    });

    toolModel.regexList = toolModel.element.querySelector('[regex-list]');
    toolModel.optionsList = toolModel.element.querySelector('[option-list]');

    toolModel.element.querySelectorAll('[option]').forEach(div => {
        let optionModel = new Option();
        optionModel.element = div as HTMLElement;
        optionModel.name = (div.querySelector('[option-name]') as HTMLLabelElement).innerText;

        toolModel.options.push(optionModel);
    });

    componentList.push(toolModel);

    console.log(componentList);

});

prototype.addEventListener('removed', (event: any): void => {

    const element = (event.detail[1] as HTMLElement);
    const tool = componentList.filter(i => i.element == element)[0];
    componentList.splice(componentList.indexOf(tool), 1);
    element.parentElement.removeChild(element);

});

function drag() {

}