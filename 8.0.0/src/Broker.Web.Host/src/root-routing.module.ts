import { NgModule } from '@angular/core';
import { Routes, RouterModule, ExtraOptions } from '@angular/router';

const routes: Routes = [
    { path: '', redirectTo: '/admin/pages/dashboard', pathMatch: 'full' },
   // { path: '', redirectTo: '/app/about', pathMatch: 'full' },
    {
        path: 'account',
        loadChildren: () => import('account/account.module').then(m => m.AccountModule), // Lazy load account module
        data: { preload: true }
    },
    {
        path: 'admin',
        loadChildren: () => import('appNew/app.module').then(m => m.AppModule), // Lazy load account module
        data: { preload: true }
    },
 //   {
 //       path: 'app',
 //       loadChildren: () => import('app/app.module').then(m => m.AppModule), // Lazy load account module
 //       data: { preload: true }
 //   }
];
const config: ExtraOptions = {
    useHash: true, relativeLinkResolution: 'legacy'
};
@NgModule({
    imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy', useHash: true })],
   // imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
    providers: []
})
export class RootRoutingModule { }
