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
import { AdvertisementsRoutingModule } from './advertisements-routing.module';
import { AdvertisementsComponent } from './advertisements/advertisements.component';
import { AdminAdvertiseDetailsComponent } from '../advertisements/AdvertiseDetails/advertiseDetails.component';
import { AdminTabbedAdvertisementsComponent } from '../advertisements/tabbedAdvertisements/tabbedAdvertisements.component';
import { DialogManageAdvertisementRejectReasonComponent } from '../advertisements/advertisements/dialog-manage-advertisementRejectReason.component'
import { DialogManageAdvertisementRequestRejectReasonComponent } from '../advertisements/advertisementsRequests/dialog-manage-advertisementRequestRejectReason.component'
import { AdvertisementsRequestsComponent } from '../advertisements/advertisementsRequests/advertisementsRequests.component';

@NgModule({
    imports: [
        AdvertisementsRoutingModule,
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
    exports: [AdvertisementsComponent,AdvertisementsRequestsComponent],
    declarations: [
        AdvertisementsComponent,
        AdminAdvertiseDetailsComponent,
        AdminTabbedAdvertisementsComponent,
        AdvertisementsRequestsComponent,
        DialogManageAdvertisementRejectReasonComponent,
        DialogManageAdvertisementRequestRejectReasonComponent,
    ],
    entryComponents: [ 
        AdvertisementsComponent
    ],
})
export class AdvertisementsModule {
}
