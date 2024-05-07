import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { NotFoundComponent } from '../miscellaneous/not-found/not-found.component';
import { AdminCountriesComponent } from '../lookups/countries/countries.component';
import { AdminPackagesComponent } from '../lookups/packages/packages.component';
import { AdminGovernoratesComponent } from '../lookups/governorates/governorates.component';
import { AdminCitiesComponent } from '../lookups/cities/cities.component';
import { AdminFacilitiesComponent } from '../lookups/facilities/facilities.component';
import { AdminDocumentsComponent } from '../lookups/documents/documents.component';
import { AdminDecorationsComponent } from '../lookups/decorations/decorations.component';
import { ManageAboutComponent } from './about/about.component';
import { ManageContactComponent } from './contact/contact.component';
import { ManagePointsComponent } from './points/points.component';
import { ManageFeatureAdsComponent } from './featureAds/featureAds.component';
import { AdminDurationsComponent } from './durations/durations.component';
import { AdminDiscountCodesComponent } from './discountCodes/discountCodes.component';
import { AdminPropertiesComponent } from './properties/properties.component';
import { AdminContactUsesComponent } from './contactUs/contactUses.component';
import { AdminSocialContactComponent } from './socialContacts/socialContacts.component';
import { AdminMinTimeToBookForChaletsComponent } from './minTimeToBookForChalets/minTimeToBookForChalets.component';


const routes: Routes = [
    {
        path: 'cities',
        component: AdminCitiesComponent,
    },
    {
        path: 'countries',
        component: AdminCountriesComponent,
    },
    {
        path: 'governorates',
        component: AdminGovernoratesComponent,
    },
    {
        path: 'packages',
        component: AdminPackagesComponent,
    },
    {
        path: 'facilities',
        component: AdminFacilitiesComponent,
    },
    {
        path: 'durations',
        component: AdminDurationsComponent,
    },
    {
        path: 'documents',
        component: AdminDocumentsComponent,
    },
    {
        path: 'minTimeToBookForChalets',
        component: AdminMinTimeToBookForChaletsComponent,
    },
    {
        path: 'decorations',
        component: AdminDecorationsComponent,
    },
    {
        path: 'discountCodes',
        component: AdminDiscountCodesComponent,
    },
    {
        path: 'about',
        component: ManageAboutComponent,
    },
    {
        path: 'contact',
        component: ManageContactComponent,
    },
    {
        path: 'points',
        component: ManagePointsComponent,
    },
    {
        path: 'featureAds',
        component: ManageFeatureAdsComponent,
    },
    {
        path: 'properties',
        component: AdminPropertiesComponent,
    },
    {
        path: 'ContactUs',
        component: AdminContactUsesComponent,
    },
    {
        path: 'SocialContacts',
        component: AdminSocialContactComponent,
    },
    {
        path: '**',
        component: NotFoundComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LookupsRoutingModule {
} 
