import { Component, Injector, OnInit } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import { CityDto, CityServiceProxy, CountryDto, GovernorateDto, CountryServiceProxy, GovernorateServiceProxy} from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'ngx-dialog-manage-governorate',
    templateUrl: 'dialog-manage-governorate.component.html',
    styleUrls: ['governorates.component.scss'],
})
export class DialogManageGovernorateComponent extends AppComponentBase implements OnInit {

    governorateId: number;
    governorateNameAr: string;
    governorateNameEn: string;
    public governorateForm: FormGroup;
    countries: CountryDto[] = [];
    selectCountry: CountryDto[] = [];
    countrydto: CountryDto;
    dropdownSettings: any = {};
    ShowFilter = true;
    public submitflag: boolean = false;
    constructor(injector: Injector, private fb: FormBuilder, private _governorateService: GovernorateServiceProxy, private _countryService: CountryServiceProxy, protected ref: NbDialogRef<DialogManageGovernorateComponent>) {
        super(injector);
        this.countrydto = new CountryDto();
    }

    ngOnInit() {
        this.getCountries();
        this.dropdownSettings = {
            singleSelection: true,
            idField: 'id',
            textField: this.localization.currentLanguage.name != 'en' ? 'nameAr' : 'nameEn',
            selectAllText: this.l('Common.SelectAll'),
            unSelectAllText: this.l('Common.UnSelectAll'),
            searchPlaceholderText: this.l('Common.Search'),
            noDataAvailablePlaceholderText: this.l('Common.NoDataAvailable'),
            itemsShowLimit: 3,
            closeDropDownOnSelection: true,
            allowSearchFilter: true
        };

        this.governorateForm = this.fb.group({
            nameAr: [this.governorateNameAr, [Validators.required]],
            nameEn: [this.governorateNameEn, [Validators.required]],
            countryId: [this.selectCountry, [Validators.required]],
        }) 
    }

    cancel() {
        this.ref.close();
    }
    getCountries() {
        this._countryService.getAll(undefined, undefined, undefined).pipe(
            finalize(() => {

            })
        ).subscribe((result) => {
            this.countries = this.countries.concat(result.countries);
        });
    }
    checkArabicChar(event) {
        var ew = Number(event.which);
        if (1568 <= ew && ew <= 1610) //All Arabic char
            return true;
        if (32 == ew) //space
            return true;
        return false;
    } //arabic Char Only

    checkEnglishChar(event) {
        var ew = event.which;
        if (ew == 32)   //space
            return true;
        if (65 <= ew && ew <= 90) //Capital Char
            return true;
        if (97 <= ew && ew <= 122)//small char
            return true;

        return false;
    } //English Char Only
    submit() {
        if (this.selectCountry[0].id == 0)
            this.submitflag = true;
        else {
            this.submitflag = false;
            var input = new GovernorateDto();
            input.id = this.governorateId;
            input.nameAr = this.governorateNameAr;
            input.nameEn = this.governorateNameEn;
            input.countryId = this.selectCountry[0].id;
            this.manage(input);  
        }
    }

    manage(input) {
        this._governorateService.manage(input)
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


}
