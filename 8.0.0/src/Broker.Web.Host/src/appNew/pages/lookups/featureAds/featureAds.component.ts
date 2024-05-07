import { Component, OnInit, ChangeDetectionStrategy, Injector } from '@angular/core';
import { DefinitionServiceProxy, DefinitionDto} from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import { BehaviorSubject, Observable } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'app-manage-featureAds',
    templateUrl: './featureAds.component.html',
changeDetection: ChangeDetectionStrategy.Default,
//styleUrls: ['../lookups.component.scss']
})
export class ManageFeatureAdsComponent extends AppComponentBase implements OnInit {

    //address: any = {};
    percentage: any = {};
    //email: any = {};
    featureAdsList: DefinitionDto[] = [];
    featureAdsForm: FormGroup;
    hasError: boolean;
    isLoading$: Observable<boolean>;
    isLoadingSubject: BehaviorSubject<boolean>;
    formData = new FormData();
    latitude: number;
    longitude: number;
  
    constructor(injector: Injector, private fb: FormBuilder, private _featureAdsService: DefinitionServiceProxy) {
        super(injector);
        this.isLoadingSubject = new BehaviorSubject<boolean>(false);
        this.isLoading$ = this.isLoadingSubject.asObservable();
    }

    ngOnInit(): void {
        //this.setCurrentPosition();
        this.getFeatureAds();
        this.initForm();
    }

    getFeatureAds() {
        debugger;
        this._featureAdsService.getAll("", 9, undefined, undefined, )
        .pipe(finalize(() => { }))
        .subscribe((result: any) => {
            debugger;
            console.log(result);
            if (result.definitions.length > 0) {
                this.featureAdsList = [];
                this.featureAdsList.push(result.definitions);
                
                
                this.percentage = result.definitions[0];
            }
            
            //this.contactsList.forEach((x)=>{ 
            //    if(x.key == "Address"){
            //        this.address = x;
            //        if(x.descriptionAr)
            //            this.latitude = +x.descriptionAr; 
            //        if(x.descriptionEn)
            //            this.longitude = +x.descriptionEn;
            //    }
            //    else if(x.key == "Phone"){
            //        this.phone = x;
            //    }
            //    else if(x.key == "Email"){
            //        this.email = x;    
            //    }
            //});
        });
    }

    //private setCurrentPosition() {
    //    if ('geolocation' in navigator) {
    //      navigator.geolocation.getCurrentPosition((position) => {
    //        this.latitude = position.coords.latitude;
    //        this.longitude = position.coords.longitude;
    //      });
    //    }
    //    else {
    //        this.latitude = 33.312805;
    //        this.longitude = 44.361488;
    //    }
    //}
    
    //markerDragEnd($event: google.maps.MapMouseEvent) {
    //    this.latitude = $event.latLng.lat();
    //    this.longitude = $event.latLng.lng();
    //}
  
    get f() {
        return this.featureAdsForm.controls;
    }

    initForm() {
        //userEmailAddress: [this.doctor.userEmailAddress, Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$")],
        //userPhoneNumber: [this.doctor.userPhoneNumber, Validators.pattern('[- +()0-9]+')],
        this.featureAdsForm = this.fb.group({
            //addressEn: [
            //    this.address.nameEn,
            //    Validators.compose([
            //        Validators.required,
            //        Validators.minLength(3),
            //        Validators.maxLength(320)
            //    ]),
            //],
            //addressAr: [
            //    this.address.nameEn,
            //    Validators.compose([
            //        Validators.required,
            //        Validators.minLength(3),
            //        Validators.maxLength(320)
            //    ]),
            //],
            percentage: [
                this.percentage.nameEn,
                Validators.compose([
                    Validators.required,
                    Validators.minLength(1),
                    Validators.maxLength(2),
                    /*Validators.pattern('[- +()0-9]+')*/
                ]),
            ],
            //email: [
            //    this.email.nameEn,
            //    Validators.compose([
            //        Validators.required,
            //        Validators.minLength(3),
            //        Validators.maxLength(320),
            //        Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$")
                    
            //    ]),
            //]
        });
    }
    


    checkEnglishCharNumSympol(event) {
        var ew = event.which;
        if (32 <= ew && ew <= 127)  //All English char and Symbols
            return true;
        return false;
    } //English
    checkNumber(event) {
        var ew = event.which;
        if (48 <= ew && ew <= 57)// 0 - 9
            return true;
        return false;
    }//numbers only
    checkArabicCharNumSympol(event) {
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
        debugger;
        //if(this.address && this.address.nameEn){
            //this.address.type = 7;
            //this.address.key = "Address";
            //if(this.latitude)
            //    this.address.descriptionAr = this.latitude;
            //if(this.longitude)
            //    this.address.descriptionEn = this.longitude;
            //this.isLoadingSubject.next(true);
            //if (this.address.id)
            //    this.update(this.address);
            //else 
            //    this.create(this.address);
           // this.manage(this.address);
        //}
        if (this.percentage && this.percentage.nameEn){
            this.percentage.type = 9;
           // this.phone.key = "Phone";
            this.percentage.nameAr = this.percentage.nameEn;
            this.isLoadingSubject.next(true);
            //if (this.phone.id)
            //    this.update(this.phone);
            //else 
            //    this.create(this.phone);
            this.manage(this.percentage);
        }
        //if(this.email && this.email.nameEn){
           // this.email.type = 7;
            //this.email.key = "Email";
            //this.email.nameAr = this.email.nameEn;
            //this.isLoadingSubject.next(true);
            //if (this.email.id)
            //    this.update(this.email);
            //else 
            //    this.create(this.email);
            //this.manage(this.email);
        //}
    }
    
    manage(obj) {
        this._featureAdsService.manage(obj)
        .pipe(
            finalize(() => {
                this.isLoadingSubject.next(false);
            })
        )
        .subscribe(() => {
            this.notify.info(this.l('SavedSuccessfully'));
        });

    }
    
    //update(obj) {
    //    this._contactService.update(obj)
    //    .pipe(
    //        finalize(() => {
    //            this.isLoadingSubject.next(false);
    //        })
    //    )
    //    .subscribe(() => {
    //        this.notify.info(this.l('SavedSuccessfully'));
    //    });

    //}
    
}  