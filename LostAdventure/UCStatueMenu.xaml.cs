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
    /// <summary>
    /// Logique d'interaction pour UCStatueMenu.xaml
    /// </summary>
    public partial class UCStatueMenu : UserControl
    {
        private LostAdventureTest.StatueOptions options;

        public UCStatueMenu(LostAdventureTest.StatueOptions statueOptions)
        {
            InitializeComponent();
            options = statueOptions;
        }


        private void butSoin_Click(object sender, RoutedEventArgs e)
        {
            int healCost = options.GetHealCost?.Invoke() ?? 0;
            int currentHP = options.GetHP?.Invoke() ?? 0;
            int maxHP = options.GetMaxHP?.Invoke() ?? 0;

            if (currentHP >= maxHP)
            {
                MessageBox.Show("Vous avez déja tout vos PV", "Soin", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (options.TrySpendGold?.Invoke(healCost) == true)
            {
                options.HealToMax?.Invoke();
                options.RefreshHud?.Invoke();
                MessageBox.Show($"Soigner au maximum de PV pour {healCost} gold!", "Soin", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Pas assez de gold! Vous avez besoin de {healCost} gold.", "Soin", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void butShop_Click(object sender, RoutedEventArgs e)
        {
            // Show upgrade options
            int damageCost = options.GetDamageCost?.Invoke() ?? 0;
            int maxHpCost = options.GetMaxHpCost?.Invoke() ?? 0;
            int gold = options.GetGold?.Invoke() ?? 0;

            var result = MessageBox.Show(
                $"Your Gold: {gold}\n\n" +
                $"1. Augmente les dégats de (+1) - {damageCost} gold\n" +
                $"2. Augmente les PV de (+5) - {maxHpCost} gold\n\n" +
                $"Click YES pour les dégats, NO pour les PV, CANCEL pour revenir en arrière",
                "Shop",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                
                if (options.TrySpendGold?.Invoke(damageCost) == true)
                {
                    options.UpgradeDamage?.Invoke();
                    options.RefreshHud?.Invoke();
                    MessageBox.Show($"Dégat augmenter pour {damageCost} gold!", "Upgrade", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"Pas assez de gold ! Coût de {damageCost} gold.", "Upgrade", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else if (result == MessageBoxResult.No)
            {
                
                if (options.TrySpendGold?.Invoke(maxHpCost) == true)
                {
                    options.UpgradeMaxHP?.Invoke();
                    options.RefreshHud?.Invoke();
                    MessageBox.Show($"Gain de PV pour {maxHpCost} gold!", "Upgrade", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"Pas assez de gold ! Coût de {maxHpCost} gold.", "Upgrade", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void butPartir_Click(object sender, RoutedEventArgs e)
        {
            options.CloseMenu?.Invoke();
        }
    }
}
