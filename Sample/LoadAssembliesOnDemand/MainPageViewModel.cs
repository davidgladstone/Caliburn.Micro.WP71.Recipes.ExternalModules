using System;
using System.Diagnostics;
using System.Windows;
using Caliburn.Micro;
using Caliburn.Micro.WP71.Recipes.ExternalModules;
using Module1;
using Module1.Feature;
using Module2;

namespace LoadAssembliesOnDemand
{
    public class MainPageViewModel : Screen
    {
        private readonly INavigationService navigation;

        private Screen another;

        public Screen Another
        {
            get { return another; }
            set { another = value; NotifyOfPropertyChange(() => Another); }
        }

        public MainPageViewModel(INavigationService navigation)
        {
            this.navigation = navigation;
        }

        public void GotoModule1()
        {
            navigation.UriFor<MyPageViewModel>()
                .Navigate();
        }

        public void GotoFeature()
        {
            navigation.UriFor<FeaturePageViewModel>()
                .Navigate();
        }

        public void LoadModule2()
        {
            var screen = IoC.Get<AnotherViewModel>();

            Another = screen;
        }

    }
}
