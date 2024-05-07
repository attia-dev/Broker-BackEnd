import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { NotFoundComponent } from '../miscellaneous/not-found/not-found.component';
import { UserComponent } from './users/users.component';
import { DialogManageUsersComponent } from './users/dialog-manage-Users.component';
import { PermissionUserComponent } from './permission-user/permission-user.component';

const routes: Routes = [
    {
        path: '',
        children: [


            {
                path: 'users',
                component: UserComponent,
                data: {
                    title: "Pages.Admins.Title",
                    breadcrumb: "Pages.Admins.Title",
                    // permission: 'Read.Permission.Admins'
                },
                children: [

                ]
            },

            {
                path: 'permissionUser/:id',
                component: PermissionUserComponent,
                data: {
                    title: "Pages.User.Permission",
                    breadcrumb: "Pages.User.Permission",
                }
            },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class UsersRoutingModule {
}
