import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import { DiscountCodeDto, DiscountCodeServiceProxy } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';

@Component({
    selector: 'ngx-dialog-manage-discountCode',
    templateUrl: 'dialog-manage-discountCode.component.html',
    styleUrls: ['discountCodes.component.scss'],
})
export class DialogManageDiscountCodeComponent extends AppComponentBase implements OnInit {
    discountCodeId: number;
    discountCodeCode: string;
    discountCodePercentage: number;
    discountCodeFixedAmount: number;
    discountCodeIsPublish: boolean = false;
    from:any;
    to:any;
    selectedObj: DiscountCodeDto = new DiscountCodeDto();
    public discountCodeForm: FormGroup;

    constructor(injector: Injector, private fb: FormBuilder, private _discountCodeService: DiscountCodeServiceProxy, protected ref: NbDialogRef<DialogManageDiscountCodeComponent>) {
        super(injector);
    }

    ngOnInit() {
       
        this.discountCodeForm = this.fb.group({
            code: [this.discountCodeCode, [Validators.required]], 
            percentage: [this.discountCodePercentage],
            //from: [this.from, [Validators.required]],
            //to: [this.to, [Validators.required]],
            //fixedAmount: [this.discountCodeFixedAmount],
            isPublish: [this.discountCodeIsPublish],
           
        })
    }
    cancel() {
        this.ref.close();
    }
    checkArabicCharNumSympol(event){
                 var ew = Number(event.which);
                 if (1568 <= ew && ew <= 1610) //All Arabic char
                     return true;
                 if (32 <= ew && ew <= 64)
                     return true;
                 if (91 <= ew && ew <= 96)
                     return true;
                 if (123 <= ew && ew <= 127)
                     return true;
                 return false;
    } //arabic
    checkArabicChar(event) {
        var ew = Number(event.which);
        if (1568 <= ew && ew <= 1610) //All Arabic char
            return true;
        if (32 == ew ) //space
            return true;
        return false;
    } //arabic Char Only

    checkEnglishCharNumSympol(event){
        var ew = event.which;
        if(32 <= ew && ew <= 127)  //All English char and Symbols
            return true;
        return false;
    } //English
    checkEnglishChar(event){
        var ew = event.which;
        if (ew == 32)   //space
            return true;
        if (65 <= ew && ew <= 90) //Capital Char
            return true;
        if (97 <= ew && ew <= 122)//small char
            return true;

        return false;
    } //English Char Only

    checkNumber(event) {
        var ew = event.which;
        if (48 <= ew && ew <= 57)// 0 - 9
            return true;
        return false;
    }//numbers only

    anyWholeNumber(event): boolean {
        const charCode = (event.which) ? event.which : event.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;

    }

    anyDecimalNumber(event): boolean {
        const charCode = (event.which) ? event.which : event.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46) {
            return false;
        }
        return true;

    }

    submit() {
        debugger;
        var input = new DiscountCodeDto();
        input.id = this.discountCodeId;
        input.code = this.discountCodeCode;
        //input.fixedAmount = this.discountCodeFixedAmount;
        input.percentage = this.discountCodePercentage;
        input.isPublish = this.discountCodeIsPublish;
        input.from=this.from;
        input.to = this.to;
        if (input.from != null) {
            var datestring = this.convertDateFromHijri(input.from, "YYYY-MM-DD", "en"); // 
            input.from = moment(datestring, "YYYY-MM-DD").add(6, 'hours'); // 
        }
        if (input.to != null) {
            var datestring = this.convertDateFromHijri(input.to, "YYYY-MM-DD", "en"); // 
            input.to = moment(datestring, "YYYY-MM-DD").add(6, 'hours'); // 
        }

       
        this.manage(input);
    }

    manage(input) {
       
        this._discountCodeService.manage(input)
            .pipe(
                finalize(() => {
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('Common.Message.ActionSuccess'));
                setTimeout(() => {
                    this.ref.close(true);
                }, 1000);
            });
    }
    getfromDate(date): string {
        var fromdateString = this.convertDateFromHijri(date, 'YYYY-MM-DD', "en");
        if (date)
            this.from = moment(fromdateString, "YYYY-MM-DD")
        else
            this.from = this.selectedObj.from;
        return fromdateString;
    }
    gettoDate(date): string {
        var todateString = this.convertDateFromHijri(date, 'YYYY-MM-DD', "en");
        if (date)
            this.to = moment(todateString, "YYYY-MM-DD")
        else
            this.to = this.selectedObj.to;
        return todateString;
    }
}
