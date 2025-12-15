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
			// Restart the game
			var mainWindow = Application.Current.MainWindow as MainWindow;
			mainWindow?.AfficheJeu();
		}

		private void butLeave_Click(object sender, RoutedEventArgs e)
		{
			// Return to main menu
			var mainWindow = Application.Current.MainWindow as MainWindow;
			mainWindow?.AfficheMainMenu();
		}
	}
}
