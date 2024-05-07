import { Component, Injector, OnInit } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import { PackageDto, PackageServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'ngx-dialog-manage-package',
    templateUrl: 'dialog-manage-package.component.html',
    styleUrls: ['packages.component.scss'],
})
export class DialogManagePackageComponent extends AppComponentBase implements OnInit {

    packageId: number; 
    packageNameAr: string;
    packageNameEn: string;
    packagePrice: number; 
    packagePoints: number;
    public packageForm: FormGroup;

    constructor(injector: Injector, private fb: FormBuilder, private _packageService: PackageServiceProxy, protected ref: NbDialogRef<DialogManagePackageComponent>) {
        super(injector);
    }

    ngOnInit() {
      //  let regex = new RegExp('(^[~`!@#$%^&*()_+=[\]\\{}|;\':",.\/<>?a-zA-Z0-9-]+$)');
      //[Validators.pattern('(^[~`!@#$%^&*()_+=[\]\\{}|;\':",.\/<>?a-zA-Z0-9-]+$)')]
        this.packageForm = this.fb.group({
            nameAr: [this.packageNameAr, [Validators.required]],
            nameEn: [this.packageNameEn, [Validators.required]],
            price: [this.packagePrice, [Validators.required]],
            points: [this.packagePoints,[Validators.required]],
           //nameEn: [this.countryNameEn, [Validators.required]]
        })
    }
// func(){
//     document.getElementById("user").onkeypress.(function(event){
//     var ew =Number(event.which);
//    //alert(ew);
//     if(1568 <= ew && ew <= 1610)//All Arabic char
//         return true;
//    if(32 <= ew && ew <= 64)
//         return true;
//    if(91 <= ew && ew <= 96)
//         return true;
//    if(123 <= ew && ew <= 127)
//         return true;

//     return false;
// });  //arabic
// }
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

    submit() {
        var input = new PackageDto();
        input.id = this.packageId;
        input.nameAr = this.packageNameAr;
        input.nameEn = this.packageNameEn;
        input.price = this.packagePrice;
        input.points = this.packagePoints
        this.manage(input);
    }

    manage(input) {
        this._packageService.manage(input)
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
