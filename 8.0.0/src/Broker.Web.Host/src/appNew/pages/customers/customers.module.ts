import { NgModule } from '@angular/core';
import { ThemeModule } from '../../@theme/theme.module';
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
    NbMenuModule,
    NbActionsModule,
    NbLayoutModule,
    NbSidebarModule,
} from '@nebular/theme';
import { DataTablesModule } from 'angular-datatables';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CustomersRoutingModule } from './customers-routing.module';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { AgmCoreModule } from '@agm/core';
import { AppConsts } from '@shared/AppConsts';
import { NgxPaginationModule } from 'ngx-pagination';
import { AdminBrokerPersonsComponent } from './brokerPersons/brokerPersons.component';
import { AdminOwnersComponent } from './owners/owners.component';
import { AdminSeekersComponent } from './seekers/seekers.component';
import { AdminCompaniesComponent } from './companies/companies.component';




@NgModule({
    imports: [
        NgxPaginationModule,
        CustomersRoutingModule,
        ThemeModule,
        NbMenuModule,
        NbDialogModule.forChild(),
        NbCardModule,
        NbCheckboxModule,
        NbRadioModule,
        NbPopoverModule,
        NbButtonModule,
        NbInputModule,
        NbSelectModule,
        NbTooltipModule,
        NbIconModule,
        DataTablesModule,
        FormsModule,
        ReactiveFormsModule,
        NbActionsModule,
        NbLayoutModule,
        NbSidebarModule,
        NbWindowModule,
        NgMultiSelectDropDownModule.forRoot(),

    ],
    declarations: [
        AdminBrokerPersonsComponent,
        AdminOwnersComponent,
        AdminSeekersComponent,
        AdminCompaniesComponent,
        
        
        
    ],
    entryComponents: [
        

    ],
})
export class AdminCustomersModule {
}

