using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Caliburn.Micro.WP71.Recipes.ExternalModules
{
    public class StaticViewModelLocator
    {
        private static readonly ILog Log = LogManager.GetLog(typeof(ViewModelLocator));
        private static readonly Dictionary<string, Type> KnownViewModels = new Dictionary<string, Type>();
        private static readonly Dictionary<string, Assembly> InitializedAssemblies = new Dictionary<string, Assembly>();

        public static Func<Type, bool, Type> LocateTypeForViewType = (viewType, searchForInterface) =>
        {
            InitializeViewModelsAssembly(viewType);

            var typeName = viewType.FullName;

            Func<string, string> getReplaceString;
            if (searchForInterface)
                getReplaceString = r => r;
            else
                getReplaceString = r => Regex.IsMatch(r, @"I\${basename}") ? String.Empty : r;

            var viewModelTypeList = ViewModelLocator.NameTransformer.Transform(typeName, getReplaceString).ToList();

            var viewModelType = (from vm in viewModelTypeList
                                 where KnownViewModels.ContainsKey(vm)
                                 select KnownViewModels[vm]).FirstOrDefault();

            if (viewModelType == null)
            {
                Log.Warn("View Model not found. Searched: {0}.", string.Join(", ", viewModelTypeList.ToArray()));
            }

            return viewModelType;
        };

        public static void InitializeViewModelsAssembly(Type viewModel)
        {
            var assembly = viewModel.Assembly;

            if (InitializedAssemblies.ContainsKey(assembly.FullName))
                return;

            InitializedAssemblies.Add(assembly.FullName, assembly);

            var bootStrappers = from t in assembly.GetTypes()
                                where
                                    (t.IsAbstract == false && t.IsInterface == false && t.IsPublic &&
                                     typeof(ExternalAssemblyBootstrapper).IsAssignableFrom(t))
                                select t;

            foreach (var bootStrapper in bootStrappers)
            {
                InitializeModule(bootStrapper);
            }

            RegisterViewModelsWithLocator(assembly);

        }

        private static void InitializeModule(Type bootStrapper)
        {
            var moduleInitializer = Activator.CreateInstance(bootStrapper) as ExternalAssemblyBootstrapper;
            if (moduleInitializer == null)
                return;

            IoC.BuildUp(moduleInitializer);
            moduleInitializer.Initialize();
        }

        private static void RegisterViewModelsWithLocator(Assembly assembly)
        {
            foreach (var viewModel in assembly.GetExportedTypes().Where(x => x.IsPublic && !x.IsInterface && x.Name.EndsWith("ViewModel")))
            {
                RegisterKnownViewModel(viewModel);
            }
        }

        private static void RegisterKnownViewModel(Type viewModel)
        {
            if (viewModel == null)
                return;

            if (string.IsNullOrWhiteSpace(viewModel.FullName))
                return;

            if (KnownViewModels.ContainsKey(viewModel.FullName))
                return;

            KnownViewModels.Add(viewModel.FullName, viewModel);
        }

    }
}
