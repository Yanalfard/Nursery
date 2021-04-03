import { TblField } from "./model/db/tblField";
import { TblForm } from "./model/db/tblForm";
import { TblRegex } from "./model/db/tblRegex";
import { TblValue } from "./model/db/tblValue";
import { InputType } from "./model/inputType";
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
    toolModel.type = InputType[el.querySelector('[tool]').getAttribute('tool').toLowerCase()];

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

    toolModel.optionsList = toolModel.element.querySelector('[option-list]');

    // Option
    toolModel.element.querySelectorAll('[option]').forEach(div => {
        let optionModel = new Option();
        optionModel.element = div as HTMLElement;
        optionModel.name = (div.querySelector('[option-name]') as HTMLLabelElement).innerText;

        toolModel.options.push(optionModel);
    });

    // Regex
    let regexList = toolModel.element.querySelector('[regex-list]');
    if (regexList) {
        toolModel.regexList = regexList as HTMLElement;
        toolModel.regexSelect = regexList.querySelector('[regex-select]');



        toolModel.regexSelect.addEventListener('change', () => {
            const name = toolModel.regexSelect.value;
            c

            console.log(toolModel.regexSelect.selectedOptions);
            const template =
                `<div class="regex-item">
                    <label regex="${name}" class="cell-label">${name}</label>
                    <button class="cell-btn" uk-icon="times"></button>
                </div>`;


            const list = new DOMParser().parseFromString(template, 'text/html');
            const label = list.querySelector('[regex]');
            const btn = list.querySelector('button');


            toolModel.regexList.innerHTML += template;
        });

    }

    // Select
    let selectList = toolModel.element.querySelector('[select-list]');
    if (selectList) {
        toolModel.selectList = selectList as HTMLElement;
        toolModel.selectBtn = selectList.querySelector('[select-btn]');
        toolModel.selectInput = selectList.querySelector('[select-input]');
    }




    componentList.push(toolModel);

});

prototype.addEventListener('removed', (event: any): void => {

    const element = (event.detail[1] as HTMLElement);
    const tool = componentList.filter(i => i.element == element)[0];
    componentList.splice(componentList.indexOf(tool), 1);
    element.parentElement.removeChild(element);

});

function drag() {

}