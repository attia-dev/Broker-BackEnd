import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
//import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
//import { ChangeEvent, CKEditorComponent } from '@ckeditor/ckeditor5-angular';
import { CountryDto, CountryServiceProxy, DefinitionServiceProxy, DefinitionDto, ContactUsDto, ContactUsServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';
import { HttpClient, HttpEventType } from '@angular/common/http';

@Component({
    selector: 'ngx-dialog-manage-contactUs',
    templateUrl: 'dialog-manage-contactUs.component.html',
    styleUrls: ['contactUses.component.scss'],
})
export class DialogManageContactUsComponent extends AppComponentBase implements OnInit {
 
    baseUrl: string;
    public progress: number;
    public message1: string;
    selectedObj: ContactUsDto = new ContactUsDto();
    public contactUsForm: FormGroup;
    emailPattern= "^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$";
    constructor(injector: Injector,private http: HttpClient, private fb: FormBuilder,
         private _contactUsService: ContactUsServiceProxy,
          protected ref: NbDialogRef<DialogManageContactUsComponent>) {
        super(injector);
        this.baseUrl = AppConsts.remoteServiceBaseUrl + '/Resources/UploadFiles/';
    }

    ngOnInit() {
        this.initForm();
    }

    initForm()
    {
        this.contactUsForm = this.fb.group({
            emailAddress: [this.selectedObj.emailAddress, [Validators.required]],
            emailSubject: [this.selectedObj.emailSubject, [Validators.required]],
            attachmentPath: [this.selectedObj.attachmentPath],
            userId: [this.selectedObj.userId],
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
                this.selectedObj.attachmentPath = fileToUpload.name;

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
        this.selectedObj.userId=this.appSession.userId;
        this.manage();
    }
    

    manage() {
        this._contactUsService.manage(this.selectedObj)
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
