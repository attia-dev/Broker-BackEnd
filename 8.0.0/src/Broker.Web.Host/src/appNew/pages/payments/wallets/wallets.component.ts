import { Component, Injector, OnInit } from '@angular/core';
import { DataTableComponentBase, InitDataTableInput } from '@shared/helpers/datatable-component-base';
import { WalletDto } from '@shared/service-proxies/service-proxies';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { finalize } from 'rxjs/operators';
import { NbDialogService } from '@nebular/theme';
import { DialogManageWalletComponent } from './dialog-manage-wallet.component';
import { AppConsts } from '../../../../shared/AppConsts';
import { debug } from 'console';
import { ActivatedRoute } from '@angular/router';
class DataTablesResponse {
    data: any[];
    draw: number;
    recordsFiltered: number;
    recordsTotal: number;
}

class PagedCountriesRequestDto extends PagedRequestDto {
    keyword: string;
}


@Component({
    selector: 'app-admin-wallets',
    templateUrl: './wallets.component.html',
    styleUrls: ['./wallets.component.scss']
})
export class AdminWalletsComponent extends DataTableComponentBase {
    protected rejectResonBalance?(obj: object): void {
        throw new Error('Method not implemented.');
    }

    public closeResult: string;
    columnDefs: any[] = [];
    selectedObj: WalletDto = new WalletDto();
    searchValue: any;
    public baseUrl2: string;
    companyId: any;
    constructor(injector: Injector, private dialogService: NbDialogService, private route: ActivatedRoute) {
        super(injector);
        this.baseUrl2 = AppConsts.remoteServiceBaseUrl + '/Resources/UploadFiles/';
        this.companyId = this.route.snapshot.paramMap.get('id');
    }

    ngOnInit() {
        /*this.addAjaxParam("EnumCategory", 9);*/
        debugger;
        const initDataTableInput = new InitDataTableInput();
        initDataTableInput.ColumnDefs = [
            {
                name: "id", data: 'id', targets: 0, orderable: false,
                render: (data, type, full) => {
                    return '<label class="checkbox checkbox-single d-block mb-0"><input type="checkbox" class="checkbox_animated" id=' + data + ' value=' + data + ' /></label>';
                }
            },
            {
                title: this.l('Common.Type'), searchable: true, name: "type", data: 'type', targets: 1, orderable: true,
                render: (data, type, full) => {
                    switch (data) {
                        case 1:
                            return this.l('Pages.TransactionType.Incom');
                            break;
                        case 2:
                            return this.l('Pages.TransactionType.OutCome');
                            break;
                        default:
                }
                    
                }
            },
            {
                title: this.l('Common.Amount'), searchable: true, name: "amount", data: 'amount', targets: 2, orderable: true,
                render: (data, type, full) => {
                    return data || '';
                }
            },
            {
                title: this.l('Common.Points'), searchable: true, name: "points", data: 'points', targets: 3, orderable: true,
                render: (data, type, full) => {
                    return data || '';
                }
            },
            
            {
                title: this.l('Common.TransactionTime'), searchable: true, name: "transactionTime", data: 'transactionTime', targets: 4, orderable: true,
                "render": (data, type, full) => {
                    if (full.transactionTime)
                        return this.getDateTimeFormate(full.transactionTime);
                    else
                        return '';
                }
            },
            
            {
                title: this.l('Common.CreatedBy'), name: 'creatorUserName', data: 'creatorUser.name', targets: [5], searchable: true, visible: false, orderable: true,
                render: (data, type, full) => {
                    if (full.creatorUser) {
                        return full.creatorUser.name;
                    }
                    else {
                        return this.l('Common.System');
                    }
                }
            }
            , {
                title: this.l('Common.CreatedOn'), name: 'creationTime', data: 'creationTime', targets: [6], searchable: true, visible: false, orderable: true,
                "render": (data, type, full) => {
                    if (full.creationTime)
                        return this.getDateTimeFormate(full.creationTime);
                    else
                        return '';
                }
            }
            , {
                title: this.l('Common.ModifiedBy'), name: 'lastModifierUserName', data: 'lastModifierUser.name', targets: [7], searchable: true, visible: false, orderable: true,
                "render": (data, type, full) => {
                    if (!full.lastModifierUserId)
                        return '';
                    if (full.lastModifierUser) {
                        return full.lastModifierUser.name;
                    }
                    else {
                        return '';
                    }
                }
            }
            , {
                title: this.l('Common.ModifiedOn'), name: 'lastModificationTime ', data: 'lastModificationTime', targets: [8], searchable: true, visible: false, orderable: true,
                "render": (data, type, full) => {
                    if (full.lastModificationTime) {
                        return this.getDateTimeFormate(full.lastModificationTime);
                    }
                    else {
                        return '';
                    }
                }
            }
            , {
                title: this.l('Common.Deleted'), name: 'isDeleted', data: 'isDeleted', targets: [9], searchable: true, visible: false, orderable: true,
                "render": (data, type, full) => {
                    if (full.isDeleted)
                        return this.l('Common.Deleted');
                    else
                        return this.l('Common.NotDeleted');
                }
            }
            , {
                title: this.l('Common.DeletedBy'), name: 'deleterUserName', data: 'deleterUser.name', targets: [10], searchable: true, visible: false, orderable: true,
                "render": (data, type, full) => {
                    if (!full.deleterUserId)
                        return '';
                    if (full.deleterUser) {
                        return full.deleterUser.name;
                    }
                    else {
                        return this.l('Common.System');
                    }
                }
            }
            , {
                title: this.l('Common.DeletedOn'), name: 'deletionTime', data: 'deletionTime', targets: [11], searchable: true, visible: false, orderable: true,
                "render": (data, type, full) => {
                    if (data) {
                        return this.getDateTimeFormate(full.deletionTime);
                    }
                    else {
                        return '';
                    }
                }
            },
            {
                targets: 12, data: 'id', title: this.l('Common.Actions'), width: '100px', visible: true, orderable: false,
                render: (data, type, full) => {
                    if (this.isDeleted == false) {
                        let rowDiv =  '<button type="button" class="btn btn-round btn-primary buttons-html5 edit">'+this.l("Common.View")+'</button>' ;
                        /*rowDiv += this.isGranted("Delete.Permission.DoctorTitle") ? '<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-danger delete confirm" data-action="Delete" data-id="' + data + '" title="' + this.l("Common.Delete") + '"><i class="fa fa-trash"></i></a>' : "";*/
                        return rowDiv;
                    }
                    else {
                        return "";
                    }
                }
            }
        ];

        this.addAjaxParam("CompanyId", this.companyId);
        initDataTableInput.DataTableSrc = "/api/services/app/Wallet/IsPaged";
        this.columnDefs = super.initDataTable(initDataTableInput);

    }

    protected edit?(obj: WalletDto): void {
        this.selectedObj = obj;
        this.openModal();
    }

    addNew() {
        this.selectedObj = new WalletDto();
        this.openModal();
    }

    openModal() {
        this.dialogService.open(DialogManageWalletComponent, {
            context: {
                amount: this.selectedObj.amount,
                points: this.selectedObj.points,
                
                transactionTime: this.selectedObj.transactionTime,
                
            },
            closeOnBackdropClick: false,
        }).onClose.subscribe(status => {
            if (status == true) {
                this.rerender();
            }
        });
    }

    protected delete(obj): void {

    }

    list(pageNumber: number, finishedCallback: Function): void {

    }
    //searchName(event) {
    //    debugger;
    //    if (event.target.value != '' || event.target.value != null) {
    //        this.addAjaxParam("Name", event.target.value);
    //    }
    //    else {
    //        this.clearAjaxParams();
    //    }
    //    this.rerender();
    //}

    //clearSearch() {
    //    debugger;
    //    this.searchValue = '';
    //    this.clearAjaxParams();
    //    this.rerender();
    //}
}