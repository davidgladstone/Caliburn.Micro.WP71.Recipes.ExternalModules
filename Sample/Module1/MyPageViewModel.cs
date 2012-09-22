using System.Windows;
using Caliburn.Micro;

namespace Module1
{
    public class MyPageViewModel : Screen
    {
        public void HelloFromModule1()
        {
            MessageBox.Show("Hello from module1");
        }
    }
}
