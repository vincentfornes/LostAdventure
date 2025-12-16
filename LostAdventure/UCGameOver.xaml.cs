using System.Windows;
using System.Windows.Controls;

namespace LostAdventure
{
	public partial class UCGameOver : UserControl
	{
		public UCGameOver()
		{
			InitializeComponent();
		}

		private void butRestart_Click(object sender, RoutedEventArgs e)
		{
			// Restart le jeux
			var mainWindow = Application.Current.MainWindow as MainWindow;
			mainWindow?.AfficheJeu();
		}

		private void butLeave_Click(object sender, RoutedEventArgs e)
		{
			// Retourne au Menu principale
			var mainWindow = Application.Current.MainWindow as MainWindow;
			mainWindow?.AfficheMainMenu();
		}
	}
}
