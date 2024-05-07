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
import { ProjectsRoutingModule } from './projects-routing.module';
import { ProjectsComponent } from './projects/projects.component';
import { AdminProjectDetailsComponent } from './ProjectDetails/projectDetails.component';
import { DialogManageProjectRejectReasonComponent } from './projects/dialog-manage-projectRejectReason.component'

@NgModule({
    imports: [
        ProjectsRoutingModule,
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
        ProjectsComponent,
        AdminProjectDetailsComponent,
        DialogManageProjectRejectReasonComponent,
    ],
    entryComponents: [ 
     
    ],
})
export class ProjectsModule {
}
