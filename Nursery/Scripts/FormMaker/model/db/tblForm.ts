import { TblField } from "./tblField";

export class TblForm {
    FormId: number = -1;
    Name: string = '';
    /** A text to put on forms subtitle */
    Body: string = '';
    Priority: number = 0;
    DateCreated: Date;
    IsDeleted: boolean = false;
    //-
    Fields: TblField[] = [];

    constructor() { }
}
