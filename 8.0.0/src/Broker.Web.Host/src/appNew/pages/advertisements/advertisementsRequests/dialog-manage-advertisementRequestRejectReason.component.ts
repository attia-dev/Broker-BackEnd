import { Component, Injector, OnInit } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import { AdvertisementDto, AdvertisementServiceProxy} from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';
import { HttpEventType, HttpClient } from '@angular/common/http';

@Component({
    selector: 'ngx-dialog-manage-advertisementRequestRejectReason',
    templateUrl: 'dialog-manage-advertisementRequestRejectReason.component.html',
    styleUrls: ['advertisementsRequests.component.scss'],
})
export class DialogManageAdvertisementRequestRejectReasonComponent extends AppComponentBase implements OnInit {

    selectedObj: AdvertisementDto;
    public selectedForm: FormGroup;
    ShowFilter = true;
    constructor(injector: Injector, private fb: FormBuilder, private _advertisementService: AdvertisementServiceProxy,  private http: HttpClient, protected ref: NbDialogRef<DialogManageAdvertisementRequestRejectReasonComponent>) {
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
        this.selectedObj.isApprove = false;
        this.manage();  
    }
   
    manage() {
        this._advertisementService.manageReject(this.selectedObj)
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
