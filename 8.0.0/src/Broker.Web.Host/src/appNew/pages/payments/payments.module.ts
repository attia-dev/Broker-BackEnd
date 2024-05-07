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
import { PaymentsRoutingModule } from './payments-routing.module';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { AgmCoreModule } from '@agm/core';
import { AppConsts } from '@shared/AppConsts';
import { NgxPaginationModule } from 'ngx-pagination';
import { AdminWalletsComponent } from './wallets/wallets.component';
import { DialogManageWalletComponent } from './wallets/dialog-manage-wallet.component';



@NgModule({
    imports: [
        NgxPaginationModule,
        PaymentsRoutingModule,
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
        AdminWalletsComponent,
        DialogManageWalletComponent
        
    ],
    entryComponents: [

        DialogManageWalletComponent
    ],
})
export class AdminPaymentsModule {
}
