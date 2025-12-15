using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace LostAdventure
{
    /// <summary>
    /// Logique d'interaction pour UCJeu.xaml
    /// </summary>
    
    public partial class UCJeu : UserControl
    {
        private BitmapImage[] spriteAventurier = new BitmapImage[8];
        private int nb = 0;
        public Image[] spriteEnnemis;
        public Image[] spriteBoss;
        public UCJeu()
        {
            InitializeComponent();
            InitializeSprites();
            
        }

        public void Jeu(object sender, EventArgs e)
        {
            nb++;
            if (nb == spriteAventurier.Length)
                nb = 0;
            Aventurier.Source = spriteAventurier[nb];
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        public void InitializeSprites()
        {
            for (int i = 1; i < spriteAventurier.Length; i++)
                spriteAventurier[i] = new BitmapImage(new Uri($"pack://application:,,,/Img/Aventurier/AventurierMarche({i}).png"));
        }

        
        public void AttacherEvenementsClavier()
        {
          
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.KeyDown += canvasJeu_KeyDown;
                Application.Current.MainWindow.KeyUp += canvasJeu_KeyUp;
                Application.Current.MainWindow.Focus();
            }
        }

        private void canvasJeu_KeyDown(object sender, KeyEventArgs e)
        {
            double positionLeftAventurier = Canvas.GetLeft(Aventurier);

            if (double.IsNaN(positionLeftAventurier))
                positionLeftAventurier = 460;

            if (e.Key == Key.Q)
                Canvas.SetLeft(Aventurier, Canvas.GetLeft(Aventurier) - 10);
                
            
            if (e.Key == Key.D)
                Canvas.SetLeft(Aventurier, Canvas.GetLeft(Aventurier)+10);
            
            if (e.Key == Key.Space)
            {
                while (Canvas.GetTop(Aventurier) > 450)
                {
                    
                    Canvas.SetTop(Aventurier, Canvas.GetTop(Aventurier) - 5);
                }
            }

#if DEBUG
            Console.WriteLine("Position Left Aventurier :" + Canvas.GetLeft(Aventurier));
            #endif
        }

        private void canvasJeu_KeyUp(object sender, KeyEventArgs e)
        {
           if (e.Key == Key.Space)
            {
                while (Canvas.GetTop(Aventurier) < 600)
                {
                    Canvas.SetTop(Aventurier, Canvas.GetTop(Aventurier) + 5);
                }
            }

        }
    }
}