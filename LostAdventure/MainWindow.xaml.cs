using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
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
        
        private DispatcherTimer minuterie;
        double aventurierX, aventurierY;
        bool gauche, droite;

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

        public void InitialiseJeu()
        {
            aventurierX = 100;
            aventurierY = 200;
            InitializeTimer();
           
        }

        

        public void AfficheReglesJeu()
        {
            UCReglesJeu ucReglesJeu = new UCReglesJeu();
            ZoneDeJeu.Content = ucReglesJeu;
        }

        public void AfficheJeu()
        {
            UCJeu uc = new UCJeu();
            ZoneDeJeu.Content = uc;
        }

        private void deplacementAventurier()
        {

        }

        private void InitializeTimer()
        {
            minuterie = new DispatcherTimer();
            minuterie.Interval = TimeSpan.FromMilliseconds(16);
            //minuterie.Tick += Jeu;
            minuterie.Start();
        }

        

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Q) droite = true;
            if (e.Key == Key.D) droite = true;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Q) gauche = false;
            if (e.Key == Key.D) droite = false;
        }
    }
}