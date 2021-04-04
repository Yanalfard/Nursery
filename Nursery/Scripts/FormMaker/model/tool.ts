import { TblField } from "./db/tblField";
import { InputType } from "./inputType";

export class Tool {
    element: HTMLElement;
    type: InputType;

    btnSettings: HTMLButtonElement;
    btnDelete: HTMLButtonElement;
    //-
    optionsList: HTMLFormElement;
    //-
    regexElement: HTMLElement
    regexList: HTMLElement;
    regexAdder: HTMLElement;
    regexSelect: HTMLSelectElement;
    //-
    selectElement: HTMLElement;
    selectList: HTMLElement;
    selectAdder: HTMLElement;
    selectInput: HTMLInputElement;
    selectBtn: HTMLButtonElement;

    options: Option[] = [];
    regexs: Regex[] = [];
    selects: Select[] = [];

    constructor() { }

}

export enum OptionType {
    input,
    select,
    checkbox,
    doubleInput,
}

export class Option {
    element: HTMLElement;
    optionType: OptionType;
    name: string;
    value: string;

    constructor(element?: HTMLElement, optionType?: OptionType, name?: string, value?: string) {
        this.element = element;
        this.optionType = optionType;
        this.name = name;
        this.value = value;

        switch (+optionType) {

            case OptionType.input:
                const input = element.querySelector('input') as HTMLInputElement;
                input.addEventListener('change', () => {
                    value = input.value;
                });
                break;

            case OptionType.select:
                const select = element.querySelector('select') as HTMLSelectElement;
                select.addEventListener('change', () => {
                    value = select.value;
                });
                break

            case OptionType.checkbox:
                const checkbox = element.querySelector('input[type=checkbox]') as HTMLInputElement;
                checkbox.addEventListener('change', () => {
                    value = checkbox.value;
                });
                break;

            case OptionType.doubleInput:
                const from = element.querySelector('[input-from]') as HTMLInputElement;
                const to = element.querySelector('[input-to]') as HTMLInputElement;
                //-
                const change = () => {
                    value = from.value + ',' + to.value;
                }
                from.addEventListener('change', change);
                to.addEventListener('change', change);
                break;

            default:
                console.error('Unknown Option Type');
                break;
        }

    }

    private valueChanged() {

    }

}

export class Regex {
    element: HTMLElement;
    label: HTMLLabelElement;
    button: HTMLButtonElement;
    name: string;
    value: string;
    constructor() { }
}

export class Select {
    element: HTMLElement;
    label: HTMLLabelElement;
    button: HTMLButtonElement;

    value: string;
    constructor() { }
}