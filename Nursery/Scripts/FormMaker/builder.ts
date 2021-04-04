import { TblField } from "./model/db/tblField";
import { TblForm } from "./model/db/tblForm";
import { TblRegex } from "./model/db/tblRegex";
import { TblValue } from "./model/db/tblValue";
import { Form } from "./model/form";
import { InputType } from "./model/inputType";
import { Tool, Option, Regex, Select, OptionType } from "./model/tool";

// Toolbox Element Recreation implementation
const toolbox = document.getElementById('toolbox');
const toolboxHTML = toolbox.innerHTML;
const btnFinish = document.getElementById('btn-finish');

btnFinish.addEventListener('click', () => {
    let temp: Tool[];
    componentList.forEach((tool) => {
        let index = parseInt(tool.element.getAttribute('order'));
        temp[index] = tool;
    });
    componentList = temp;

});

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
prototype.addEventListener('moved', enumerateTools);

function enumerateTools() {
    let counter = 0;
    prototype.childNodes.forEach(formElement => {
        if (formElement.nodeType === 1) {
            const i = formElement as HTMLElement;
            i.setAttribute('order', counter.toString());
            counter++;
        }
    })
}

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

    // option
    toolModel.optionsList = toolModel.element.querySelector('[option-list]');

    // Option
    toolModel.element.querySelectorAll('[option]').forEach(div => {
        const element = div as HTMLDivElement;
        const mainElement = (div.querySelector('[option-name]') as HTMLLabelElement);
        const type = element.getAttribute("option");
        const name = mainElement.getAttribute("option-name");
        let optionModel = new Option(element, OptionType[type], name);

        toolModel.options.push(optionModel);
    });

    // Regex
    let regexList = toolModel.element.querySelector('[regex-list]');
    if (regexList) {
        toolModel.regexList = regexList as HTMLElement;
        toolModel.regexSelect = regexList.querySelector('[regex-select]');
        toolModel.regexAdder = regexList.querySelector('.regex-list');

        toolModel.regexSelect.addEventListener('change', () => {

            const val = toolModel.regexSelect.value;
            const selectedOption = toolModel.regexSelect.selectedOptions[0];

            toolModel.regexSelect.selectedIndex = 0;
            if (!selectedOption.getAttribute('value')) return;

            selectedOption.style.display = 'none';

            const template =
                `<div class="regex-item">
                    <label regex="${val}" class="cell-label">${selectedOption.innerText}</label>
                    <button type="button" class="cell-btn" uk-icon="times"></button>
                </div>`;

            const doc = new DOMParser().parseFromString(template, 'text/html');

            let reg = new Regex();
            reg.element = doc.querySelector('.regex-item');
            reg.label = doc.querySelector('[regex]');
            reg.button = doc.querySelector('button');

            reg.element.appendChild(reg.label);
            reg.element.append(reg.button);


            reg.button.addEventListener('click', () => {
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
    let selectElement = toolModel.element.querySelector('[select]');
    if (selectElement) {
        toolModel.selectElement = selectElement as HTMLElement;
        toolModel.selectAdder = selectElement.querySelector('[select-adder]');
        toolModel.selectInput = toolModel.selectAdder.querySelector('[select-input]');
        toolModel.selectBtn = toolModel.selectAdder.querySelector('[select-btn]');
        toolModel.selectList = selectElement.querySelector('[select-list]');

        // add
        toolModel.selectBtn.addEventListener('click', () => {
            let val = toolModel.selectInput.value;
            if (!val) return;

            const template =
                `<div class="select-item">
                    <label select-item="${val}" class="cell-label">${val}</label>
                    <button btnSelect class="cell-btn cell-border-right" uk-icon="times"></button>
                </div>`;

            const doc = new DOMParser().parseFromString(template, 'text/html');

            let sel = new Select();
            sel.value = val;
            sel.element = doc.querySelector('.select-item');
            sel.label = doc.querySelector('[select-item]');
            sel.button = doc.querySelector('[btnSelect]');

            sel.button.addEventListener('click', () => {
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

prototype.addEventListener('removed', (event: any): void => {

    const element = (event.detail[1] as HTMLElement);
    const tool = componentList.filter(i => i.element == element)[0];
    componentList.splice(componentList.indexOf(tool), 1);
    element.parentElement.removeChild(element);
    enumerateTools();

});

function drag() {

}