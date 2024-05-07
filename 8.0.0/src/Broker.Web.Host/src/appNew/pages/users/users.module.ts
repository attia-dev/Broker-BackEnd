import { NgModule } from '@angular/core';
import { ThemeModule } from '../../@theme/theme.module';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { TabsModule } from 'ngx-bootstrap/tabs';
import {
    NbButtonModule,
    NbCardModule,
    NbRadioModule,
    NbCheckboxModule,
    NbDialogModule,
    NbInputModule,
    NbPopoverModule,
    NbSelectModule,
    NbIconModule,
    NbTooltipModule,
    NbWindowModule,
    NbMenuModule
} from '@nebular/theme';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { DataTablesModule } from 'angular-datatables';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { UsersRoutingModule } from './users-routing.module';
import { UserComponent } from './users/users.component';
import { DialogManageUsersComponent } from './users/dialog-manage-Users.component';
import { PermissionUserComponent } from './permission-user/permission-user.component';

@NgModule({
    imports: [
        UsersRoutingModule,
        ThemeModule,
        NbMenuModule,
        NbRadioModule,
        NbDialogModule.forChild(),
        NbCardModule,
        NbCheckboxModule,
        NbPopoverModule,
        NbButtonModule,
        NbInputModule,
        NbSelectModule,
        NbTooltipModule,
        NbIconModule,
        DataTablesModule,
        FormsModule,
        ReactiveFormsModule,
        NgMultiSelectDropDownModule.forRoot(),
        CommonModule,
        HttpClientModule,
        TabsModule,
        
    ],
    declarations: [
        UserComponent,
        DialogManageUsersComponent,
        PermissionUserComponent
    ],
    entryComponents: [ 
     
    ],
})
export class UsersModule {
}
