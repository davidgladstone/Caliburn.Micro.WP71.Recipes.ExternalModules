using Caliburn.Micro;
using Caliburn.Micro.WP71.Recipes.ExternalModules;
using Module1.Feature;

namespace Module1
{
    public class Bootstrapper : ExternalAssemblyBootstrapper
    {
        protected override void ConfigureContainer(PhoneContainer container)
        {
            container.PerRequest<MyPageViewModel>();
            container.PerRequest<FeaturePageViewModel>();
        }
    }
}
