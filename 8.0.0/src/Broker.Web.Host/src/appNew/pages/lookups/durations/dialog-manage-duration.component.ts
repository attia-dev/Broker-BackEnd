import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import { DurationDto, DurationServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'ngx-dialog-manage-duration',
    templateUrl: 'dialog-manage-duration.component.html',
    styleUrls: ['durations.component.scss'], 
})
export class DialogManageDurationComponent extends AppComponentBase implements OnInit {
    durationId: number;
    durationPeriod: number;
    durationAmount: number;
    durationType: number[];
    durationIsPublish: boolean = false;
    
    selectedObj: DurationDto = new DurationDto();
    public durationForm: FormGroup;

    // INTEREST PAYMENT TYPES DROPDOWN
    buildingType: any[] = [
        { id: 1, name: this.l('Common.Apartment') },
        { id: 2, name: this.l('Common.Villa') },
        { id: 3, name: this.l('Common.ChaletForSummer') },
        { id: 4, name: this.l('Common.Building') },
        { id: 5, name: this.l('Common.AdminOffice') },
        { id: 6, name: this.l('Common.Shop') },
        { id: 7, name: this.l('Common.Land') },
    ];
    dropdownSettingsBuildingType: any;
    selectedBuildingType: any[] = [];

    constructor(injector: Injector, private fb: FormBuilder, private _durationService: DurationServiceProxy, protected ref: NbDialogRef<DialogManageDurationComponent>) {
        super(injector);

        this.dropdownSettingsBuildingType = {
            singleSelection: false,
            idField: 'id',
            textField: 'name',
            selectAllText: this.l('Common.SelectAll'),
            unSelectAllText: this.l('Common.UnSelectAll'),
            searchPlaceholderText: this.l('Common.Search'),
            noDataAvailablePlaceholderText: this.l('Common.NoDataAvailable'),
            closeDropDownOnSelection: true,
            itemsShowLimit: 3,
            allowSearchFilter: true
        };
    }

    ngOnInit() {
        this.durationForm = this.fb.group({
            period: [this.durationPeriod, [Validators.required]],
            amount: [this.durationAmount, [Validators.required]],
            type: [this.durationType],
            isPublish: [this.durationIsPublish],
        });
        this.durationType.forEach(x => {
            this.selectedBuildingType.push(this.buildingType[x - 1]
                                  )     }  );
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
        var input = new DurationDto();
        input.id = this.durationId;
        input.period = this.durationPeriod;
        input.amount = this.durationAmount;
        let types: number[] = [];
        this.selectedBuildingType.forEach(x => types.push(x.id));
        if (this.selectedBuildingType != undefined)
            input.buildingTypes = types /*this.selectedBuildingType*/;
        input.isPublish = this.durationIsPublish;
        this.manage(input);
    }

    manage(input) {
        this._durationService.manage(input)
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

    onBuildingTypeSelect(event) {
        this.durationType = event.id;
    }

    onBuildingTypeDeSelect() {
        this.durationType = null;
    }
}
