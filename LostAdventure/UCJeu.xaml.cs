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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace LostAdventure
{
    /// <summary>
    /// Logique d'interaction pour UCJeu.xaml
    /// </summary>
    
    public partial class UCJeu : UserControl
    {
        private BitmapImage[] aventurier = new BitmapImage[8];
        private int nb = 0;
        public Image[] spriteEnnemis;
        public Image[] spriteBoss;
        public UCJeu()
        {
            InitializeComponent();
        }

        public void Jeu(object sender, EventArgs e)
        {
            nb++;
            if (nb == aventurier.Length)
                nb = 0;
            Aventurier.Source = aventurier[nb];
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            Application.Current.MainWindow.KeyDown += canvasJeu_KeyDown;
            Application.Current.MainWindow.KeyUp += canvasJeu_KeyUp;

        }

        public void InitializeSprites()
        {
            for (int i = 1; i < aventurier.Length; i++)
                aventurier[i] = new BitmapImage(new Uri($"pack://application:,,,/Img/Aventurier/AventurierMarche({i}).png"));
        }

        private void canvasJeu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
                Canvas.SetLeft(Aventurier, Canvas.GetLeft(Aventurier)+2);
            if (e.Key == Key.Right)
                Canvas.SetLeft(Aventurier, Canvas.GetLeft(Aventurier)-2);

        }

        private void canvasJeu_KeyUp(object sender, KeyEventArgs e)
        {
           
           
        }
    }
}