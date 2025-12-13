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
using System.Windows.Threading;

namespace LostAdventure
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Image[] spriteAventurier;
        public Image[] spriteEnnemis;
        public Image[] spriteBoss;
        private DispatcherTimer minuterie;
        double aventurierX, aventurierY;
        bool left, right, up, down;

        public MainWindow()
        {
            InitializeComponent();
            AfficheMainMenu();
        }
        public void AfficheMainMenu()
        {
            UCMainMenu ucMainMenu = new UCMainMenu();
            ZoneDeJeu.Content = ucMainMenu;
        }

        private void InitialiseJeu()
        {
            aventurierX = 100;
        }


        private void AfficheReglesJeu()
        {
            UCReglesJeu ucReglesJeu = new UCReglesJeu();
            ZoneDeJeu.Content = ucReglesJeu;
        }

        private void InitializeTimer()
        {
            minuterie = new DispatcherTimer();
            minuterie.Interval = TimeSpan.FromMilliseconds(16);
            //minuterie.Tick += ;
            minuterie.Start();
        }


        private void butJouer_Click(object sender, RoutedEventArgs e)
        {
            InitialiseJeu();
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left) left = true;
            if (e.Key == Key.Right) right = true;
            if (e.Key == Key.Up) up = true;
            if (e.Key == Key.Down) down = true;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left) left = false;
            if (e.Key == Key.Right) right = false;
            if (e.Key == Key.Up) up = false;
            if (e.Key == Key.Down) down = false;
        }
    }
}