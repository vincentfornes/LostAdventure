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
        private BitmapImage[]  aventurier = new BitmapImage[8];
        private int nb = 0;
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

        public void InitialiseJeu()
        {
            aventurierX = 100;
            aventurierY = 200;
            InitializeTimer();
            InitializeSprites();
        }

        private void Jeu(object sender, EventArgs e)
        {
            nb++;
            if (nb == aventurier.Length)
                nb= 0;
            Aventurier.Source = aventurier[nb];
        }

        public void AfficheReglesJeu()
        {
            UCReglesJeu ucReglesJeu = new UCReglesJeu();
            ZoneDeJeu.Content = ucReglesJeu;
        }

        private void deplacementAventurier()
        {
        
        }

        private void InitializeTimer()
        {
            minuterie = new DispatcherTimer();
            minuterie.Interval = TimeSpan.FromMilliseconds(16);
            minuterie.Tick += Jeu;
            minuterie.Start();
        }

        private void InitializeSprites()
        {
            for (int i = 1; i < aventurier.Length; i++)
                aventurier[i] = new BitmapImage(new Uri($"/Images/Aventurier/AventurierMarche({i}).png"));
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