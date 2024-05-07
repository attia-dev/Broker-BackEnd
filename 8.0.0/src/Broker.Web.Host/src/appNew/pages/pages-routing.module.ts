import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { PagesComponent } from './pages.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ECommerceComponent } from './e-commerce/e-commerce.component';
import { NotFoundComponent } from './miscellaneous/not-found/not-found.component';


const routes: Routes = [{
    path: '',
    component: PagesComponent,
    children: [
        {
            path: 'ecommerce',
            component: ECommerceComponent,
        },
        {
            path: 'dashboard',
            component: DashboardComponent,
        },
        {
            path: 'advertisements',
            loadChildren: () => import('./advertisements/advertisements.module')
                .then(m => m.AdvertisementsModule),
        },
        {
            path: 'projects',
            loadChildren: () => import('./projects/projects.module')
                .then(m => m.ProjectsModule),
        },
        {
            path: 'ratings',
            loadChildren: () => import('./ratings/ratings.module')
                .then(m => m.RatingsModule),
        },
    {
        path: 'lookups',
        loadChildren: () => import('./lookups/lookups.module')
            .then(m => m.AdminLookupsModule),
    },
   
    
   {
        path: 'users',
       loadChildren: () => import('./users/users.module')
            .then(m => m.UsersModule),
   },


        {
            path: 'customers',
            loadChildren: () => import('./customers/customers.module')
                .then(m => m.AdminCustomersModule),
        },
        {
            path: 'payments',
            loadChildren: () => import('./payments/payments.module')
                .then(m => m.AdminPaymentsModule),
        },
        //   {
        //       path: 'users',
        //       loadChildren: () => import('./users/users.module')
        //           .then(m => m.UsersModule),
        //   },
        //   {
        //       path: 'employees',
        //       loadChildren: () => import('./employees/employees.module')
        //           .then(m => m.EmployeesModule),
        //   },
        //   {
        //       path: 'requests',
        //       loadChildren: () => import('./requests/requests.module')
        //           .then(m => m.RequestsModule),
        //   },
        {
            path: 'layout',
            loadChildren: () => import('./layout/layout.module')
                .then(m => m.LayoutModule),
        },
        {
            path: 'forms',
            loadChildren: () => import('./forms/forms.module')
                .then(m => m.FormsModule),
        },
        {
            path: 'ui-features',
            loadChildren: () => import('./ui-features/ui-features.module')
                .then(m => m.UiFeaturesModule),
        },
        {
            path: 'modal-overlays',
            loadChildren: () => import('./modal-overlays/modal-overlays.module')
                .then(m => m.ModalOverlaysModule),
        },
        {
            path: 'extra-components',
            loadChildren: () => import('./extra-components/extra-components.module')
                .then(m => m.ExtraComponentsModule),
        },

        {
            path: 'charts',
            loadChildren: () => import('./charts/charts.module')
                .then(m => m.ChartsModule),
        },
        //{
        //    path: 'editors',
        //    loadChildren: () => import('./editors/editors.module')
        //        .then(m => m.EditorsModule),
        //},

        {
            path: 'miscellaneous',
            loadChildren: () => import('./miscellaneous/miscellaneous.module')
                .then(m => m.MiscellaneousModule),
        },
        {
            path: '',
            redirectTo: 'dashboard',
            pathMatch: 'full',
        },
        {
            path: '**',
            component: NotFoundComponent,
        },
    ],
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PagesRoutingModule {
}