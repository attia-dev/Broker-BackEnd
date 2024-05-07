import { Component, Injector, OnInit } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import { CountryDto, CountryServiceProxy,DefinitionServiceProxy, DefinitionDto } from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';
import { HttpClient, HttpEventType } from '@angular/common/http';

@Component({
    selector: 'ngx-dialog-manage-facility',
    templateUrl: 'dialog-manage-facility.component.html',
    styleUrls: ['facilities.component.scss'],
})
export class DialogManageFacilityComponent extends AppComponentBase implements OnInit {

    definitionId: number; 
    definitionNameAr: string;
    definitionNameEn: string;
    avatar:string;
    selectedObj: DefinitionDto = new DefinitionDto();
    public definitionForm: FormGroup;
    baseUrl: string;
    public progress: number;
    public message1: string;

    constructor(injector: Injector,private http: HttpClient, private fb: FormBuilder,  private _definitionService: DefinitionServiceProxy, protected ref: NbDialogRef<DialogManageFacilityComponent>) {
        super(injector);
        this.baseUrl = AppConsts.remoteServiceBaseUrl + '/Resources/UploadFiles/';
    }

    ngOnInit() {
        
      //  let regex = new RegExp('(^[~`!@#$%^&*()_+=[\]\\{}|;\':",.\/<>?a-zA-Z0-9-]+$)');
      //[Validators.pattern('(^[~`!@#$%^&*()_+=[\]\\{}|;\':",.\/<>?a-zA-Z0-9-]+$)')]
        this.definitionForm = this.fb.group({
            nameAr: [this.definitionNameAr, [Validators.required]],
            nameEn: [this.definitionNameEn, [Validators.required]],
            
           //nameEn: [this.countryNameEn, [Validators.required]]
        })
    }

    public uploadFile = (files) => {
        debugger;
        if (files.length === 0) {
            return;
        }
        let fileToUpload = <File>files[0];
        const formData = new FormData();
        formData.append('file', fileToUpload, fileToUpload.name);
        this.http.post(AppConsts.remoteServiceBaseUrl + '/api/upload/upload', formData, { reportProgress: true, observe: 'events' })
            .subscribe(event => {
                if (event.type === HttpEventType.UploadProgress)
                    this.progress = Math.round(100 * event.loaded / event.total);
                else if (event.type === HttpEventType.Response) {
                    this.message1 = 'Upload success.';
                    debugger;
    
                    this.avatar = fileToUpload.name;
                   
                    /*this.doctorPictureForm = this.fb.group({
                        avatar: [this.selectedObj.avatar, [Validators.required]],
    
                    })*/
    
                }
            });
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

    submit() {
        var input = new DefinitionDto();
        input.type = 1;
        input.id = this.definitionId;
        input.nameAr = this.definitionNameAr;
        input.nameEn = this.definitionNameEn;
        input.avatar=this.avatar;
        this.manage(input);
    }
    

    manage(input) {
        this._definitionService.manage(input)
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
