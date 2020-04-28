using Call;
using Diagnostic;
using Log;
using Store;
using System.Web.Mvc;
using ThrowAPI;
using Unity;
using Unity.Injection;
using Unity.Mvc5;

namespace ThrowAcquisition
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            const string PartnerID = "1";
            const string Password = "passw0rd";
            string login_url = @"https://stg3.lanciobp.net:4600/api/Login";

            var container = new UnityContainer();

            string[] store_args = new string[4] {
                "DefaultEndpointsProtocol=https;AccountName=0100lanciostorage;AccountKey=KIZ/Su4EvRebOilwawZQkTWlXs6gKDO76S4uXD1q0ss7GNg5f7DC66i39Ln2B/rl/mjPjSYtigZDnOsWKDrRSg==;EndpointSuffix=core.windows.net",
                "TokenTableTest",
                "TokenID",
                "TokenValue"
            };
            container.RegisterType<IStore, TableStorageStore>("LoginStore", new InjectionConstructor(new object[] { store_args }));

            container.RegisterType<ICall, HttpCall>();
            container.RegisterType<IDiagnostic, ApplicationInsightsTrace>();
            container.RegisterType<ILog, TableStorageLog>();
            container.RegisterType<IActivation, Activation>();
            container.RegisterType<IMobileUserRecognition, MobileUserRecognition>();
            container.RegisterType<IResponseParser, LoginParser>("LoginParser");
            container.RegisterType<IResponseParser, ActivationParser>("ActivationParser");

            container.RegisterType<ILogin, Login>(new Unity.Injection.InjectionConstructor(
                new object[] {
                    login_url,
                    PartnerID,
                    Password,
                    container.Resolve<ICall>(),
                    container.Resolve<IResponseParser>("LoginParser"),
                    container.Resolve<IStore>("LoginStore"),
                }));

            container.RegisterType<IActivation, Activation>(new Unity.Injection.InjectionConstructor(
                new object[] {
                    login_url,
                    PartnerID,
                    container.Resolve<ICall>(),
                    container.Resolve<IDiagnostic>(),
                    container.Resolve<ILogin>(),
                    container.Resolve<IResponseParser>("ActivationParser"),
                }));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}