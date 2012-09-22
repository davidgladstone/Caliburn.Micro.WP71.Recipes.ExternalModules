using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace Caliburn.Micro.WP71.Recipes.ExternalModules
{
    public static class PhoneViewLocator
    {
        static readonly ILog Log = LogManager.GetLog(typeof(ViewModelLocator));
        public static Func<Type, DependencyObject, object, Type> LocateTypeForModelType = (modelType, displayLocation, context) =>
        {
            var viewTypeName = modelType.FullName;

            if (Execute.InDesignMode)
            {
                viewTypeName = ViewLocator.ModifyModelTypeAtDesignTime(viewTypeName);
            }

            if (string.IsNullOrWhiteSpace(viewTypeName))
                return null;

            viewTypeName = viewTypeName.Substring(
                0,
                viewTypeName.IndexOf("`") < 0
                    ? viewTypeName.Length
                    : viewTypeName.IndexOf("`")
                );

            Func<string, string> getReplaceString;
            if (context == null)
            {
                getReplaceString = r => { return r; };
            }
            else
            {
                getReplaceString = r =>
                {
                    return Regex.Replace(r, Regex.IsMatch(r, "Page$") ? "Page$" : "View$", ViewLocator.ContextSeparator + context);
                };
            }

            var viewTypeList = ViewLocator.NameTransformer.Transform(viewTypeName, getReplaceString);
            var viewType = (from type in modelType.Assembly.GetExportedTypes()
                            where viewTypeList.Contains(type.FullName)
                            select type).FirstOrDefault();

            if (viewType == null)
            {
                Log.Warn("View not found. Searched: {0}.", string.Join(", ", viewTypeList.ToArray()));
            }

            return viewType;
        };
    }
}
