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
import { LookupsRoutingModule } from './lookups-routing.module';
import { AdminGovernoratesComponent } from './governorates/governorates.component';
import { DialogManageGovernorateComponent } from './governorates/dialog-manage-governorate.component';
import { DialogManageCountryComponent } from './countries/dialog-manage-country.component';
import { AdminCitiesComponent } from './cities/cities.component';
import { DialogManageCityComponent } from './cities/dialog-manage-city.component';
import { AdminCountriesComponent } from './countries/countries.component';
import { AdminPackagesComponent } from './packages/packages.component';
import { DialogManagePackageComponent } from './packages/dialog-manage-package.component';
import { DialogManageFacilityComponent } from './facilities/dialog-manage-facility.component';
import { AdminFacilitiesComponent } from './facilities/facilities.component';
import { DialogManageDocumentComponent } from './documents/dialog-manage-document.component';
import { AdminDocumentsComponent } from './documents/documents.component';
import { DialogManageDecorationComponent } from './decorations/dialog-manage-decoration.component';
import { AdminDecorationsComponent } from './decorations/decorations.component';
import { AdminDurationsComponent } from './durations/durations.component';
import { DialogManageDurationComponent } from './durations/dialog-manage-duration.component';
import { AdminDiscountCodesComponent } from './discountCodes/discountCodes.component';
import { DialogManageDiscountCodeComponent } from './discountCodes/dialog-manage-discountCode.component';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { AgmCoreModule } from '@agm/core';
import { AppConsts } from '@shared/AppConsts';
import { NgxPaginationModule } from 'ngx-pagination';
import { ManageAboutComponent } from './about/about.component';
import { ManageContactComponent } from './contact/contact.component';
import { ManagePointsComponent } from './points/points.component';
import { ManageFeatureAdsComponent } from './featureAds/featureAds.component';
import { DialogManagePropertyComponent } from './properties/dialog-manage-property.component';
import { AdminPropertiesComponent } from './properties/properties.component';
import { AdminContactUsesComponent } from './contactUs/contactUses.component';
import { DialogManageContactUsComponent } from './contactUs/dialog-manage-contactUs.component';
import { AdminSocialContactComponent } from './socialContacts/socialContacts.component';
import { DialogManageSocialContactComponent } from './socialContacts/dialog-manage-socialContact.component';

import { DialogManageMinTimeToBookForChaletComponent } from './minTimeToBookForChalets/dialog-manage-minTimeToBookForChalet.component';
import { AdminMinTimeToBookForChaletsComponent } from './minTimeToBookForChalets/minTimeToBookForChalets.component';

    
@NgModule({
    imports: [
        NgxPaginationModule,
        LookupsRoutingModule,
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
        AgmCoreModule.forRoot({
            apiKey: AppConsts.GoogleMapApiKey,
            libraries: ['places']
        }),
        NbWindowModule,
        NgMultiSelectDropDownModule.forRoot(),

    ],
    declarations: [
        AdminCitiesComponent,
        DialogManageCityComponent,
        AdminCountriesComponent,
        DialogManageCountryComponent,
        AdminGovernoratesComponent,
        DialogManageGovernorateComponent,
        DialogManageFacilityComponent,
        AdminFacilitiesComponent,
        AdminDocumentsComponent,
        DialogManageDocumentComponent,
        AdminDecorationsComponent,
        DialogManageDecorationComponent,
        AdminPackagesComponent,
        DialogManagePackageComponent,
        ManagePointsComponent,
        ManageFeatureAdsComponent,
        AdminDurationsComponent,
        DialogManageDurationComponent,
        AdminDiscountCodesComponent,
        DialogManageDiscountCodeComponent,
        ManageAboutComponent,
        ManageContactComponent,
        AdminPropertiesComponent,
        DialogManagePropertyComponent,
        AdminContactUsesComponent,
        DialogManageContactUsComponent,
        AdminSocialContactComponent,
        DialogManageSocialContactComponent,
        AdminMinTimeToBookForChaletsComponent,
        DialogManageMinTimeToBookForChaletComponent,
    ],
    entryComponents: [
        DialogManageCountryComponent,
        DialogManageGovernorateComponent,
        DialogManageFacilityComponent,
        DialogManageDocumentComponent,
        DialogManageDecorationComponent,
        DialogManagePackageComponent,
        DialogManageDurationComponent,
        DialogManageDiscountCodeComponent,
        DialogManageContactUsComponent,
        DialogManageSocialContactComponent,
        DialogManageMinTimeToBookForChaletComponent
    ],
})
export class AdminLookupsModule {
}
