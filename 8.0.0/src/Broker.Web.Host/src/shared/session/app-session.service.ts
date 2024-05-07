import { AbpMultiTenancyService } from 'abp-ng2-module';
import { Injectable } from '@angular/core';
import {
    ApplicationInfoDto,
    GetCurrentLoginInformationsOutput,
    SessionServiceProxy,
    TenantLoginInfoDto,
    UserLoginInfoDto,
    SeekerLoginInfoDto,
    BrokerPersonLoginInfoDto,
    OwnerLoginInfoDto,
    CompanyLoginInfoDto
} from '@shared/service-proxies/service-proxies';

@Injectable()
export class AppSessionService {

    private _user: UserLoginInfoDto;
    private _tenant: TenantLoginInfoDto;
    private _application: ApplicationInfoDto;

    private _seeker: SeekerLoginInfoDto;
    private _brokerPerson: BrokerPersonLoginInfoDto;
    private _owner: OwnerLoginInfoDto;
    private _company: CompanyLoginInfoDto;
    constructor(
        private _sessionService: SessionServiceProxy,
        private _abpMultiTenancyService: AbpMultiTenancyService) {
    }

    get application(): ApplicationInfoDto {
        return this._application;
    }

    get user(): UserLoginInfoDto {
        return this._user;
    }

    get userId(): number {
        return this.user ? this.user.id : null;
    }

    get tenant(): TenantLoginInfoDto {
        return this._tenant;
    }

    get tenantId(): number {
        return this.tenant ? this.tenant.id : null;
    }

    get seeker(): SeekerLoginInfoDto {
        return this._seeker;
    }

    get seekerId(): number {
        return this.seeker ? this.seeker.id : null;
    }
    get brokerPerson(): BrokerPersonLoginInfoDto {
        return this._brokerPerson;
    }

    get brokerPersonId(): number {
        return this.brokerPerson ? this.brokerPerson.id : null;
    }
    get company(): CompanyLoginInfoDto {
        return this._company;
    }

    get companyId(): number {
        return this.company ? this.company.id : null;
    }
    get owner(): OwnerLoginInfoDto {
        return this._owner;
    }

    get ownerId(): number {
        return this.owner ? this.owner.id : null;
    }




    getShownLoginName(): string {
        const userName = this._user.userName;
        if (!this._abpMultiTenancyService.isEnabled) {
            return userName;
        }

        return (this._tenant ? this._tenant.tenancyName : '.') + '\\' + userName;
    }

    init(): Promise<boolean> {
        return new Promise<boolean>((resolve, reject) => {
            this._sessionService.getCurrentLoginInformations().toPromise().then((result: GetCurrentLoginInformationsOutput) => {
                this._application = result.application;
                this._user = result.user;
                this._tenant = result.tenant;

                this._seeker = result.seeker;
                this._brokerPerson = result.brokerPerson;
                this._company = result.company;
                this._owner = result.owner;

                resolve(true);
            }, (err) => {
                reject(err);
            });
        });
    }

    changeTenantIfNeeded(tenantId?: number): boolean {
        if (this.isCurrentTenant(tenantId)) {
            return false;
        }

        abp.multiTenancy.setTenantIdCookie(tenantId);
        location.reload();
        return true;
    }

    private isCurrentTenant(tenantId?: number) {
        if (!tenantId && this.tenant) {
            return false;
        } else if (tenantId && (!this.tenant || this.tenant.id !== tenantId)) {
            return false;
        }

        return true;
    }
}
