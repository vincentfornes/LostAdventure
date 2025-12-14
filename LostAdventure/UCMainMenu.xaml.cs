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
		public UCMainMenu()
		{
			InitializeComponent();
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
			MessageBox.Show("Touches du jeu (à venir)");
		}

		private void butQuitter_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
	}
}

