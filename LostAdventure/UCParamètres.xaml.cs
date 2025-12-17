using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LostAdventure
{
    /// <summary>
    /// Logique d'interaction pour UCParamètres.xaml
    /// </summary>
    public partial class UCParamètres : UserControl
    {
        public UCParamètres()
        {
            InitializeComponent();
            
        }

       

        private void butQuitterParamètres_Click(object sender, RoutedEventArgs e)
        {
            var main = Application.Current.MainWindow as MainWindow;
            main?.AfficheMainMenu();
        }

        private void TailleFenetre_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this);
            if (mainWindow != null)
            {

                if (mainWindow.WindowState == WindowState.Normal)
                {
                    mainWindow.WindowState = WindowState.Maximized;
                }
                else
                {
                    mainWindow.WindowState = WindowState.Normal;
                }
            }
        }

        private void slidSon_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var main = Application.Current.MainWindow as MainWindow;
            if (main != null)
            {
                main.SetVolume(e.NewValue); // ✅ ajuste le volume
            }
        }
    }
}
