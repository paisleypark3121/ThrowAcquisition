using AppSettings;
using Call;
using Diagnostic;
using Log;
using Store;
using System.Web.Mvc;
using ThrowAcquisition.Controllers;
using ThrowAcquisition.ServiceLayer.Catalogue;
using ThrowAPI;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Mvc5;

namespace ThrowAcquisition
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {            
            var container = new UnityContainer();

            container.RegisterType<IAppSettings, AppSettings.AppSettings>(new ContainerControlledLifetimeManager());

            string[] store_login_args = new string[4] {
                "DefaultEndpointsProtocol=https;AccountName=0100lanciostorage;AccountKey=KIZ/Su4EvRebOilwawZQkTWlXs6gKDO76S4uXD1q0ss7GNg5f7DC66i39Ln2B/rl/mjPjSYtigZDnOsWKDrRSg==;EndpointSuffix=core.windows.net",
                "TokenTable",
                "TokenID",
                "TokenValue"
            };
            container.RegisterType<IStore, TableStorageStore>("LoginStore", new InjectionConstructor(new object[] { store_login_args }));

            container.RegisterType<IService, Service>();
            container.RegisterType<ICall, HttpCall>();
            container.RegisterType<IDiagnostic, ApplicationInsightsTrace>();
            container.RegisterType<ILog, TableStorageLog>();
            container.RegisterType<IResponseParser, LoginParser>("LoginParser");
            container.RegisterType<IResponseParser, ActivationParser>("ActivationParser");
            container.RegisterType<IResponseParser, DeactivationParser>("DeactivationParser");
            container.RegisterType<IResponseParser, CheckSubsParser>("CheckSubsParser");
            container.RegisterType<IResponseParser, OTPParser>("OTPParser");

            container.RegisterType<ILogin, Login>(
                new InjectionConstructor(
                    new object[] {
                        container.Resolve<IAppSettings>(),
                        container.Resolve<ICall>(),
                        container.Resolve<IStore>("LoginStore"),
                }));

            container.RegisterType<IEndUser, EndUser>(
                new InjectionConstructor(
                    new object[] {
                        container.Resolve<IAppSettings>(),
                        container.Resolve<ICall>(),
                        container.Resolve<IDiagnostic>(),
                        container.Resolve<ILogin>(),
                        container.Resolve<IResponseParser>("ActivationParser"),
                        container.Resolve<IResponseParser>("DeactivationParser"),
                        container.Resolve<IResponseParser>("CheckSubsParser"),
                        container.Resolve<IResponseParser>("OTPParser"),
                }));

            //container.RegisterType<AcquisitionController>(
            //    new InjectionConstructor(
            //        container.Resolve<IDiagnostic>(),
            //        container.Resolve<IMobileUserRecognition>(),
            //        container.Resolve<IActivation>(),
            //        container.Resolve<IService>()
            //    ));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}