using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using Caliburn.Micro.WP71.Recipes.ExternalModules;
using Module1;
using Module2;

namespace LoadAssembliesOnDemand
{
    public class MyBootstrapper : PhoneBootstrapper
    {
        private PhoneContainer container;

        protected override void Configure()
        {
            container = new PhoneContainer(RootFrame);
            container.RegisterPhoneServices();
            
            container.PerRequest<MainPageViewModel>();

            Caliburn.Micro.WP71.Recipes.ExternalModules.ExternalModuleLoader.Initialize();
        }

        protected override object GetInstance(System.Type service, string key)
        {
            return IocExtensions.GetInstance(container, service, key);
        }

        protected override IEnumerable<object> GetAllInstances(System.Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }
    }
}
