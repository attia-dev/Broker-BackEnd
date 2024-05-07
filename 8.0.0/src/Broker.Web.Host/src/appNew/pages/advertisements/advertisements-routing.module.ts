import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { NotFoundComponent } from '../miscellaneous/not-found/not-found.component';
import { AdvertisementsComponent } from './advertisements/advertisements.component';
import { AdminAdvertiseDetailsComponent } from '../advertisements/AdvertiseDetails/advertiseDetails.component';
import { AdminTabbedAdvertisementsComponent } from '../advertisements/tabbedAdvertisements/tabbedAdvertisements.component';
import { AdvertisementsRequestsComponent } from '../advertisements/advertisementsRequests/advertisementsRequests.component';

const routes: Routes = [
    {
        path: '',
        children: [

            
           /* {
                path: 'tabbedAds',
                component: AdminTabbedAdvertisementsComponent,
                data: {
                    title: "Pages.Advertisements.Title",
                    breadcrumb: "Pages.Advertisements.Title",
                    // permission: 'Read.Permission.Admins'
                },
                children: [

                ]
            },*/
            ///fdfdf
            {
                path: 'tabbedAds',
                component: AdminTabbedAdvertisementsComponent,
                data: {
                    title: "Pages.StoreInvoices.Title",
                    breadcrumb: "Pages.StoreInvoices.Title",
                    //permission: 'Read.Permission.Settings'
                },
                children: [
                    
                    {
                        path: 'advertisements',
                        component: AdvertisementsComponent,
                        data: {
                            title: "Pages.Advertisements.Title",
                            breadcrumb: "Pages.Advertisements.Title",
                            // permission: 'Read.Permission.Admins'
                        },
                        children: [
        
                        ]
                    },
                    {
                        path: 'requests',
                        component: AdvertisementsRequestsComponent,
                        data: {
                            title: "Pages.AdvertisementsRequests.Title",
                            breadcrumb: "Pages.AdvertisementsRequests.Title",
                            // permission: 'Read.Permission.Admins'
                        },
                        children: [
        
                        ]
                    },
                 
                ]

            },
            ///fdfdf
           
            {
                path: 'advertisementDetails/:id',
                component: AdminAdvertiseDetailsComponent,
            },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class AdvertisementsRoutingModule {
}
