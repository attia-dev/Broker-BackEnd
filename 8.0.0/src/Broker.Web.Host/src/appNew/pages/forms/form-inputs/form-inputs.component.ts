import { Component } from '@angular/core';

@Component({
    selector: 'ngx-form-inputs',
    styleUrls: ['./form-inputs.component.scss'],
    templateUrl: './form-inputs.component.html',
})
export class FormInputsComponent {
    selected = 4;
    starRate = 2;
    heartRate = 4;
    radioGroupValue = 'This is value 2';
    radioGroupValue2 = 'This is value 2';
}
