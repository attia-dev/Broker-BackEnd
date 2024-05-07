import { Component, Injector, OnInit } from '@angular/core';
import { DataTableComponentBase, InitDataTableInput } from '@shared/helpers/datatable-component-base';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { finalize } from 'rxjs/operators';
import { NbDialogService } from '@nebular/theme';
import { AppComponentBase } from '../../../../shared/app-component-base';
import { AppConsts } from '../../../../shared/AppConsts';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { AdvertisementDto, AdvertisementServiceProxy } from '@shared/service-proxies/service-proxies';

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
    selector: 'app-admin-advertiseDetails',
    templateUrl: './advertiseDetails.component.html',
    styleUrls: ['./advertiseDetails.component.scss']
})
export class AdminAdvertiseDetailsComponent extends AppComponentBase implements OnInit {
    public closeResult: string;
    columnDefs: any[] = [];
    selectedObj:AdvertisementDto = new AdvertisementDto();
    advertisementId: number;
    baseUrl: string;
    constructor(injector: Injector, private dialogService: NbDialogService, private location: Location,
        private activatedRoute: ActivatedRoute,
        private _advertiseSerive: AdvertisementServiceProxy
    )
    {
        super(injector);
        this.baseUrl = AppConsts.remoteServiceBaseUrl + '/Resources/UploadFiles/';
    }

    ngOnInit() {

        this.advertisementId = parseInt(this.activatedRoute.snapshot.paramMap.get("id"));
        this.getAdvertisementById();

    }

    getAdvertisementById() {
        debugger;
        this._advertiseSerive.getById(this.advertisementId).pipe(
            finalize(() => {

            })
        ).subscribe((result: AdvertisementDto) => {
            
            this.selectedObj = result;
            console.log(this.selectedObj);
        });
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
    getChalletType(status) {
        switch (status) {
            case 1:
                return this.l('Common.Chalet');
            case 2:
                return this.l('Common.Villa');
            default:
                return "";
        }
    }
    getAgreementStatus(status) {
        switch (status) {
            case 1:
                return this.l('Common.Sell');
            case 2:
                return this.l('Common.Rent');
            default:
                return "";
        }
    }
    getBuildingStatus(status) {
        switch (status) {
            case 1:
                return this.l('Common.New');
            case 2:
                return this.l('Common.Used');
            case 2:
                return this.l('Common.Renewed');
            default:
                return "";
        }
    }

    getLandingStatus(status) {
        switch (status) {
            case 1:
               return this.l('Common.Empty');
            case 2:
                return this.l('Common.Used');
            default:
                return ""; 
        }
    }
    getUsingFor(status) {
        switch (status) {
            case 1:
                return this.l('Common.Buildings');
            case 2:
                 return this.l('Common.Industrial');
            case 3:
               return this.l('Common.Agriculture');
            case 4:
                return this.l('Common.Investment');
            case 5:
                 return this.l('Common.Residential');
            case 6:
                return this.l('Common.Commercial');
            default:
                return ""; 
        }
    }
    getPaymentFacilitiesType(status) {
        switch (status) {
            case 1:
                 return this.l('Common.Allowed');
            case 2:
                 return this.l('Common.NotAllowed');
            default:
                return "";
        }
    }
    getMrMrsType(status) {
        switch (status) {
            case 1:
                return this.l('Common.Mr');
            case 2:
                return this.l('Common.Mrs');
            default:
                return "";
        }
    }
    getAdvertiseMakerType(status) {
        switch (status) {
            case 1:
                 return this.l('Common.TheOwner');
            case 2:
                 return this.l('Common.TheBroker');
            default:
                return "";
        }
    }
    getProximityToTheSeaType(status) {
        switch (status) {
            case 1:
                return this.l('Common.M100');
            case 2:
                return this.l('Common.M500');
            case 3:
                return this.l('Common.M500To1KM');
            case 4:
                return this.l('Common.km1To5KM');
            default:
                return "";
        }
    }
    getOfficiesType(status) {
        switch (status) {
            case 1:
                return this.l('Common.Company');
            case 2:
                return this.l('Common.Factory');
            default:
                return "";
        }
    }
    getRentType(status) {
        switch (status) {
            case 1:
                return this.l('Common.Monthly');
            case 2:
                return this.l('Common.MidTerm');
            case 3:
                return this.l('Common.Annual');
            default:
                return "";
        }
    }
    getDecorationStatus(status) {
        switch (status) {
            case 1:
                return this.l('Common.Without');
            case 2:
                return this.l('Common.SemiFinished');
            case 3:
                return this.l('Common.Full');
            case 4:
                return this.l('Common.HighQuality');
            default:
                return "";
        }
    }
        getDocumentStatus(status) {
            switch (status) {
                case 1:
                    return this.l('Common.Registered');
                case 2:
                    return this.l('Common.Unregistered');
                case 3:
                    return this.l('Common.Registerable');
                case 4:
                    return this.l('Common.Unregisterable');
                default:
                    return "";
            }
    }

    getChaletRentType(status) {
        switch (status) {
            case 1:
                return this.l('Common.Day');
            case 2:
                return this.l('Common.Week');
            default:
                return "";
        }
    }

    getMinTimeToBookType(status) {
        switch (status) {
            case 1:
                return this.l('Common.Day1');
            case 2:
                return this.l('Common.Days2');
            case 3:
                return this.l('Common.Days3');
            case 4:
                return this.l('Common.Week1');
            case 5:
                return this.l('Common.Days10');
            default:
                return "";
        }
    }

    Cancel() {
        this.location.back();
    }

    list(pageNumber: number, finishedCallback: Function): void {

    }
    
}