
using System;

namespace LostAdventureTest
{
	public class StatueOptions
	{
		public Func<int>? GetGold { get; set; }
		public Func<int>? GetHP { get; set; }
		public Func<int>? GetMaxHP { get; set; }

		public Func<int>? GetHealCost { get; set; }
		public Func<int>? GetDamageCost { get; set; }
		public Func<int>? GetMaxHpCost { get; set; }

		public Func<int, bool>? TrySpendGold { get; set; }
		public Action? HealToMax { get; set; }
		public Action? UpgradeDamage { get; set; }
		public Action? UpgradeMaxHP { get; set; }

		public Action? RefreshHud { get; set; }
		public Action? CloseMenu { get; set; }
	}
}

