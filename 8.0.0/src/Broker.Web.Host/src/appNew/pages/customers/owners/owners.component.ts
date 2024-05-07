import { Component, Injector, OnInit } from '@angular/core';
import { DataTableComponentBase, InitDataTableInput } from '@shared/helpers/datatable-component-base';
import { CityDto,CountryDto, OwnerDto } from '@shared/service-proxies/service-proxies';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { finalize } from 'rxjs/operators';
import { NbDialogService } from '@nebular/theme';

class DataTablesResponse {
    data: any[];
    draw: number;
    recordsFiltered: number;
    recordsTotal: number;
}

class PagedOwnersRequestDto extends PagedRequestDto {
    keyword: string;
}


@Component({
    selector: 'app-admin-owners',
    templateUrl: './owners.component.html',
    styleUrls: ['./owners.component.scss']
})
export class AdminOwnersComponent extends DataTableComponentBase {
    protected rejectResonBalance?(obj: object): void {
        throw new Error('Method not implemented.');
    }
    public closeResult: string;
    columnDefs: any[] = [];
    selectedObj: OwnerDto = new OwnerDto();
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
                title: this.l('Common.Name'), searchable: true, name: "user.name", data: 'user.name', targets: 1, orderable: true,
                render: (data, type, full) => {
                    return data || '';
                }
            },
            {
                title: this.l('Common.MobileNumber'), searchable: true, name: "user.phoneNumber", data: 'user.phoneNumber', targets: 2, orderable: true,
                render: (data, type, full) => {
                    return data || '';
                }
            },
            {
                title: this.l('Common.IsActive'), searchable: true, name: "isActive", data: 'isActive', targets: 3, orderable: true,
                render: (data, type, full) => {
                    /* return data || '';*/
                    switch (data) {
                        case true:
                            return this.l('Common.Active');
                            break;
                        case false:
                            return this.l('Common.Deactivated');
                            break;
                        default:
                        //return null;
                    }
                }
            },
            {
                title: this.l('Common.CreatedBy'), name: 'creatorUser.name', data: 'creatorUser.name', targets: [4], searchable: true, visible: false, orderable: true,
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
                title: this.l('Common.CreatedOn'), name: 'creationTime', data: 'creationTime', targets: [5], searchable: true, visible: false, orderable: true,
                "render": (data, type, full) => {
                    if (full.creationTime)
                        return this.getDateTimeFormate(full.creationTime);
                    else
                        return '';
                }
            }
            , {
                title: this.l('Common.ModifiedBy'), name: 'lastModifierUserName', data: 'lastModifierUser.Name', targets: [6], searchable: true, visible: false, orderable: true,
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
                title: this.l('Common.ModifiedOn'), name: 'lastModificationTime ', data: 'lastModificationTime', targets: [7], searchable: true, visible: false, orderable: true,
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
                title: this.l('Common.Deleted'), name: 'isDeleted', data: 'isDeleted', targets: [8], searchable: true, visible: false, orderable: true,
                "render": (data, type, full) => {
                    if (full.isDeleted)
                        return this.l('Common.Deleted');
                    else
                        return this.l('Common.NotDeleted');
                }
            }
            , {
                title: this.l('Common.DeletedBy'), name: 'deleterUserName', data: 'deleterUser.Name', targets: [9], searchable: true, visible: false, orderable: true,
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
                title: this.l('Common.DeletedOn'), name: 'deletionTime', data: 'deletionTime', targets: [10], searchable: true, visible: false, orderable: true,
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
                targets: 11, data: 'id', title: this.l('Common.Actions'), width: '100px', visible: true, orderable: false,
                render: (data, type, full) => {
                    if (this.isDeleted == false) {
                        let rowDiv = /*this.isGranted("Write.Permission.Cities") ?*/ /*'<button type="button" class="btn btn-round btn-primary buttons-html5 edit"><i class="fa fa-edit"></i></button>'*/ /*: ""*/  "";
                        if (full.isActive == true) {
                            rowDiv += this.isGranted("Write.Permission.Owners") ? '<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-danger deactivate confirm" data-action="deactivate" data-id="' + data + '" title = "' + this.l("Common.Deactivate") + '"> <i class="fa fa-times"></i></a>' : '';
                        }
                        else {
                            rowDiv += this.isGranted("Write.Permission.Owners") ? '<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-success active confirm" data-action="active" data-id="' + data + '" title = "' + this.l("Common.Active") + '"> <i class="fa fa-check"></i></a>' : '';
                        }
                        rowDiv += this.isGranted("Delete.Permission.Owners") ?  '<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-danger delete confirm" data-action="Delete" data-id="' + data + '" title="' + this.l("Common.Delete") + '"><i class="fa fa-trash"></i></a>' : "";
                        return rowDiv;
                    }
                    else {
                        return this.isGranted("Delete.Permission.Owners") ?  '<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-danger delete confirm" data-action="Restore" data-id="' + data + '" title="' + this.l("Common.Restore") + '"><i class="fa fa-undo"></i></a>' : "";
                    }
                }
            }
        ];

        initDataTableInput.DataTableSrc = "/api/services/app/Owner/IsPaged";
        this.columnDefs = super.initDataTable(initDataTableInput);

    }

    protected edit?(obj: OwnerDto): void {
        //this.selectedObj = obj;
        //this.selectCountry = [];
        //const x = new CountryDto();
        //x.id = this.selectedObj.countryId;
        //x.nameEn = this.selectedObj.country.nameEn;
        //x.nameAr = this.selectedObj.country.nameAr;
        //this.selectCountry.push(x);
        //this.openModal();
    }

    addNew() {
        //this.selectCountry = [];
        //this.selectedObj = new OwnerDto();
        //this.openModal();
    }

    //openModal() {
    //    this.dialogService.open(DialogManageOwnerComponent, {
    //        context: {
    //            governorateId: this.selectedObj.id,
    //            governorateNameAr: this.selectedObj.nameAr,
    //            governorateNameEn: this.selectedObj.nameEn,
    //            selectCountry: this.selectCountry
    //        },
    //        closeOnBackdropClick: false,
    //    }).onClose.subscribe(status => {
    //        if (status == true) {
    //            this.rerender();
    //        }
    //    });
    //}

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