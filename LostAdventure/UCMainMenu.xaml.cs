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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LostAdventure
{
	public partial class UCMainMenu : UserControl
	{
        public static MediaPlayer musiqueDeFond = new MediaPlayer();
        public static double nivSon = 50;
        public static double NivSon
        {
            get { return nivSon; }
            set
            {
                nivSon = value;
                // Important : Mettre à jour le volume dès que la valeur est changée
                SetVolumeMusiqueDeFond();
            }
        }
        public UCMainMenu()
		{
			InitializeComponent();
			InitMusiqueDeFond();
        }

		private void butJouer_Click(object sender, RoutedEventArgs e)
		{
			var main = Application.Current.MainWindow as MainWindow;
			main.AfficheJeu();
        }

		private void butRegles_Click(object sender, RoutedEventArgs e)
		{
			var main = Application.Current.MainWindow as MainWindow;
			main.AfficheReglesJeu();
		}

		private void butTouches_Click(object sender, RoutedEventArgs e)
		{
			var main = Application.Current.MainWindow as MainWindow;
			main.AfficheTouches();
		}

		private void butQuitter_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
        private void butParametres_Click(object sender, RoutedEventArgs e)
        {
            var main = Application.Current.MainWindow as MainWindow;
            main.AfficheParametres();
        }


        public void InitMusiqueDeFond()
        {
            try
            {
                musiqueDeFond.Open(new Uri("Sons/MusiquePremièreSalle.mp3", UriKind.Relative));
                musiqueDeFond.Volume = NivSon / 100.0;
                musiqueDeFond.MediaEnded += (s, e) =>
                {
                    musiqueDeFond.Position = TimeSpan.Zero;
                    musiqueDeFond.Play();
                };
                musiqueDeFond.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur musique : " + ex.Message);
            }
        }
        public static void SetVolumeMusiqueDeFond()
        {
            // Le volume de MediaPlayer utilise une échelle de 0.0 à 1.0.
            musiqueDeFond.Volume = NivSon / 100.0;
           
        }




    }
}

