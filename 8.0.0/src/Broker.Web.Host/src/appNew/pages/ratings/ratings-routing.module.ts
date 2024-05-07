import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { NotFoundComponent } from '../miscellaneous/not-found/not-found.component';
import { RatingsComponent } from './ratings/ratings.component';

const routes: Routes = [
    {
        path: '',
        children: [


            {
                path: 'ratings',
                component: RatingsComponent,
                data: {
                    title: "Pages.Ratings.Title",
                    breadcrumb: "Pages.Ratings.Title",
                    // permission: 'Read.Permission.Admins'
                },
                children: [

                ]
            },
        ]
            
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class RatingsRoutingModule {
}
