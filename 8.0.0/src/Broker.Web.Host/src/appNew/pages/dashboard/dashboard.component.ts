import { Component, Injector, OnDestroy, OnInit } from '@angular/core';
import { CommonServiceProxy, CountDto } from '@shared/service-proxies/service-proxies';
import { finalize, takeWhile } from 'rxjs/operators';
//import { TaskDto, CountDto, CommonServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../shared/app-component-base';


interface CardSettings {
    title: string;
    icon: string;
    type: string;
    link: string;
    count: number;
}

@Component({
    selector: 'ngx-dashboard',
    styleUrls: ['./dashboard.component.scss'],
    templateUrl: './dashboard.component.html',
})
export class DashboardComponent extends AppComponentBase implements OnInit {

    countCards: CardSettings[] = [
        {
            title: this.l('Pages.Seekers.Title'), 
            link: this.isGranted("Read.Permission.Seekers") ? '/admin/pages/customers/seekers' : '',
           //link:  '/admin/pages/customers/seekers',
            
           icon: 'nb-person',
           type: 'warning',
            count: 0
        },

        {
            title: this.l('Pages.Owners.Title'), 
            link: this.isGranted("Read.Permission.Owners") ? '/admin/pages/customers/owners' : '',
           //link:  '/admin/pages/customers/owners',
            
           icon: 'nb-person',
           type: 'warning',
            count: 0
        },

        {
            title: this.l('Pages.Companies.Title'), 
            link: this.isGranted("Read.Permission.Companies") ? '/admin/pages/customers/companies' : '',
           //link:  '/admin/pages/customers/companies',
            
           icon: 'nb-person',
           type: 'success',
            count: 0
        },

        {
            title: this.l('Pages.Sponsors.Title'), 
            link: this.isGranted("Read.Permission.Companies") ? '/admin/pages/customers/companies' : '',
           //link:  '/admin/pages/customers/companies',
            
           icon: 'nb-person',
           type: 'warning',
            count: 0
        },

        {
            title: this.l('Pages.BrokerPersons.Title'), 
            link: this.isGranted("Read.Permission.Brokers") ? '/admin/pages/customers/brokerPersons' : '',
           //link:  '/admin/pages/customers/brokerPersons',
            
           icon: 'nb-person',
           type: 'warning',
            count: 0
        },
    

    ];

    constructor(injector: Injector,private _commonServiceProxy: CommonServiceProxy)
    {
        super(injector);
    }

    ngOnInit(): void {
        this.getCount()
    }

    getCount() {
       this._commonServiceProxy.getCount().pipe(
            finalize(() => {

            })
        ).subscribe((result:CountDto) => {
            this.countCards[0].count = result.seekerCount;
           this.countCards[1].count = result.ownerCount;
            this.countCards[2].count = result.companyCount;
           this.countCards[3].count = result.sponsorCount;
            this.countCards[4].count = result.brokerCount;
        });
    }
    ngOnDestroy() {
       
    }
}
