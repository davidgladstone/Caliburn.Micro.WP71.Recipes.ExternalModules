This project enables the on-demand loading of external assemblies when creating a Windows Phone app using Caliburn.Micro.

[http://mikaelkoskinen.net/post/windows-phone-caliburn-micro-split-app-multiple-assemblies.aspx](http://mikaelkoskinen.net/post/windows-phone-caliburn-micro-split-app-multiple-assemblies.aspx)

##Installation##

1. Install-Package Caliburn.Micro.WP71.Recipes.ExternalModules
2. Add the following code to Bootstapper's configure-method: 
 
`Caliburn.Micro.WP71.Recipes.ExternalModules.ExternalModuleLoader.Initialize();`

##Usage##
Create assembly-level bootstrappers by inheriting ExternalAssemblyBootstrapper:

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

## Sample ##
The sample app is available from the repository.