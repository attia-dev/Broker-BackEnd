import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
//import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
//import { ChangeEvent, CKEditorComponent } from '@ckeditor/ckeditor5-angular';
import { CountryDto, CountryServiceProxy, DefinitionServiceProxy, DefinitionDto } from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';
import { HttpClient, HttpEventType } from '@angular/common/http';

@Component({
    selector: 'ngx-dialog-manage-property',
    templateUrl: 'dialog-manage-property.component.html',
    styleUrls: ['properties.component.scss'],
})
export class DialogManagePropertyComponent extends AppComponentBase implements OnInit {
    //public Editor = ClassicEditor;
    //@ViewChild('editorAr') editorAr: CKEditorComponent;
    //@ViewChild('editorEn') editorEn: CKEditorComponent;
    definitionId: number; 
    definitionNameAr: string;
    definitionNameEn: string;
    baseUrl: string;
    public progress: number;
    public message1: string;
    selectedObj: DefinitionDto = new DefinitionDto();
    public definitionForm: FormGroup;

    constructor(injector: Injector,private http: HttpClient, private fb: FormBuilder, private _definitionService: DefinitionServiceProxy, protected ref: NbDialogRef<DialogManagePropertyComponent>) {
        super(injector);
        this.baseUrl = AppConsts.remoteServiceBaseUrl + '/Resources/UploadFiles/';
    }

    ngOnInit() {
        /*this.getDocument();*/
      //  let regex = new RegExp('(^[~`!@#$%^&*()_+=[\]\\{}|;\':",.\/<>?a-zA-Z0-9-]+$)');
      //[Validators.pattern('(^[~`!@#$%^&*()_+=[\]\\{}|;\':",.\/<>?a-zA-Z0-9-]+$)')]
        this.definitionForm = this.fb.group({
            nameAr: [this.selectedObj.nameAr, [Validators.required]],
            nameEn: [this.selectedObj.nameEn, [Validators.required]],
            //descriptionAr: [this.selectedObj.descriptionAr,],
            //descriptionEn: [this.selectedObj.descriptionEn,],
            
           //nameEn: [this.countryNameEn, [Validators.required]]
        })

        console.log('from manage',this.selectedObj);
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

                this.selectedObj.avatar = fileToUpload.name;
               
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
       /* var input = new DefinitionDto();
        input.type = 10;
        input.id = this.definitionId;
        input.nameAr = this.definitionNameAr;
        input.nameEn = this.definitionNameEn;*/
        
        //input.descriptionAr = this.definitionDescriptionAr,
        //input.descriptionEn = this.definitionDescriptionEn,
        this.selectedObj.type=10;
        this.manage();
    }
    

    manage() {
        this._definitionService.manage(this.selectedObj)
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
