using System;

namespace Caliburn.Micro.WP71.Recipes.ExternalModules
{
    public static class ExternalModuleLoader
    {
        public static Func<object> Initialize = () =>
                                                    {
                                                        ViewModelLocator.LocateTypeForViewType = StaticViewModelLocator.LocateTypeForViewType;
                                                        ViewLocator.LocateTypeForModelType = PhoneViewLocator.LocateTypeForModelType;
                                                        return null;
                                                    };
    }
}
