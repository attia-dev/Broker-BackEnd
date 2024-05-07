using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Broker.Authorization
{
    public class BrokerAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            //context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
            //context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            //context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            SetDashboardsPermissions(context);
            SetLookupsPermissions(context);
            SetUsersPermissions(context);
            SetAdvertisementsPermissions(context);
            SetManagePagesPermissions(context);
            SetAdminsPermissions(context);
         
      
        }

        void SetLookupsPermissions(IPermissionDefinitionContext context)
        {
            var readLookups = context.CreatePermission(PermissionNames.Lookups.Read, L("Permission.Lookups"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            var writeLookups = context.CreatePermission(PermissionNames.Lookups.Write, L("Permission.Lookups"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            var deleteLookups = context.CreatePermission(PermissionNames.Lookups.Delete, L("Permission.Lookups"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);

            readLookups.CreateChildPermission(PermissionNames.Countries.Read, L("Permission.Countries"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            writeLookups.CreateChildPermission(PermissionNames.Countries.Write, L("Permission.Countries"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            deleteLookups.CreateChildPermission(PermissionNames.Countries.Delete, L("Permission.Countries"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);

            readLookups.CreateChildPermission(PermissionNames.Governorates.Read, L("Permission.Governorates"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            writeLookups.CreateChildPermission(PermissionNames.Governorates.Write, L("Permission.Governorates"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            deleteLookups.CreateChildPermission(PermissionNames.Governorates.Delete, L("Permission.Governorates"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);



            readLookups.CreateChildPermission(PermissionNames.Cities.Read, L("Permission.Cities"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            writeLookups.CreateChildPermission(PermissionNames.Cities.Write, L("Permission.Cities"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            deleteLookups.CreateChildPermission(PermissionNames.Cities.Delete, L("Permission.Cities"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);

            readLookups.CreateChildPermission(PermissionNames.Packages.Read, L("Permission.Packages"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            writeLookups.CreateChildPermission(PermissionNames.Packages.Write, L("Permission.Packages"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            deleteLookups.CreateChildPermission(PermissionNames.Packages.Delete, L("Permission.Packages"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);

            readLookups.CreateChildPermission(PermissionNames.Facilities.Read, L("Permission.Facilities"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            writeLookups.CreateChildPermission(PermissionNames.Facilities.Write, L("Permission.Facilities"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            deleteLookups.CreateChildPermission(PermissionNames.Facilities.Delete, L("Permission.Facilities"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);


          //readLookups.CreateChildPermission(PermissionNames.Documents.Read, L("Permission.Documents"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
          //writeLookups.CreateChildPermission(PermissionNames.Documents.Write, L("Permission.Documents"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
          //deleteLookups.CreateChildPermission(PermissionNames.Documents.Delete, L("Permission.Documents"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
          //
          //readLookups.CreateChildPermission(PermissionNames.Decorations.Read, L("Permission.Decorations"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
          //writeLookups.CreateChildPermission(PermissionNames.Decorations.Write, L("Permission.Decorations"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
          //deleteLookups.CreateChildPermission(PermissionNames.Decorations.Delete, L("Permission.Decorations"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);

            readLookups.CreateChildPermission(PermissionNames.Durations.Read, L("Permission.Durations"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            writeLookups.CreateChildPermission(PermissionNames.Durations.Write, L("Permission.Durations"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            deleteLookups.CreateChildPermission(PermissionNames.Durations.Delete, L("Permission.Durations"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);

            readLookups.CreateChildPermission(PermissionNames.DiscountCodes.Read, L("Permission.DiscountCodes"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            writeLookups.CreateChildPermission(PermissionNames.DiscountCodes.Write, L("Permission.DiscountCodes"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            deleteLookups.CreateChildPermission(PermissionNames.DiscountCodes.Delete, L("Permission.DiscountCodes"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);

            readLookups.CreateChildPermission(PermissionNames.Properties.Read, L("Permission.Properties"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            writeLookups.CreateChildPermission(PermissionNames.Properties.Write, L("Permission.Properties"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            deleteLookups.CreateChildPermission(PermissionNames.Properties.Delete, L("Permission.Properties"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);

            readLookups.CreateChildPermission(PermissionNames.FeatureAds.Read, L("Permission.FeatureAds"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            writeLookups.CreateChildPermission(PermissionNames.FeatureAds.Write, L("Permission.FeatureAds"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            deleteLookups.CreateChildPermission(PermissionNames.FeatureAds.Delete, L("Permission.FeatureAds"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);

            readLookups.CreateChildPermission(PermissionNames.Points.Read, L("Permission.Points"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            writeLookups.CreateChildPermission(PermissionNames.Points.Write, L("Permission.Points"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            deleteLookups.CreateChildPermission(PermissionNames.Points.Delete, L("Permission.Points"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);

            readLookups.CreateChildPermission(PermissionNames.ContactUs.Read, L("Permission.ContactUs"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            writeLookups.CreateChildPermission(PermissionNames.ContactUs.Write, L("Permission.ContactUs"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            deleteLookups.CreateChildPermission(PermissionNames.ContactUs.Delete, L("Permission.ContactUs"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
        }

        void SetUsersPermissions(IPermissionDefinitionContext context)
        {
            var readLookups = context.CreatePermission(PermissionNames.Users.Read, L("Permission.Users"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            var writeLookups = context.CreatePermission(PermissionNames.Users.Write, L("Permission.Users"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            var deleteLookups = context.CreatePermission(PermissionNames.Users.Delete, L("Permission.Users"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);

            readLookups.CreateChildPermission(PermissionNames.Brokers.Read, L("Permission.Brokers"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            writeLookups.CreateChildPermission(PermissionNames.Brokers.Write, L("Permission.Brokers"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            deleteLookups.CreateChildPermission(PermissionNames.Brokers.Delete, L("Permission.Brokers"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);

            readLookups.CreateChildPermission(PermissionNames.Owners.Read, L("Permission.Owners"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            writeLookups.CreateChildPermission(PermissionNames.Owners.Write, L("Permission.Owners"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            deleteLookups.CreateChildPermission(PermissionNames.Owners.Delete, L("Permission.Owners"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);

            readLookups.CreateChildPermission(PermissionNames.Seekers.Read, L("Permission.Seekers"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            writeLookups.CreateChildPermission(PermissionNames.Seekers.Write, L("Permission.Seekers"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            deleteLookups.CreateChildPermission(PermissionNames.Seekers.Delete, L("Permission.Seekers"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);

            readLookups.CreateChildPermission(PermissionNames.Companies.Read, L("Permission.Companies"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            writeLookups.CreateChildPermission(PermissionNames.Companies.Write, L("Permission.Companies"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            deleteLookups.CreateChildPermission(PermissionNames.Companies.Delete, L("Permission.Companies"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
        }
        void SetAdvertisementsPermissions(IPermissionDefinitionContext context)
        {
            var readLookups = context.CreatePermission(PermissionNames.Advertisements.Read, L("Permission.Advertisements"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            var writeLookups = context.CreatePermission(PermissionNames.Advertisements.Write, L("Permission.Advertisements"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            var deleteLookups = context.CreatePermission(PermissionNames.Advertisements.Delete, L("Permission.Advertisements"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
        }
        void SetManagePagesPermissions(IPermissionDefinitionContext context)
        {
            var readLookups = context.CreatePermission(PermissionNames.ManagePages.Read, L("Permission.ManagePages"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            var writeLookups = context.CreatePermission(PermissionNames.ManagePages.Write, L("Permission.ManagePages"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            var deleteLookups = context.CreatePermission(PermissionNames.ManagePages.Delete, L("Permission.ManagePages"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);

            readLookups.CreateChildPermission(PermissionNames.Abouts.Read, L("Permission.Abouts"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            writeLookups.CreateChildPermission(PermissionNames.Abouts.Write, L("Permission.Abouts"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            deleteLookups.CreateChildPermission(PermissionNames.Abouts.Delete, L("Permission.Abouts"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);

            readLookups.CreateChildPermission(PermissionNames.Contacts.Read, L("Permission.Contacts"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            writeLookups.CreateChildPermission(PermissionNames.Contacts.Write, L("Permission.Contacts"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            deleteLookups.CreateChildPermission(PermissionNames.Contacts.Delete, L("Permission.Contacts"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
        }


        void SetDashboardsPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Dashboards.Read, L("Permission.Dashboards"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
        }

        void SetAdminsPermissions(IPermissionDefinitionContext context)
        {
            var readAdmins = context.CreatePermission(PermissionNames.Admins.Read, L("Permission.Admins"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            var writeAdmins = context.CreatePermission(PermissionNames.Admins.Write, L("Permission.Admins"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            context.CreatePermission(PermissionNames.Admins.Delete, L("Permission.Admins"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);

            readAdmins.CreateChildPermission(PermissionNames.UserPermissions.Read, L("Permission.UserPermissions"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
            writeAdmins.CreateChildPermission(PermissionNames.UserPermissions.Write, L("Permission.UserPermissions"), multiTenancySides: MultiTenancySides.Host | MultiTenancySides.Tenant);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, BrokerConsts.LocalizationSourceName);
        }
    }
}
