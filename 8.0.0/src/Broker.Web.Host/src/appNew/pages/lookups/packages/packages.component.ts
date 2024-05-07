import { Component, Injector, OnInit } from '@angular/core';
import { DataTableComponentBase, InitDataTableInput } from '@shared/helpers/datatable-component-base';
import { PackageDto } from '@shared/service-proxies/service-proxies';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { finalize } from 'rxjs/operators';
import { NbDialogService } from '@nebular/theme';
import { DialogManagePackageComponent } from './dialog-manage-package.component';

class DataTablesResponse {
    data: any[];
    draw: number;
    recordsFiltered: number;
    recordsTotal: number;
}

class PagedPackagesRequestDto extends PagedRequestDto {
    keyword: string;
}


@Component({
    selector: 'app-admin-packages',
    templateUrl: './packages.component.html',
    styleUrls: ['./packages.component.scss']
})
export class AdminPackagesComponent extends DataTableComponentBase {
    protected rejectResonBalance?(obj: object): void {
        throw new Error('Method not implemented.');
    }

    public closeResult: string;
    columnDefs: any[] = [];
    selectedObj: PackageDto = new PackageDto();
    searchValue: any;
    constructor(injector: Injector, private dialogService: NbDialogService) {
        super(injector);
    }

    ngOnInit() {

        const initDataTableInput = new InitDataTableInput();
        initDataTableInput.ColumnDefs = [
            {
                name: "id", data: 'id', targets: 0, orderable: false,
                render: (data, type, full) => {
                    return '<label class="checkbox checkbox-single d-block mb-0"><input type="checkbox" class="checkbox_animated" id=' + data + ' value=' + data + ' /></label>';
                }
            },
            {
                title: this.l('Common.NameAr'), searchable: true, name: "nameAr", data: 'nameAr', targets: 1, orderable: true,
                render: (data, type, full) => {
                    return data || '';
                }
            },
            {
                title: this.l('Common.NameEn'), searchable: true, name: "nameEn", data: 'nameEn', targets: 2, orderable: true,
                render: (data, type, full) => {
                    return data || '';
                }
            },{
                title: this.l('Common.Price'), searchable: true, name: "price", data: 'price', targets: 3, orderable: true,
                render: (data, type, full) => {
                    return data || '';
                }
            },
            {
                title: this.l('Common.Points'), searchable: true, name: "points", data: 'points', targets: 4, orderable: true,
                render: (data, type, full) => {
                    return data || '';
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
                        let rowDiv = this.isGranted("Write.Permission.Packages") ?'<button type="button" class="btn btn-round btn-primary buttons-html5 edit"><i class="fa fa-edit"></i></button>' : "";
                        rowDiv += this.isGranted("Delete.Permission.Packages") ?'<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-danger delete confirm" data-action="Delete" data-id="' + data + '" title="' + this.l("Common.Delete") + '"><i class="fa fa-trash"></i></a>' : "";
                        return rowDiv;
                    }
                    else {
                        return this.isGranted("Delete.Permission.Packages") ? '<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-danger delete confirm" data-action="Restore" data-id="' + data + '" title="' + this.l("Common.Restore") + '"><i class="fa fa-undo"></i></a>' : "";
                    }
                }
            }
        ];

        initDataTableInput.DataTableSrc = "/api/services/app/Package/IsPaged";
        this.columnDefs = super.initDataTable(initDataTableInput);

    }

    protected edit?(obj: PackageDto): void {
        this.selectedObj = obj;
        this.openModal();
    }

    addNew() {
        this.selectedObj = new PackageDto();
        this.openModal();
    }

    openModal() {
        this.dialogService.open(DialogManagePackageComponent, {
            context: {
                packageId: this.selectedObj.id,
                packageNameAr: this.selectedObj.nameAr,
                packageNameEn: this.selectedObj.nameEn,
                packagePrice: this.selectedObj.price,
                packagePoints: this.selectedObj.points
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
    searchName(event) {
        debugger;
        if (event.target.value != '' || event.target.value != null) {
            this.addAjaxParam("Name", event.target.value);
        }
        else {
            this.clearAjaxParams();
        }
        this.rerender();
    }
     
    clearSearch() {
        debugger;
        this.searchValue = '';
        this.clearAjaxParams();
        this.rerender();
    }
}
