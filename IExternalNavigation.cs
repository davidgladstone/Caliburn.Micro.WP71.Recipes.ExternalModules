using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
            var phoneContainer = Container as PhoneContainer;
            ConfigureStorageMechanismsAndWorkers(phoneContainer);
            ConfigureContainer(phoneContainer);
        }

        /// <summary>
        /// Identify, load and configure all instances of <see cref="IStorageMechanism"/>
        /// and <see cref="IStorageHandler"/> that are defined in the assembly associated
        /// with this bootstrapper.
        /// </summary>
        /// <param name="phoneContainer">The currently configured 
        /// <see cref="PhoneContainer"/>.</param>
        /// <remarks>Caliburn Micro will automatically load storage handlers and storage 
        /// mechanisms from the assemblies configured in <see cref="AssemblySource.Instance"/> 
        /// when <see cref="PhoneContainer.RegisterPhoneServices"/> is first invoked.
        /// Since the purpose of this recipe is to allow the delayed loading
        /// of assemblies it makes sense to locate the storage handlers alongside the
        /// view models in the same assembly. 
        /// </remarks>
        private void ConfigureStorageMechanismsAndWorkers(PhoneContainer phoneContainer)
        {
            if (phoneContainer == null) return;

            var coordinator = (StorageCoordinator)(phoneContainer.GetInstance(typeof(StorageCoordinator), null));
            var assembly = GetType().Assembly;

            phoneContainer.AllTypesOf<IStorageMechanism>(assembly);
            phoneContainer.AllTypesOf<IStorageHandler>(assembly);

            phoneContainer.GetAllInstances(typeof(IStorageMechanism)).
                            Where(m => ReferenceEquals(m.GetType().Assembly, assembly)).
                            Apply(m => coordinator.AddStorageMechanism((IStorageMechanism)m));

            phoneContainer.GetAllInstances(typeof(IStorageHandler)).
                            Where(h => ReferenceEquals(h.GetType().Assembly, assembly)).
                            Apply(h => coordinator.AddStorageHandler((IStorageHandler)h));
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
