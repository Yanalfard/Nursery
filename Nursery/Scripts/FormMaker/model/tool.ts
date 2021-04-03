import { TblField } from "./db/tblField";
import { InputType } from "./inputType";

export class Tool {
    element: HTMLElement;
    type: InputType;

    btnSettings: HTMLButtonElement;
    btnDelete: HTMLButtonElement;

    optionsList: HTMLFormElement;
    regexList: HTMLElement;
    regexSelect: HTMLSelectElement;

    selectList: HTMLElement;
    selectAdder: HTMLElement;
    selectInput: HTMLInputElement;
    selectBtn: HTMLButtonElement;

    options: Option[] = [];
    regexs: Regex[] = [];
    selects: Select[] = [];

    constructor() { }

}

export class Option {
    element: HTMLElement;
    name: string;
    value: string;
    constructor() { }
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
    value: string;
    constructor() { }
}