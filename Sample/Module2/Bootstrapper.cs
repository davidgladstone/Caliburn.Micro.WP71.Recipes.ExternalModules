using Caliburn.Micro;
using Caliburn.Micro.WP71.Recipes.ExternalModules;

namespace Module2
{
    public class Bootstrapper : ExternalAssemblyBootstrapper
    {
        protected override void ConfigureContainer(PhoneContainer container)
        {
            container.PerRequest<AnotherViewModel>();
        }
    }
}
