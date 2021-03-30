export class TblRegex {
    RegexId: number = -1;
    Name: string = '';
    Regex: RegExp = new RegExp('');
    ValidationMessage: string = '';
    IsDeleted: boolean = false;

    constructor(regex: RegExp, validationMessage?: string, name?: string) {
        this.Regex = regex;
        this.ValidationMessage = validationMessage ?? '';
        this.Name = name;
    }
}