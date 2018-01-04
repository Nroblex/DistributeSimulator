using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GitUtilSimulate
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void AppStart(object sender, StartupEventArgs e)
        {
            MainWindow window = new GitUtilSimulate.MainWindow
            {
                Title = "GitUtil-Simulate-Distribute",
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            window.Show();
        }
    }
}
