import { Component, Injector, OnInit } from '@angular/core';
import { DataTableComponentBase, InitDataTableInput } from '@shared/helpers/datatable-component-base';
import {UserDto } from '@shared/service-proxies/service-proxies';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { finalize } from 'rxjs/operators';
import { NbDialogService } from '@nebular/theme';
import { DialogManageUsersComponent } from './dialog-manage-users.component';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
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
    selector: 'app-users',
    templateUrl: './users.component.html',
    styleUrls: ['./users.component.scss']
})
export class UserComponent extends DataTableComponentBase {
    protected rejectResonBalance?(obj: object): void {
        throw new Error('Method not implemented.');
    }
    request = new PagedUsersRequestDto;
    public closeResult: string;
    columnDefs: any[] = [];
    selectedObj: UserDto = new UserDto();
    searchValue: any;
    constructor(injector: Injector, private dialogService: NbDialogService, private fb: FormBuilder) {
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
                title: this.l('Common.CreatedOn'), name: 'creationTime', data: 'creationTime', targets: [1], searchable: true, visible: false, orderable: true,
                "render": (data, type, full) => {
                    if (full.creationTime)
                        return this.getDateTimeFormate(full.creationTime);//$filter('date')(full.creationTime, 'dd MMMM yyyy');
                    else
                        return '';
                }
            },
            { 
                title: this.l("Common.Name"), name: "name", data: 'name', targets: 2, searchable: true, orderable: true,
                render: (data, type, full) => {
                  if (this.isGranted("Read.Permission.UserPermissions"))
                    return '<a href="#/admin/pages/users/permissionUser/' + full.id + '" class="" title="">' + data + '</a>';
                  else //permissionUser/:id
                    return data;
                }
            },
            {
                title: this.l("UserName"), name: "userName", data: 'userName', targets: 3, searchable: true, visible: true, orderable: true,
                render: (data, type, full) => {
                    return data;
                }
            },
            {
                title: this.l("Common.EmailAddress"), name: "emailAddress", data: 'emailAddress', targets: 4, searchable: true, visible: true, orderable: true,
                render: (data, type, full) => {
                    return data;
                }
            },
           
            //{
            //    title: this.l("Common.PhoneNumber"), name: "phoneNumber", data: 'phoneNumber', targets: 4, searchable: true, visible: true, orderable: true,
            //    render: (data, type, full) => {
            //        return data;
            //    }
            //},
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
                title: this.l('Common.ModifiedBy'), name: 'lastModifierUserName', data: 'lastModifierUserName', targets: [6], searchable: true, visible: false, orderable: true,
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
                        return this.getDateTimeFormate(full.lastModificationTime);//$filter('date')(full.lastModificationTime, 'dd MMMM yyyy');
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
                title: this.l('Common.DeletedBy'), name: 'deleterUserName', data: 'deleterUserName', targets: [9], searchable: true, visible: false, orderable: true,
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
                        return this.getDateTimeFormate(full.deletionTime);//$filter('date')(data.deletionTime, 'dd MMMM yyyy');
                    }
                    else {
                        return '';
                    }
                }
            },
            {
                title: this.l("Common.Actions"), data: 'id', targets: 11, width: '150px', orderable: false,
                render: (data, type, full) => {
                    if (this.isDeleted == false) {
                        let rowDiv = '<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-info edit" title=""><i class="fa fa-edit"></i></a>' ;
                        if (data != this.appSession.userId && data != 2)
                            rowDiv += '<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-danger delete confirm" data-action="Delete" data-id="' + data + '" title="' + this.l("Common.Delete") + '"><i class="fa fa-trash"></i></a>' ;
                        return rowDiv;
                    }
                    else {
                        return  '<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-danger delete confirm" data-action="Restore" data-id="' + data + '" title="' + this.l("Common.Restore") + '"><i class="fa fa-undo"></i></a>' ;
                    }
                }
            }
            /*
            {
                title: this.l("Common.Actions"), data: 'id', targets: 11, width: '150px', orderable: false,
                render: (data, type, full) => {
                    if (this.isDeleted == false) {
                        let rowDiv = this.isGranted("Write.Permission.Admins") ? '<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-info edit" title=""><i class="fa fa-edit"></i></a>' : '';
                        if (data != this.appSession.userId && data != 2)
                            rowDiv += this.isGranted("Delete.Permission.Admins") ? '<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-danger delete confirm" data-action="Delete" data-id="' + data + '" title="' + this.l("Common.Delete") + '"><i class="fa fa-trash"></i></a>' : '';
                        return rowDiv;
                    }
                    else {
                        return this.isGranted("Delete.Permission.Admins") ? '<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-danger delete confirm" data-action="Restore" data-id="' + data + '" title="' + this.l("Common.Restore") + '"><i class="fa fa-undo"></i></a>' : '';
                    }
                }
            }
            */
            
        ];
        //initDataTableInput.InitialParams=[];
        //this.setAjaxParam("Name","");
        //this.refreshAjaxParams();
        initDataTableInput.DataTableSrc = "/api/services/app/User/IsPaged";
        this.columnDefs = super.initDataTable(initDataTableInput);

    }

    protected edit?(obj: UserDto): void {
          
        this.selectedObj = obj;
        this.openModal();
    }

    addNew() {
        this.selectedObj = new UserDto();
        this.openModal();
    }

    openModal() {
        this.dialogService.open(DialogManageUsersComponent, {
            context: {
                userId: this.selectedObj.id,
                selectedObj: this.selectedObj
            },
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
    searchName(name) {
        debugger;
        //if (this.request != null)
        if (name != '' || name != null) {
            
            this.addAjaxParam("Name", name);
        }
        else {
            this.clearAjaxParams();
        }
        this.rerender();
    }

    clearSearch() {
        debugger;
        this.searchValue = '';
        this.request.keyword = '';
        this.clearAjaxParams();
        this.rerender();
    }
}