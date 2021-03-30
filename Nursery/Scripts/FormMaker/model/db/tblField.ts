import { InputType } from "../inputType";
import { TblRegex } from "./tblRegex";

export class TblField {
    FieldId: number = -1;
    FormId: number = -1;
    Label: string = '';
    Type: InputType = InputType.text;
    IsRequired: boolean = false;
    /** 
     *  
     *  */
    Options: string | string[] = '';
    Placeholder: string = '';
    Tooltip: string = '';
    IsDeleted: boolean = false;
    //-
    Validations: TblRegex[] = [];

    constructor() { }
}