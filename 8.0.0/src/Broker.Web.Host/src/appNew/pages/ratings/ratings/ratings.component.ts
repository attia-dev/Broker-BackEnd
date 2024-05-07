import { AdvertisementDto, RateDto } from './../../../../shared/service-proxies/service-proxies';
import { Component, Injector, OnInit } from '@angular/core';
import { DataTableComponentBase, InitDataTableInput } from '@shared/helpers/datatable-component-base';
import {UserDto } from '@shared/service-proxies/service-proxies';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { finalize } from 'rxjs/operators';
import { NbDialogService } from '@nebular/theme';

import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
class DataTablesResponse {
    data: any[];
    draw: number;
    recordsFiltered: number;
    recordsTotal: number;
}

class PagedUsersRequestDto extends PagedRequestDto {
    keyword: string;
}


@Component({
    selector: 'app-ratings',
    templateUrl: './ratings.component.html',
    styleUrls: ['./ratings.component.scss']
})
export class RatingsComponent extends DataTableComponentBase {
    protected rejectResonBalance?(obj: object): void {
        throw new Error('Method not implemented.');
    }
    request = new PagedUsersRequestDto;
    public closeResult: string;
    columnDefs: any[] = [];
    selectedObj: RateDto = new RateDto();
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
                title: this.l("UserName"), name: "user",
                data: this.localization.currentLanguage.name != 'en' ? 'user.name' : 'user.surname', targets: 1, searchable: true, visible: true, orderable: true,
                render: (data, type, full) => {
                    return data;
                }
            },

            {
                title: this.l("Common.PhoneNumber"), name: "phoneNumber",
                data: 'user.phoneNumber', targets: 2, searchable: true, visible: true, orderable: true,
                render: (data, type, full) => {
                    return data;
                }
            },

            {
                title: this.l("EmailAddress"), name: "emailAddress",
                data: 'user.emailAddress', targets: 3, searchable: true, visible: true, orderable: true,
                render: (data, type, full) => {
                    return data;
                }
            },
           
            {
                title: this.l("Common.RatingValue"), name: "userRate",
                data: 'userRate', targets: 4, searchable: true, visible: true, orderable: true,
                render: (data, type, full) => {
                    return data;
                }
            },

           
            {
                title: this.l('Common.CreatedBy'), name: 'creatorUser.name', data: 'creatorUser.name', targets: [5], searchable: true, visible: false, orderable: true,
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
                title: this.l('Common.CreatedOn'), name: 'creationTime ', data: 'creationTime', targets: [6], searchable: true, visible: false, orderable: true,
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
                title: this.l('Common.ModifiedBy'), name: 'lastModifierUserName', data: 'lastModifierUserName', targets: [7], searchable: true, visible: false, orderable: true,
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
                        return this.getDateTimeFormate(full.lastModificationTime);//$filter('date')(full.lastModificationTime, 'dd MMMM yyyy');
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
                title: this.l('Common.DeletedBy'), name: 'deleterUserName', data: 'deleterUserName', targets: [10], searchable: true, visible: false, orderable: true,
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
                        return this.getDateTimeFormate(full.deletionTime);//$filter('date')(data.deletionTime, 'dd MMMM yyyy');
                    }
                    else {
                        return '';
                    }
                }
            },
            /*{
                title: this.l("Common.Actions"), data: 'id', targets: 13, width: '150px', orderable: false,
                render: (data, type, full) => {
                    if (this.isDeleted == false) {
                        let rowDiv = this.isGranted("Write.Permission.Advertisements") ? '<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-info edit" title=""><i class="fa fa-eye"></i></a>' : '';
                        
                        rowDiv += this.isGranted("Delete.Permission.Advertisements") ? '<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-danger delete confirm" data-action="Delete" data-id="' + data + '" title="' + this.l("Common.Delete") + '"><i class="fa fa-trash"></i></a>' : '';
                        
                           
                    }
                    else {
                        return this.isGranted("Delete.Permission.Advertisements") ? '<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-danger delete confirm" data-action="Restore" data-id="' + data + '" title="' + this.l("Common.Restore") + '"><i class="fa fa-undo"></i></a>' : '';
                    }
                }
            }*/
            
            
        ];
        initDataTableInput.InitialParams=[];
        this.setAjaxParam("Name","");
        //this.refreshAjaxParams();
        initDataTableInput.DataTableSrc = "/api/services/app/Rate/IsPaged";
        this.columnDefs = super.initDataTable(initDataTableInput);

    }

    protected edit?(obj: RateDto): void {
          
        //this.selectedObj = obj;
       // this.openModal();
    }

    addNew() {
        //this.selectedObj = new RateDto();
       // this.openModal();
    }

    openModal() {
        
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