import { Component, Injector, OnInit } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import { DefinitionDto, DefinitionServiceProxy,/*, PagedDefinitionResultRequestDto*/ 
WalletDto,
WalletServiceProxy} from '@shared/service-proxies/service-proxies';
import { AppConsts } from '../../../../shared/AppConsts';
import { HttpEventType, HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
@Component({
    selector: 'ngx-dialog-manage-wallet',
    templateUrl: 'dialog-manage-wallet.component.html',
    styleUrls: ['./wallets.component.scss'],
})
export class DialogManageWalletComponent extends AppComponentBase implements OnInit {

    definitionId: any;
    selectedObj: WalletDto;
    public walletForm: FormGroup;
    public progress: number=0;
    public message1: string;
    public baseUrl: string;
    ShowFilter = true;
    definitions: WalletDto[] = [];
    amount: any;
    points: any;
    jobTitle: any;
    entity: any;
    transactionTime: any;
    endDate: any;
    isStillWorking: any;


    constructor(private http: HttpClient, private route: ActivatedRoute, private router: Router, injector: Injector, private fb: FormBuilder, private _walletService: WalletServiceProxy, protected ref: NbDialogRef<DialogManageWalletComponent>) {
        super(injector);
        this.baseUrl = AppConsts.remoteServiceBaseUrl + '/Resources/UploadFiles/';
        /*this.selectedObj = new ExperienceDto();*/
    }

    ngOnInit() {

        this.walletForm = this.fb.group({
            amount: [this.selectedObj.amount],
            points: [this.selectedObj.points],
            //jobTitle: [this.selectedObj.jobTitle],
            //entity: [this.selectedObj.entity],
            transactionTime: [this.selectedObj.transactionTime],
            
        })
    }
    //getDateFormate(date): string {
    //    //return moment(date).format('DD MMM YYYY');
    //    return this.convertDateFromHijri(date, 'D MMM YYYY', this.localization.currentLanguage.name);
    //}

    cancel() {
        debugger;
        this.ref.close();
    }

    submit() {

    }

    manage(input) {
        this._walletService.manage(input)
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
