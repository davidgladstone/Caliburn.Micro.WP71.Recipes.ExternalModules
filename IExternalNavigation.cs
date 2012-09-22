using System;

//namespace Caliburn.Micro
//{
//    /// <summary>
//    /// Extension methods related to navigation.
//    /// </summary>
//    public static class NavigationExtensions
//    {
//        /// <summary>
//        /// Creates a Uri builder based on a view model type.
//        /// </summary>
//        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
//        /// <param name="navigationService">The navigation service.</param>
//        /// <returns>The builder.</returns>
//        public static UriBuilder<TViewModel> UriFor<TViewModel>(this INavigationService navigationService)
//        {
//            return new UriBuilder<TViewModel>().AttachTo(navigationService);
//        }
//    }
//}

namespace Caliburn.Micro.WP71.Recipes.ExternalModules
{
    /// <summary>
    /// Used for navigating to a page in an another assembly
    /// </summary>
    public static class ExternalNavigationExtensions
    {
        public static ExternalUriBuilder<TViewModel> UriForExternalPage<TViewModel>(this INavigationService navigationService)
        {
            return new ExternalUriBuilder<TViewModel>().AttachTo(navigationService);
        }
    }

    public interface IExternalAssemblyBootstrapper
    {
        void Initialize();
    }

    public abstract class ExternalAssemblyBootstrapper : IExternalAssemblyBootstrapper
    {
        public IPhoneContainer Container { get; set; }

        public virtual void Initialize()
        {
            ConfigureContainer(Container as PhoneContainer);
        }

        //protected virtual void RegisterViewModelsWithLocator(Assembly assembly)
        //{
        //    foreach (var viewModel in assembly.GetExportedTypes().Where(x => x.IsPublic && !x.IsInterface && x.Name.EndsWith("ViewModel")))
        //    {
        //        ViewModelLocator.RegisterKnownViewModel(viewModel);
        //    }
        //}

        protected abstract void ConfigureContainer(PhoneContainer container);
    }

    public class ExternalUriBuilder<TViewModel> : UriBuilder<TViewModel>
    {
        public void Navigate(string assemblyName, string pageFileName)
        {
            //ExternalViewModelLoader.InitializeExternalAssembly(assemblyName);

            //var pageUri = string.Format("/{0};component/{1}", assemblyName, pageFileName);

            //var source = new Uri(pageUri + this.BuildQueryString(), UriKind.Relative);
            //if (this.navigationService == null)
            //    throw new Exception("Cannot navigate without attaching an INavigationService. Call AttachTo first.");

            //this.navigationService.Navigate(source);
        }


        public new ExternalUriBuilder<TViewModel> AttachTo(INavigationService inNavigationService)
        {
            //this.navigationService = inNavigationService;
            return this;
        }
    }

    public static class ExternalViewModelLoader
    {
        public static void InitializeExternalAssembly(string assemblyName)
        {
            var nameSpace = assemblyName.Replace("-", "_");
            var typeName = string.Format("{0}.{1},{2}", nameSpace, "Bootstrapper", assemblyName);
            var externalBootstrapper = Type.GetType(typeName);

            if (externalBootstrapper != null)
            {
                var bootstrapper = Activator.CreateInstance(externalBootstrapper) as IExternalAssemblyBootstrapper;
                if (bootstrapper != null)
                {
                    IoC.BuildUp(bootstrapper);
                    bootstrapper.Initialize();
                }
            }
        }
    }
}
