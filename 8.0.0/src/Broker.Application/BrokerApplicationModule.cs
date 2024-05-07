using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Broker.Authorization;

namespace Broker
{
    [DependsOn(
        typeof(BrokerCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class BrokerApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<BrokerAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(BrokerApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
