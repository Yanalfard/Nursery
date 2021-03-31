import { TblField } from "./db/tblField";

export class Tool {
    element: HTMLElement;
    btnSettings: HTMLButtonElement;
    btnDelete: HTMLButtonElement;
    optionsList: HTMLFormElement;
    regexList: HTMLElement;

    options: Option[] = [];
    regexs: Regex[] = [];



    constructor() { }

}

export class Option {
    element: HTMLElement;
    name: string;
    value: string;
    constructor() {
        
    }
}

export class Regex {
    element: HTMLElement;
    name: string;
    value: string;
    constructor() { }
}