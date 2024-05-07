import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { NotFoundComponent } from '../miscellaneous/not-found/not-found.component';
import { ProjectsComponent } from './projects/projects.component';
import { AdminProjectDetailsComponent } from './ProjectDetails/projectDetails.component';

const routes: Routes = [
    {
        path: '',
        children: [
//ProjectsComponent,
//AdminProjectDetailsComponent,

            {
                path: 'projects',
                component: ProjectsComponent,
                data: {
                    title: "Pages.Projects.Title",
                    breadcrumb: "Pages.Projects.Title",
                    // permission: 'Read.Permission.Admins'
                },
                children: [

                ]
            },
            {
                path: 'projectDetails/:id',
                component: AdminProjectDetailsComponent,
            },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProjectsRoutingModule {
}
