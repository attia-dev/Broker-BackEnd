import { Component, Injector, OnInit } from '@angular/core';
import { DataTableComponentBase, InitDataTableInput } from '@shared/helpers/datatable-component-base';
import { CountryDto, DefinitionDto, DurationDto } from '@shared/service-proxies/service-proxies';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { finalize } from 'rxjs/operators';
import { NbDialogService } from '@nebular/theme';
import { DialogManageDurationComponent } from './dialog-manage-duration.component';

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
    selector: 'app-admin-durations',
    templateUrl: './durations.component.html',
    styleUrls: ['./durations.component.scss']
})
export class AdminDurationsComponent extends DataTableComponentBase {
    protected rejectResonBalance?(obj: object): void {
        throw new Error('Method not implemented.');
    }

    public closeResult: string;
    columnDefs: any[] = [];
    searchValue: any;
    selectedObj: DurationDto = new DurationDto();
    /* public enum BuildingType
    {
        Apartment = 1,
        Villa = 2,
        ChaletForSummer = 3,
        Building = 4,
        AdminOffice = 5,
        Shop = 6,
        Land = 7,
    }*/
    buildingType: any[] = [
         this.l('Common.Apartment') ,
         this.l('Common.Villa'),
         this.l('Common.ChaletForSummer') ,
         this.l('Common.Building') ,
         this.l('Common.AdminOffice') ,
         this.l('Common.Shop') ,
         this.l('Common.Land') ,
    ];
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
                title: this.l('Common.Period'), searchable: true, name: "period", data: 'period', targets: 1, orderable: true,
                render: (data, type, full) => {
                    return data || '';
                }
            },
            {
                title: this.l('Common.Amount'), searchable: true, name: "amount", data: 'amount', targets: 2, orderable: true,
                render: (data, type, full) => {
                    return data || '';
                }
            },
            {
                title: this.l('Common.BuildingType'), searchable: true, name: "DurationBuildingTypes", data: 'DurationBuildingTypes', targets: 3, orderable: true,
                render: (data, type, full) => {
                    
                    let types: String[] = [];
                    full.durationBuildingTypes.forEach(
                        x => types.push(this.buildingType[x.type-1])
                    );
                    return types?.join(' , ') || '';
                }
            },
            {
                title: this.l('Common.IsPublish'), searchable: true, name: "isPublish", data: 'isPublish', targets: 4, orderable: true,
                render: (data, type, full) => {
                    return data;
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
                targets: 12, data: 'id', title: this.l('Common.Actions'), visible: true, orderable: false,
                render: (data, type, full) => {
                    if (this.isDeleted == false) {
                        let rowDiv = this.isGranted("Write.Permission.Durations") ?'<button type="button" class="btn btn-round btn-primary buttons-html5 edit"><i class="fa fa-edit"></i></button>' : "";
                        rowDiv += this.isGranted("Delete.Permission.Durations") ?'<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-danger delete confirm" data-action="Delete" data-id="' + data + '" title="' + this.l("Common.Delete") + '"><i class="fa fa-trash"></i></a>' : "";
                        return rowDiv;
                    }
                    else {
                        return this.isGranted("Delete.Permission.Durations") ? '<a href="javascript:void(0);" class="btn btn-icon btn-sm mx-3 btn-danger delete confirm" data-action="Restore" data-id="' + data + '" title="' + this.l("Common.Restore") + '"><i class="fa fa-undo"></i></a>' : "";
                    }
                }
            }
        ];

        initDataTableInput.DataTableSrc = "/api/services/app/Duration/IsPaged";
        this.columnDefs = super.initDataTable(initDataTableInput);

    }

    protected edit?(obj: DurationDto): void {
        this.selectedObj = obj;
        this.openModal();
    }

    addNew() {
        this.selectedObj = new DurationDto();
        this.openModal();
    }
    

    openModal() {
        let durationTypes: number[]=[];
        this.selectedObj?.durationBuildingTypes?.forEach(
            x =>
                durationTypes.push(x.type)
        );
        this.dialogService.open(DialogManageDurationComponent, {
            context: {
                durationId: this.selectedObj.id,
                durationPeriod: this.selectedObj.period,
                durationAmount: this.selectedObj.amount,
                durationType: durationTypes,
                durationIsPublish: this.selectedObj.isPublish,
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
                return "No Status";
        }
    }
}
