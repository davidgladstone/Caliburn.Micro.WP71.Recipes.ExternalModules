using System.Windows;
using Caliburn.Micro;

namespace Module2
{
    public class AnotherViewModel : Screen
    {
        public override string DisplayName
        {
            get { return "another"; }
            set
            {
                base.DisplayName = value;
            }
        }

        public void Testi()
        {
            MessageBox.Show("Module2 says hello");
        }
    }
}
