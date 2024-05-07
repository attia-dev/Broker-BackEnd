import { Component, Injector, OnInit } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import { ProjectServiceProxy, ProjectDto} from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';
import { HttpEventType, HttpClient } from '@angular/common/http';

@Component({
    selector: 'ngx-dialog-manage-projectRejectReason',
    templateUrl: 'dialog-manage-projectRejectReason.component.html',
    styleUrls: ['projects.component.scss'],
})
export class DialogManageProjectRejectReasonComponent extends AppComponentBase implements OnInit {

    selectedObj: ProjectDto;
    public selectedForm: FormGroup;
    ShowFilter = true;
    constructor(injector: Injector, private fb: FormBuilder, private _projectService: ProjectServiceProxy,  private http: HttpClient, protected ref: NbDialogRef<DialogManageProjectRejectReasonComponent>) {
        super(injector);

        
    }

    ngOnInit() {

        this.selectedForm = this.fb.group({
            rejectReason: [this.selectedObj.rejectReason, [Validators.required]],
        }) 
    }

    cancel() {
        this.ref.close();
    }

    submit() { 
        debugger;
      
        this.manage();  
    }
   
    manage() {
        this._projectService.manageReject(this.selectedObj)
            .pipe(
                finalize(() => {
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('Common.Message.ActionSuccess'));
                setTimeout(() => {
                    this.ref.close(true);
                    //this.rerender();
                }, 1000);
            });
    }

}
