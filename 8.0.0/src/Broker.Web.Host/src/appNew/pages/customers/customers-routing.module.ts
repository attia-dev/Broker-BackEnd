import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { NotFoundComponent } from '../miscellaneous/not-found/not-found.component';
import { AdminBrokerPersonsComponent } from '../customers/brokerPersons/brokerPersons.component';
import { AdminOwnersComponent } from '../customers/owners/owners.component';
import { AdminSeekersComponent } from '../customers/seekers/seekers.component';
import { AdminCompaniesComponent } from '../customers/companies/companies.component';
import { AdminWalletsComponent } from '../payments/wallets/wallets.component';



const routes: Routes = [
    {
        path: 'brokerPersons',
        component: AdminBrokerPersonsComponent,
    },
    {
        path: 'owners',
        component: AdminOwnersComponent,
    },
    {
        path: 'seekers',
        component: AdminSeekersComponent,
    },
    {
        path: 'companies',
        component: AdminCompaniesComponent,
    },
    //{
    //    path: 'wallets/:id',
    //    component: AdminWalletsComponent,
    //},
    

    {
        path: '**',
        component: NotFoundComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CustomersRoutingModule {
} 
