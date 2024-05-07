import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
//import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
//import { ChangeEvent, CKEditorComponent } from '@ckeditor/ckeditor5-angular';
import { CountryDto, CountryServiceProxy, DefinitionServiceProxy, DefinitionDto } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'ngx-dialog-manage-decoration',
    templateUrl: 'dialog-manage-decoration.component.html',
    styleUrls: ['decorations.component.scss'],
})
export class DialogManageDecorationComponent extends AppComponentBase implements OnInit {
    //public Editor = ClassicEditor;
    //@ViewChild('editorAr') editorAr: CKEditorComponent;
    //@ViewChild('editorEn') editorEn: CKEditorComponent;
    definitionId: number; 
    definitionNameAr: string;
    definitionNameEn: string;
    
    selectedObj: DefinitionDto = new DefinitionDto();
    public definitionForm: FormGroup;

    constructor(injector: Injector, private fb: FormBuilder, private _definitionService: DefinitionServiceProxy, protected ref: NbDialogRef<DialogManageDecorationComponent>) {
        super(injector);
    }

    ngOnInit() {
        /*this.getDocument();*/
      //  let regex = new RegExp('(^[~`!@#$%^&*()_+=[\]\\{}|;\':",.\/<>?a-zA-Z0-9-]+$)');
      //[Validators.pattern('(^[~`!@#$%^&*()_+=[\]\\{}|;\':",.\/<>?a-zA-Z0-9-]+$)')]
        this.definitionForm = this.fb.group({
            nameAr: [this.definitionNameAr, [Validators.required]],
            nameEn: [this.definitionNameEn, [Validators.required]],
            
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
        var input = new DefinitionDto();
        input.type = 3;
        input.id = this.definitionId;
        input.nameAr = this.definitionNameAr;
        input.nameEn = this.definitionNameEn;
        //input.descriptionAr = this.definitionDescriptionAr,
        //input.descriptionEn = this.definitionDescriptionEn,
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
