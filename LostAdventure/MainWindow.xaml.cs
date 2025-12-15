using System.Diagnostics.Eventing.Reader;
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

        public enum EtatAnimation
        {
            Immobile,
            Marche,
            Attaque,

        }

        private DispatcherTimer minuterie;

        bool gauche, droite, saut;

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

            InitializeTimer();

        }



        public void AfficheReglesJeu()
        {
            UCReglesJeu ucReglesJeu = new UCReglesJeu();
            ZoneDeJeu.Content = ucReglesJeu;
        }
        public void AfficheTouches()
        {
            UCTouches ucTouches = new UCTouches();
            ZoneDeJeu.Content = ucTouches;
        }

        public void AfficheJeu()
        {
            UCJeu uc = new UCJeu();
            ZoneDeJeu.Content = uc;
            uc.AttacherEvenementsClavier();
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
            if (e.Key == Key.Q) gauche = true;
            if (e.Key == Key.D) droite = true;
            if (e.Key == Key.Space) saut = true;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Q) gauche = false;
            if (e.Key == Key.D) droite = false;
            if (e.Key == Key.Space) saut = false;
        }


        public abstract class Entite
        {
            public double X { get; protected set; }
            public double Y { get; protected set; }
            public int Vie { get; protected set; }
            public int Attaque { get; protected set; }
            public double Vitesse { get; protected set; }
            public double Largeur { get; protected set; }
            public double Hauteur { get; protected set; }


            
            
            protected Entite(double x, double y, int vie, int attaque, double vitesse, double largeur, double hauteur)
            {
                X = x;
                Y = y;
                Vie = vie;
                Attaque = attaque;
                Vitesse = vitesse;
                Largeur = largeur;
                Hauteur = hauteur;
            }
            
            public virtual void Deplacer(double deltaX, double deltaY)
            {
                X += deltaX * Vitesse;
                Y += deltaY * Vitesse;
            }

            public void RecevoirDegats(int degats)
            {
                Vie -= degats;
                if (Vie < 0) Vie = 0;
            }

            public void Attaquer(Entite cible)
            {
                cible.RecevoirDegats(Attaque);
            }

            public bool EstMort
            {
                get { return Vie <= 0; }
            }
            public class Aventurier : Entite
            {
                
               


                public Aventurier(double x, double y)
                    : base(x, y, 100, 10, 5, 64, 64)
                {
                    X = x;
                    Y = y;
                }

                public void Sauter(double hauteurSaut)
                {
                    Y -= hauteurSaut;
                }

                

                public void Deplacement(double deltaX, double deltaY)
                {
                   if (deltaX != 0 || deltaY != 0)
                    {
                        //Etat = EtatAnimation.Marche;
                    }
                   else
                    {
                        //Etat = EtatAnimation.Immobile;
                    }


                    base.Deplacer(deltaX, deltaY);
                }

                public void Attaquer()
                {
                   /* if (Etat != EtatAnimation.Attaque)
                        Etat = EtatAnimation.Attaque;*/
                }

                public void FinAttaque()
                {
                    //Etat = EtatAnimation.Immobile;
                }


                public class Ennemi : Entite
                {
                    public Ennemi(double x, double y, int vie, int attaque, double vitesse, double largeur, double hauteur)
                        : base(x, y, vie, attaque, vitesse, largeur, hauteur)
                    {
                        X = x;
                        Y = y;
                    }
                }

               
            }

        }
    }
}
