using System.Media;
using System.Windows;
using System.Windows.Media;

namespace LostAdventure
{
	public partial class MainWindow : Window
	{
        private MediaPlayer sonTest = new MediaPlayer();
        public static double nivSon = 50;
        private static MediaPlayer musique;

        public MainWindow()
		{
			InitializeComponent();
			AfficheMainMenu();
            InitialiseMusique();

        }

		private void InitialiseMusique()
		{
			musique = new MediaPlayer();
			musique.Open(new System.Uri("Sons/Musique.mp3", System.UriKind.Relative));
			musique.MediaEnded += RelanceMusique;
			musique.Volume = nivSon / 100.0 ;
			musique.Play();
			Console.WriteLine("Musique lancée");
        }

        public void SetVolumeMusique(double volume)
        {
            if (musique != null)
            {
                nivSon = volume;
                musique.Volume = volume / 100.0; // ✅ Convertir le pourcentage en volume (0-1)
                Console.WriteLine($"Volume changé: {musique.Volume}");
            }
        }

        private void RelanceMusique(object? sender, EventArgs e)
        {
            musique.Position = TimeSpan.Zero;
            musique.Play();
        }

        public static void SetVolumeMusique()
        {
            if (musique != null)
                musique.Volume = nivSon;
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
