import { Component, Injector, OnInit } from '@angular/core';
import { DataTableComponentBase, InitDataTableInput } from '@shared/helpers/datatable-component-base';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { finalize } from 'rxjs/operators';
import { NbDialogService } from '@nebular/theme';
import { AppComponentBase } from '../../../../shared/app-component-base';
import { AppConsts } from '../../../../shared/AppConsts';
import { ActivatedRoute,Router } from '@angular/router';
import { Location } from '@angular/common';
import { ProjectDto, ProjectServiceProxy } from '@shared/service-proxies/service-proxies';

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
    selector: 'app-admin-projectDetails',
    templateUrl: './projectDetails.component.html',
    styleUrls: ['./projectDetails.component.scss']
})
export class AdminProjectDetailsComponent extends AppComponentBase implements OnInit {
    public closeResult: string;
    columnDefs: any[] = [];
    selectedObj:ProjectDto = new ProjectDto();
    projectId: number;
    baseUrl: string;
    constructor(injector: Injector, private dialogService: NbDialogService, private location: Location,
        private activatedRoute: ActivatedRoute,
        private _projectSerive: ProjectServiceProxy,private _router:Router
    )
    {
        super(injector);
        this.baseUrl = AppConsts.remoteServiceBaseUrl + '/Resources/UploadFiles/';
    }

    ngOnInit() {

        this.projectId = parseInt(this.activatedRoute.snapshot.paramMap.get("id"));
        this.getProjectById();

    }

    getProjectById() {
        debugger;
        this._projectSerive.getById(this.projectId).pipe(
            finalize(() => {

            })
        ).subscribe((result: ProjectDto) => {
            
            this.selectedObj = result;
            console.log(this.selectedObj);
        });
    }

   navigateToAdvertise(id)
   {
    this._router.navigate(['admin/pages/advertisements/advertisementDetails',id]);
   }

    Cancel() {
        this.location.back();
    }

    list(pageNumber: number, finishedCallback: Function): void {

    }
    
}