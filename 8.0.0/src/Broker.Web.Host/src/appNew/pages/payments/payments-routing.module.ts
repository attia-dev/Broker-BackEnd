import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { NotFoundComponent } from '../miscellaneous/not-found/not-found.component';

import { AdminWalletsComponent } from './wallets/wallets.component';




const routes: Routes = [
    
    
    {
        path: 'wallets/:id',
        component: AdminWalletsComponent,
    },
    {
        path: '**',
        component: NotFoundComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PaymentsRoutingModule {
} 
