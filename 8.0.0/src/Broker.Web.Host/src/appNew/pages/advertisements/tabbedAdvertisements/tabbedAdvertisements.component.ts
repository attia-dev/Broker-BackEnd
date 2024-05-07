import { Component, Injector, OnInit } from '@angular/core';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { DataTableComponentBase, InitDataTableInput } from '@shared/helpers/datatable-component-base';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { ENGINE_METHOD_ALL } from 'constants';
import { AppComponentBase } from '@shared/app-component-base';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
class PagedCategoriesRequestDto extends PagedRequestDto {
    keyword: string;
}

@Component({
    selector: 'app-tabbedAdss',
    templateUrl: './tabbedAdvertisements.component.html'
})
export class AdminTabbedAdvertisementsComponent extends AppComponentBase implements OnInit {

    tabcounts: number;
    navLinks: any[] = [];

    constructor( injector: Injector, private fb: FormBuilder, private modalService: NgbModal, 
         private route: ActivatedRoute, private router: Router) {
        super(injector);
        this.getTab();
        debugger;
        this.router.events.subscribe((event) => {
            if (event instanceof NavigationEnd) {
                this.navLinks.filter(item => {
                    debugger;
                    if (item.location === event.url) {
                        this.navLinks.filter(menuItem => {
                            debugger;
                            if (menuItem != item)
                                menuItem.active = false;
                            else
                                menuItem.active = true;
                        })
                    }
                })
            }
        })
        this.router.navigate(['/admin/pages/advertisements/tabbedAds/advertisements']);
    }
   
    ngOnInit() {
    }

    getTab() {
        this.navLinks = [
            { index: 0, location: '/admin/pages/advertisements/tabbedAds/advertisements', label: this.l('Pages.Advertisements.Title'), active: true },
            { index: 1, location: '/admin/pages/advertisements/tabbedAds/requests', label: this.l('Pages.Requests.Title'), active: false },
        ];
    }
    
    public getRouterOutletState(outlet) {
        return outlet.isActivated ? outlet.activatedRoute : '';
    }

    getactive(link) {
        debugger;
        this.navLinks.forEach(obj => {
            if (obj != link)
            obj.active = false;
        });
        link.active = true
        this.router.navigate([link.location]);
    }

}