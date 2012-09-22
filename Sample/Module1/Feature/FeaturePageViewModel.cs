using System.Windows;
using Caliburn.Micro;

namespace Module1.Feature
{
    public class FeaturePageViewModel : Screen
    {
        public void Testi()
        {
            MessageBox.Show("Executing feature");
        }
    }
}
