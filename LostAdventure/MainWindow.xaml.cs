using System.Media;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;

namespace LostAdventure
{
	public partial class MainWindow : Window
	{
        private static MediaPlayer musique = new MediaPlayer();

        

        public MainWindow()
		{
			InitializeComponent();
			AfficheMainMenu();
            InitMusique(); 
            JouerMusique();

        }


        private void InitMusique()
        {
            musique.Open(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Sons/Musique.wav")));
            
            
            musique.MediaEnded += RelanceMusique;
            musique.Volume = 1.0; // volume par défaut
            musique.Play();
        }

        private void RelanceMusique(object? sender, EventArgs e)
        {
            musique.Position = TimeSpan.Zero;
            musique.Play();
        }

        private void JouerMusique()
        {
            musique.Open(new Uri(@"C:\Users\TonNom\source\repos\SAE101\LostAdventure\Sons\Musique.wav", UriKind.Absolute));
            musique.MediaEnded += (s, e) =>
            {
                musique.Position = TimeSpan.Zero;
                musique.Play();
            };
            musique.Volume = 1.0;
            musique.Play();
        }

        // ✅ Méthode publique pour changer le volume
        public void SetVolume(double volume)
        {
            musique.Volume = volume / 100.0; // slider 0-100 → volume 0-1
        }




        public void AfficheMainMenu()
		{
			ZoneDeJeu.Content = new UCMainMenu();
		}

		public void AfficheReglesJeu()
		{
			ZoneDeJeu.Content = new UCReglesJeu();
		}

		public void AfficheTouches()
		{
			ZoneDeJeu.Content = new UCTouches();
		}

		public void AfficheJeu()
		{
			ZoneDeJeu.Content = new UCJeu();
		}
		public void AfficheParametres()
		{
			ZoneDeJeu.Content = new UCParamètres();
        }

        
    }
}
