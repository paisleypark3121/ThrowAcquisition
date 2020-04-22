using Call;
using Diagnostic;
using Log;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace ThrowAcquisition
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            container.RegisterType<ICall, HttpCall>();
            container.RegisterType<IDiagnostic, ApplicationInsightsTrace>();
            container.RegisterType<ILog, TableStorageLog>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}