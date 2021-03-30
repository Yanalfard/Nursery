import { TblForm } from "./model/db/tblForm";
import { Form } from "./model/form";

const forms: any = document.querySelectorAll('dform');

for (var item of forms as HTMLFormElement[]) {

    const stringModel: string = item.getAttribute('model');
    const model = JSON.parse(stringModel) as TblForm;

    let form = new Form(model);

    for (var field of model.Fields) {
        form.addField(field);
    }

    form.attachForm(item);

    console.log(form);

}

