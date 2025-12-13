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

namespace LostAdventure
{
	public partial class UCReglesJeu : UserControl
	{
		public UCReglesJeu()
		{
			InitializeComponent();
		}

		private void butQuitterRegles_Click(object sender, RoutedEventArgs e)
		{
			var main = Application.Current.MainWindow as MainWindow;
			main?.AfficheMainMenu();
		}
	}
}

