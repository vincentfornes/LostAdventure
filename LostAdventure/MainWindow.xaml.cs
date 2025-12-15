using System.Windows;

namespace LostAdventure
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			AfficheMainMenu();
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
	}
}
