import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AbpHttpInterceptor } from 'abp-ng2-module';

import * as ApiServiceProxies from './service-proxies';

@NgModule({
    providers: [
        ApiServiceProxies.RoleServiceProxy,
        ApiServiceProxies.SessionServiceProxy,
        ApiServiceProxies.TenantServiceProxy,
        ApiServiceProxies.UserServiceProxy,
        ApiServiceProxies.TokenAuthServiceProxy,
        ApiServiceProxies.AccountServiceProxy,
        ApiServiceProxies.ConfigurationServiceProxy,
        ApiServiceProxies.CountryServiceProxy,
        ApiServiceProxies.GovernorateServiceProxy,
        ApiServiceProxies.CityServiceProxy,
        ApiServiceProxies.DefinitionServiceProxy,
        ApiServiceProxies.PackageServiceProxy,
        ApiServiceProxies.CommonServiceProxy,
        ApiServiceProxies.DurationServiceProxy,
        ApiServiceProxies.DiscountCodeServiceProxy,
        ApiServiceProxies.WalletServiceProxy,
        ApiServiceProxies.AdvertisementServiceProxy,
        ApiServiceProxies.ContactUsServiceProxy,
        ApiServiceProxies.SocialContactServiceProxy,
        ApiServiceProxies.ProjectServiceProxy,
        { provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true }
    ]
})
export class ServiceProxyModule { }
