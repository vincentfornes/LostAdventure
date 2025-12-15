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

        private void butSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void butSoin_Click(object sender, RoutedEventArgs e)
        {
            // Heal to max HP for 5 gold
            int healCost = options.GetHealCost?.Invoke() ?? 0;
            int currentHP = options.GetHP?.Invoke() ?? 0;
            int maxHP = options.GetMaxHP?.Invoke() ?? 0;

            if (currentHP >= maxHP)
            {
                MessageBox.Show("Your HP is already full!", "Healing", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (options.TrySpendGold?.Invoke(healCost) == true)
            {
                options.HealToMax?.Invoke();
                options.RefreshHud?.Invoke();
                MessageBox.Show($"Healed to full HP for {healCost} gold!", "Healing", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Not enough gold! Healing costs {healCost} gold.", "Healing", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                $"1. Upgrade Damage (+1) - {damageCost} gold\n" +
                $"2. Upgrade Max HP (+5) - {maxHpCost} gold\n\n" +
                $"Click YES for Damage upgrade, NO for Max HP upgrade, CANCEL to go back",
                "Shop",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Buy damage upgrade
                if (options.TrySpendGold?.Invoke(damageCost) == true)
                {
                    options.UpgradeDamage?.Invoke();
                    options.RefreshHud?.Invoke();
                    MessageBox.Show($"Damage upgraded for {damageCost} gold!", "Upgrade", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"Not enough gold! Costs {damageCost} gold.", "Upgrade", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else if (result == MessageBoxResult.No)
            {
                // Buy max HP upgrade
                if (options.TrySpendGold?.Invoke(maxHpCost) == true)
                {
                    options.UpgradeMaxHP?.Invoke();
                    options.RefreshHud?.Invoke();
                    MessageBox.Show($"Max HP upgraded for {maxHpCost} gold!", "Upgrade", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"Not enough gold! Costs {maxHpCost} gold.", "Upgrade", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void butPartir_Click(object sender, RoutedEventArgs e)
        {
            options.CloseMenu?.Invoke();
        }
    }
}
