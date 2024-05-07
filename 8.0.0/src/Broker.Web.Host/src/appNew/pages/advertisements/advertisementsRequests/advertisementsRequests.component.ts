import { AdvertisementDto } from './../../../../shared/service-proxies/service-proxies';
import { Component, Injector, OnInit } from '@angular/core';
import { DataTableComponentBase, InitDataTableInput } from '@shared/helpers/datatable-component-base';
import {UserDto } from '@shared/service-proxies/service-proxies';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { finalize } from 'rxjs/operators';
import { NbDialogService } from '@nebular/theme';

import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { DialogManageAdvertisementRequestRejectReasonComponent } from './dialog-manage-advertisementRequestRejectReason.component';
class DataTablesResponse {
    data: any[];
    draw: number;
    recordsFiltered: number;
    recordsTotal: number;
}

class PagedAdsRequestDto extends PagedRequestDto {
    keyword: string;
    IsApproved: any | undefined;
    IsEdited: boolean | undefined;
}

@Component({
    selector: 'app-adsRequests',
    templateUrl: './advertisementsRequests.component.html',
    styleUrls: ['./advertisementsRequests.component.scss']
})
export class AdvertisementsRequestsComponent extends DataTableComponentBase {
  
    request = new PagedAdsRequestDto;
    public closeResult: string;
    columnDefs: any[] = [];
    selectedObj: AdvertisementDto = new AdvertisementDto();
    searchValue: any;
    constructor(injector: Injector,private _router:Router, private dialogService: NbDialogService, private fb: FormBuilder) {
        super(injector);
        
    }

    ngOnInit() {

        const initDataTableInput = new InitDataTableInput();
        initDataTableInput.ColumnDefs = [
            {
                data: 'id', targets: 0, orderable: false,
                render: (data, type, full) => {
                    return '<label class="checkbox checkbox-single d-block mb-0"><input type="checkbox" class="checkbox_animated" style="width: 10%;" id=' + data + ' value=' + data + ' /></label>';
                }
            },
            {
                title: this.l("Common.Id"), name: "id", data: 'id', targets: 1, searchable: true, orderable: true,
                render: (data, type, full) => {
                    return data;
                }
            },
            {
                title: this.l("Common.Title"), name: "title", data: 'title', targets: 2, searchable: true, orderable: true,
                render: (data, type, full) => {
                    return data;
                }
            },

            {
                title: this.l("Common.Property"), name: "type", data: 'type', targets: 3, searchable: true, visible: true, orderable: true,
                render: (data, type, full) => {
                    return this.getBuildingType(data);
                }
            },

            {
                title: this.l("Common.Governorate"), name: "governorate",
                data: this.localization.currentLanguage.name != 'en' ? 'governorate.nameAr' : 'governorate.nameEn', targets: 4, searchable: true, visible: true, orderable: true,
                render: (data, type, full) => {
                    return data || '';
                }
            },
            //{
            //    title: this.l("Common.City"), name: "city",
            //    data: this.localization.currentLanguage.name != 'en' ? 'city.nameAr' : 'city.nameEn', targets: 5, searchable: true, visible: true, orderable: true,
            //    render: (data, type, full) => {
            //        return data || '';
            //    }
            //},

            {
                title: this.l("Common.Compound"), name: "compound", data: 'compound', targets: 5, searchable: true, visible: true, orderable: true,
                render: (data, type, full) => {
                    return data;
                }
            },

            {
                title: this.l("Common.Area"), name: "area", data: 'area', targets: 6, searchable: true, visible: true, orderable: true,
                render: (data, type, full) => {
                    return data;
                }
            },

            //{
            //    title: this.l("Common.BuildingArea"), name: "buildingArea", data: 'buildingArea', targets: 8, searchable: true, visible: true, orderable: true,
            //    render: (data, type, full) => {
            //        return data;
            //    }
            //},

            //{
            //    title: this.l("Common.AdvertiseMakerType"), name: "advertiseMaker", data: 'advertiseMaker', targets: 9, searchable: true, visible: true, orderable: true,
            //    render: (data, type, full) => {
            //        return this.getAdvertiseMakerType(data) || "";
            //    }
            //},

            {
                title: this.l('Common.CreatedBy'), name: 'creatorUser.name', data: 'creatorUser.name', targets: [7], searchable: true, visible: false, orderable: true,
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
                title: this.l('Common.CreatedOn'), name: 'creationTime ', data: 'creationTime', targets: [8], searchable: true, visible: false, orderable: true,
                "render": (data, type, full) => {
                    if (full.lastModificationTime) {
                        return this.getDateTimeFormate(full.creationTime);//$filter('date')(full.lastModificationTime, 'dd MMMM yyyy');
                    }
                    else {
                        return '';
                    }
                }
            }
            , {
                title: this.l('Common.ModifiedBy'), name: 'lastModifierUserName', data: 'lastModifierUserName', targets: [9], searchable: true, visible: false, orderable: true,
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
                title: this.l('Common.ModifiedOn'), name: 'lastModificationTime ', data: 'lastModificationTime', targets: [10], searchable: true, visible: false, orderable: true,
                "render": (data, type, full) => {
                    if (full.lastModificationTime) {
                        return this.getDateTimeFormate(full.lastModificationTime);//$filter('date')(full.lastModificationTime, 'dd MMMM yyyy');
                    }
                    else {
                        return '';
                    }
                }
            }

            , {
                title: this.l('Common.Deleted'), name: 'isDeleted', data: 'isDeleted', targets: [11], searchable: true, visible: false, orderable: true,
                "render": (data, type, full) => {
                    if (full.isDeleted)
                        return this.l('Common.Deleted');
                    else
                        return this.l('Common.NotDeleted');
                }
            }
            , {
                title: this.l('Common.DeletedBy'), name: 'deleterUserName', data: 'deleterUserName', targets: [12], searchable: true, visible: false, orderable: true,
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
                title: this.l('Common.DeletedOn'), name: 'deletionTime', data: 'deletionTime', targets: [13], searchable: true, visible: false, orderable: true,
                "render": (data, type, full) => {
                    if (data) {
                        return this.getDateTimeFormate(full.deletionTime);//$filter('date')(data.deletionTime, 'dd MMMM yyyy');
                    }
                    else {
                        return '';
                    }
                }
            },
            {
                title: this.l("Common.Actions"), data: 'id', targets: 14, width: '150px', orderable: false,
                render: (data, type, full) => {
                    if (this.isDeleted == false) {
                        let rowDiv = this.isGranted("Write.Permission.Advertisements") ? '<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-info edit" title=""><i class="fa fa-eye"></i></a>' : '';

                        rowDiv += this.isGranted("Delete.Permission.Advertisements") ? '<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-danger delete confirm" data-action="Delete" data-id="' + data + '" title="' + this.l("Common.Delete") + '"><i class="fa fa-trash"></i></a>' : '';

                        if (full.isApprove != null) {
                            if (full.isApprove) {
                                rowDiv += this.isGranted("Write.Permission.Advertisements") ? '<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-danger rejectResonBalance confirm" data-action="Reject" data-id="' + data + '" title = "' + this.l("Common.Reject") + '"> <i class="fa fa-times"></i></a>' : '';
                            }
                            else {
                                rowDiv += this.isGranted("Write.Permission.Advertisements") ? '<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-success approve confirm" data-action="Approve" data-id="' + data + '" title = "' + this.l("Common.Accept") + '"> <i class="fa fa-check"></i></a>' : '';
                            }
                        }
                        else {
                            rowDiv += this.isGranted("Write.Permission.Advertisements") ? '<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-danger rejectResonBalance confirm" data-action="Reject" data-id="' + data + '" title = "' + this.l("Common.Reject") + '"> <i class="fa fa-times"></i></a>' : '';
                            rowDiv += this.isGranted("Write.Permission.Advertisements") ? '<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-success approve confirm" data-action="Approve" data-id="' + data + '" title = "' + this.l("Common.Accept") + '"> <i class="fa fa-check"></i></a>' : '';


                        }

                        return rowDiv;
                    }
                    else {
                        return this.isGranted("Delete.Permission.Advertisements") ? '<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-danger delete confirm" data-action="Restore" data-id="' + data + '" title="' + this.l("Common.Restore") + '"><i class="fa fa-undo"></i></a>' : '';
                    }
                }
            }


        ];
        //initDataTableInput.InitialParams=[];
        this.setAjaxParam("IsEdited", true);
        //this.refreshAjaxParams();
        initDataTableInput.DataTableSrc = "/api/services/app/Advertisement/IsPaged";
        this.columnDefs = super.initDataTable(initDataTableInput);

    }

    protected edit?(obj: AdvertisementDto): void {
          
        this.selectedObj = obj;
        this._router.navigate(['admin/pages/advertisements/advertisementDetails',obj.id]);
        //this.openModal();
    }


    protected rejectResonBalance?(obj: any): void {
        debugger;
        this.selectedObj = obj;
        this.openReject();
    }
    openReject() {
        this.dialogService.open(DialogManageAdvertisementRequestRejectReasonComponent, {
            context: {
                selectedObj: this.selectedObj,
            },
            closeOnBackdropClick: false,
        }).onClose.subscribe(status => {
            if (status == false) {

                this.setAjaxParam("action", "");
                this.rerender();
            }


        });

    }
    /*addNew() {
        this.selectedObj = new AdvertisementDto();
        this.openModal();
    }*/

    openModal() {
        
    }

    protected delete(obj): void {

    }

    list(pageNumber: number, finishedCallback: Function): void {

    }

    searchName(value, IsApproved,Newer) {
        debugger;

        if (this.request != null) {
            this.addAjaxParam("Name", value);
            this.addAjaxParam("IsApprove", undefined);
            this.addAjaxParam("IsEdited", true);

        }
        else {
            this.clearAjaxParams();
        }
        this.rerender();
    }

    clearSearch() {
        debugger;
        this.addAjaxParam("Name", "");
        this.addAjaxParam("IsApprove", undefined);
        this.addAjaxParam("IsEdited",true);
        this.rerender();
    }

    getBuildingType(status) {
        switch (status) {
            case 1:
                return this.l('Common.Apartment');
            case 2:
                return this.l('Common.Villa');
            case 3:
                return this.l('Common.ChaletForSummer');
            case 4:
                return this.l('Common.Building');
            case 5:
                return this.l('Common.AdminOffice');
            case 6:
                return this.l('Common.Shop');
            case 7:
                return this.l('Common.Land');
            default:
                return "";
        }
    }

    getAdvertiseMakerType(status) {
        switch (status) {
            case 1:
                return this.l('Common.Owner');
            case 2:
                return this.l('Common.Broker');
            default:
                return "";
        }
    }

}