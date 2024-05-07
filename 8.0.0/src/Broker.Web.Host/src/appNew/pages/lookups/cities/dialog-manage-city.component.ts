import { Component, Injector, OnInit } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import { CityDto, CityServiceProxy, CountryDto, CountryServiceProxy, GovernorateDto, GovernorateServiceProxy} from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'ngx-dialog-manage-city',
    templateUrl: 'dialog-manage-city.component.html',
    styleUrls: ['cities.component.scss'],
})
export class DialogManageCityComponent extends AppComponentBase implements OnInit {

    cityId: number;
    cityNameAr: string;
    cityNameEn: string;

    

    countries: CountryDto[] = [];
    selectCountry: CountryDto[] = [];
    countrydto: CountryDto;
    public cityForm: FormGroup;
    governorates: GovernorateDto[] = [];
    selectGovernorate: GovernorateDto[] = [];
    governoratedto: GovernorateDto;
    dropdownSettings: any = {};
    
    dropdownSettingsCountry: any = {};
    ShowFilter = true;
    public submitflag: boolean = true;  
    constructor(injector: Injector, private fb: FormBuilder, private _cityService: CityServiceProxy, private _countryService: CountryServiceProxy, private _governorateService: GovernorateServiceProxy, protected ref: NbDialogRef<DialogManageCityComponent>) {
        super(injector);
        
        this.countrydto = new CountryDto();
        this.governoratedto = new GovernorateDto();
    }

    ngOnInit() {
        this.getCountries();
        if (this.cityId > 0) {
            this.submitflag == true;
            this.getGovernorates(this.selectCountry[0].id);
        }
        else {
            this.submitflag == false;
            this.governorates = [];
           //this.getGovernorates(0);
        }
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
        this.dropdownSettingsCountry = {
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

        this.cityForm = this.fb.group({
            nameAr: [this.cityNameAr, [Validators.required]],
            nameEn: [this.cityNameEn, [Validators.required]],
            governorateId: [this.selectGovernorate, [Validators.required]],
            countryId: [this.selectCountry, [Validators.required]]
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
    
    getGovernorates(countryId) {
        this.governorates = [];
        if (this.submitflag == false) {
            this.selectGovernorate = [];
        }
        else
            this.submitflag = false;

        this._governorateService.getAll(undefined, countryId, undefined, undefined).pipe(
            finalize(() => {

            })
        ).subscribe((result) => {
            this.governorates = this.governorates.concat(result.governorates);

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
    submit(nameAr, nameEn, governorateId) {
        debugger;
        var input = new CityDto();
        input.id = this.cityId;
        input.nameAr = nameAr;
        input.nameEn = nameEn;
        input.governorateId = governorateId;
        this.manage(input);
        
    }

    manage(input) {
        this._cityService.manage(input)
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
