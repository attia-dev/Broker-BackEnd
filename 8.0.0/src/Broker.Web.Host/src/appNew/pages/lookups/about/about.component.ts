import { Component, OnInit, ChangeDetectionStrategy, Injector, ViewChild } from '@angular/core';
import { DefinitionServiceProxy, DefinitionDto/*, DefinitionDtoPagedResultDto */} from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import { BehaviorSubject, Observable } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
//import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
//import { ChangeEvent, CKEditorComponent } from '@ckeditor/ckeditor5-angular';

@Component({
selector: 'app-manage-about',
templateUrl: './about.component.html',
changeDetection: ChangeDetectionStrategy.Default,
//styleUrls: ['../lookups.component.scss']
})
export class ManageAboutComponent extends AppComponentBase implements OnInit {

    //public Editor = ClassicEditor;
    //@ViewChild('editorAr') editorAr: CKEditorComponent;
    //@ViewChild('editorEn') editorEn: CKEditorComponent;
    about: DefinitionDto = new DefinitionDto();
    abouts :DefinitionDto []=[];
    aboutForm: FormGroup;
    hasError: boolean;
    isLoading$: Observable<boolean>;
    isLoadingSubject: BehaviorSubject<boolean>;
    formData = new FormData();
  
    constructor(injector: Injector, private fb: FormBuilder, private _aboutService: DefinitionServiceProxy) {
        super(injector);
        this.isLoadingSubject = new BehaviorSubject<boolean>(false);
        this.isLoading$ = this.isLoadingSubject.asObservable();
    }

    ngOnInit(): void {
       this.getAbout();
       this.initForm();
    }

    //public onChange( { editor }: ChangeEvent , type) {
    //    const data = editor.getData();
    //    if(type == 1)
    //        this.about.descriptionAr = data;
    //    else if(type == 2)
    //        this.about.descriptionEn = data;
    //    console.log( data );
    //}

    getAbout() {
        debugger;
        this._aboutService.getAll("", 4, undefined, undefined, )
        .pipe(finalize(() => { }))
            .subscribe((result: any ) => {
               // this.abouts = this.abouts.concat(result.abouts);

            console.log("HERE",result);
                if (result && result.definitions  && result.definitions.length > 0){
                this.about = result.definitions[0];
                    //this.about.nameAr = result.abute.nameAr;
                    //this.about.nameEn = result.abute.nameEn;
                //this.about.nameAr = result.concat(result.about.nameAr);
                //this.about.nameEn = result.descriptionEn;
                //this.editorAr.editorInstance.setData(this.about.descriptionAr);
                //this.editorEn.editorInstance.setData(this.about.descriptionEn);
            }        
        });
    }

    get f() {
        return this.aboutForm.controls;
    }

    initForm() {
        this.aboutForm = this.fb.group({
            nameAr: [
                this.about.nameAr,
                Validators.compose([
                    Validators.required,
                    Validators.minLength(3),
                    Validators.maxLength(320)
                ]),
            ],
            nameEn: [
                this.about.nameEn,
                Validators.compose([
                    Validators.required,
                    Validators.minLength(3),
                    Validators.maxLength(320)
                ]),
            ],
            descriptionAr: [
                this.about.descriptionAr
            ],
            descriptionEn: [
                this.about.descriptionEn
            ]
        });
    }
   
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

    checkEnglishCharNumSympol(event) {
        var ew = event.which;
        if (32 <= ew && ew <= 127)  //All English char and Symbols
            return true;
        return false;
    } //English

    
    submit() {
        this.about.type = 4;
        this.isLoadingSubject.next(true);
        this.manage();
        //if (this.about.id)
        //    this.update();
        //else 
        //    this.create();
    }
    manage() {
        this._aboutService.manage(this.about)
            .pipe(
                finalize(() => {
                    this.isLoadingSubject.next(false);
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
            });
    }
    
}  