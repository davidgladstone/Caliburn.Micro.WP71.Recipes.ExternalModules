namespace Caliburn.Micro.WP71.Recipes.ExternalModules
{
    public static class IocExtensions
    {
        public static object GetInstance(PhoneContainer container, System.Type service, string key)
        {
            var instance = container.GetInstance(service, key);

            if (instance != null)
                return instance;

            if (!service.Name.EndsWith("ViewModel"))
                return null;
                
            StaticViewModelLocator.InitializeViewModelsAssembly(service);

            return container.GetInstance(service, key);
        }
    }
}
