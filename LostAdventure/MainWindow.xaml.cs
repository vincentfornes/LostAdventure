using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AfficheMainMenu();
        }
        private void AfficheMainMenu()
        {
            UCMainMenu ucMainMenu = new UCMainMenu();
            ZoneDeJeu.Content = ucMainMenu;
        }

        private void AfficheReglesJeu()
        {
            UCReglesJeu ucReglesJeu = new UCReglesJeu();
            ZoneDeJeu.Content = ucReglesJeu;
        }

        private void butJouer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void butRegles_Click(object sender, RoutedEventArgs e)
        {
            AfficheReglesJeu();
        }

        private void butTouches_Click(object sender, RoutedEventArgs e)
        {

        }

        private void butQuitter_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}