import { Component, Injector, OnInit } from '@angular/core';
import { DataTableComponentBase, InitDataTableInput } from '@shared/helpers/datatable-component-base';
import { DiscountCodeDto } from '@shared/service-proxies/service-proxies';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { finalize } from 'rxjs/operators';
import { NbDialogService } from '@nebular/theme';
import { DialogManageDiscountCodeComponent } from './dialog-manage-discountCode.component';
import * as moment from 'moment';

class DataTablesResponse {
    data: any[];
    draw: number;
    recordsFiltered: number;
    recordsTotal: number;
}

class PagedDocumentsRequestDto extends PagedRequestDto {
    keyword: string;
}


@Component({
    selector: 'app-admin-discountCodes',
    templateUrl: './discountCodes.component.html',
    styleUrls: ['./discountCodes.component.scss']
})
export class AdminDiscountCodesComponent extends DataTableComponentBase {
    protected rejectResonBalance?(obj: object): void {
        throw new Error('Method not implemented.');
    }

    public closeResult: string;
    columnDefs: any[] = [];
    searchValue: any;
    selectedObj: DiscountCodeDto = new DiscountCodeDto();

    constructor(injector: Injector, private dialogService: NbDialogService) {
        super(injector);
    }


    ngOnInit() {
        this.addAjaxParam("EnumCategory", 2);

        const initDataTableInput = new InitDataTableInput();
        initDataTableInput.ColumnDefs = [
            {
                name: "id", data: 'id', targets: 0, orderable: false,
                render: (data, type, full) => {
                    return '<label class="checkbox checkbox-single d-block mb-0"><input type="checkbox" class="checkbox_animated" id=' + data + ' value=' + data + ' /></label>';
                }
            },
            {
                title: this.l('Common.Code'), searchable: true, name: "code", data: 'code', targets: 1, orderable: true,
                render: (data, type, full) => {
                    return data || '';
                }
            },
            {
                title: this.l('Common.Percentage'), searchable: true, name: "percentage", data: 'percentage', targets: 2, orderable: true,
                render: (data, type, full) => {
                    return data || '';
                }
            },
            //{
            //    title: this.l('Common.FixedAmount'), searchable: true, name: "fixedAmount", data: 'fixedAmount', targets: 3, orderable: true,
            //    render: (data, type, full) => {
            //        return data || '';
            //    }
            //},
            {
                title: this.l('Common.IsPublish'), searchable: true, name: "isPublish", data: 'isPublish', targets: 3, orderable: true,
                render: (data, type, full) => {
                    return data;
                }
            },
            {
                title: this.l('Common.From'), searchable: true, name: "from", data: 'from', targets: 4, orderable: true,
                render: (data, type, full) => {
                    //return data?moment(data).format('YYYY/MM/DD'):'';
                    if (data) {
                        return this.getDateFormate(full.from);
                    }
                    else {
                        return '---';
                    }
                }
            },
            {
                title: this.l('Common.To'), searchable: true, name: "to", data: 'to', targets: 5, orderable: true,
                render: (data, type, full) => {
                    // return data?moment(data).format('YYYY/MM/DD'):'';
                    if (data) {
                        return this.getDateFormate(full.to);
                    }
                        else {
                        return '---';
                    }
                    }
                
            },
            {
                title: this.l('Common.CreatedBy'), name: 'creatorUserName', data: 'creatorUser.name', targets: [6], searchable: true, visible: false, orderable: true,
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
                title: this.l('Common.CreatedOn'), name: 'creationTime', data: 'creationTime', targets: [7], searchable: true, visible: false, orderable: true,
                "render": (data, type, full) => {
                    if (full.creationTime)
                        return this.getDateTimeFormate(full.creationTime);
                    else
                        return '';
                }
            }
            , {
                title: this.l('Common.ModifiedBy'), name: 'lastModifierUserName', data: 'lastModifierUser.name', targets: [8], searchable: true, visible: false, orderable: true,
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
                title: this.l('Common.ModifiedOn'), name: 'lastModificationTime ', data: 'lastModificationTime', targets: [9], searchable: true, visible: false, orderable: true,
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
                title: this.l('Common.Deleted'), name: 'isDeleted', data: 'isDeleted', targets: [10], searchable: true, visible: false, orderable: true,
                "render": (data, type, full) => {
                    if (full.isDeleted)
                        return this.l('Common.Deleted');
                    else
                        return this.l('Common.NotDeleted');
                }
            }
            , {
                title: this.l('Common.DeletedBy'), name: 'deleterUserName', data: 'deleterUser.name', targets: [11], searchable: true, visible: false, orderable: true,
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
                title: this.l('Common.DeletedOn'), name: 'deletionTime', data: 'deletionTime', targets: [12], searchable: true, visible: false, orderable: true,
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
                targets: 13, data: 'id', title: this.l('Common.Actions'), width: '100px', visible: true, orderable: false,
                render: (data, type, full) => {
                    if (this.isDeleted == false) {
                        let rowDiv = this.isGranted("Write.Permission.DiscountCodes") ?'<button type="button" class="btn btn-round btn-primary buttons-html5 edit"><i class="fa fa-edit"></i></button>' : "";
                        rowDiv += this.isGranted("Delete.Permission.DiscountCodes") ?'<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-danger delete confirm" data-action="Delete" data-id="' + data + '" title="' + this.l("Common.Delete") + '"><i class="fa fa-trash"></i></a>' : "";
                        
                        return rowDiv;
                    }
                    else {
                        return this.isGranted("Delete.Permission.DiscountCodes") ? '<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-danger delete confirm" data-action="Restore" data-id="' + data + '" title="' + this.l("Common.Restore") + '"><i class="fa fa-undo"></i></a>' : "";
                    }
                }
            }
        ];

        initDataTableInput.DataTableSrc = "/api/services/app/DiscountCode/IsPaged";
        this.columnDefs = super.initDataTable(initDataTableInput);

    }

    protected edit?(obj: DiscountCodeDto): void {
        this.selectedObj = obj;
        this.openModal();
    }

    addNew() {
        this.selectedObj = new DiscountCodeDto();
        this.openModal();
       
    }
    

    openModal() {
        this.dialogService.open(DialogManageDiscountCodeComponent, {
            
            context: {
                discountCodeId: this.selectedObj.id,
                discountCodeCode: this.selectedObj.code,
                //discountCodeFixedAmount: this.selectedObj.fixedAmount,
                discountCodePercentage: this.selectedObj.percentage,
                discountCodeIsPublish: this.selectedObj.isPublish,
                from:new Date(this.selectedObj?.from?.toString()),  
                to: new Date(this.selectedObj?.to?.toString()),
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
}
