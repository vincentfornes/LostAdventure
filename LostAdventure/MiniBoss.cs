
using System;
using System.Windows.Controls;
using System.Windows.Media;
using LostAdventureTest;

namespace LostAdventureTest
{
	public enum BossState
	{
		Idle,
		WindupCharge,
		Charging,
		Recover,
		AoEWarning,
		AoEActive,
		WindupCharge2,
		Charging2
	}

	public class MiniBoss
	{
		public Image Sprite { get; }
		public SpriteAnimator Animator { get; }

		public double X { get; set; }
		public double Y { get; set; }

		public int MaxHP { get; set; } = 40;
		public int HP { get; set; } = 40;
		public int Damage { get; set; } = 6;
		public int GoldReward { get; set; } = 100;

		public double WalkSpeed { get; set; } = 1.8;
		public double ChargeSpeed { get; set; } = 12.0;

		public bool IsPhase2 { get; private set; } = false;
		public BossState State { get; private set; } = BossState.Idle;
		private DateTime stateStart = DateTime.UtcNow;
		private double vx = 0;
		private double aoeRadius = 180;

		public MiniBoss()
		{
			Sprite = new Image
			{
				Width = 280,
				Height = 280,
				RenderTransformOrigin = new System.Windows.Point(0.5, 0.5),
				RenderTransform = new ScaleTransform(1, 1)
			};
			Animator = new SpriteAnimator(Sprite);
			Animator.DefineSequence("Idle", "pack://application:,,,/Img/Boss/MiniBoss/Idle({0}).png", 4);
			Animator.DefineSequence("Windup", "pack://application:,,,/Img/Boss/MiniBoss/Windup({0}).png", 3);
			Animator.DefineSequence("Charge", "pack://application:,,,/Img/Boss/MiniBoss/Charge({0}).png", 6);
			Animator.DefineSequence("AoE", "pack://application:,,,/Img/Boss/MiniBoss/AoE({0}).png", 5);
			Animator.Play("Idle", fps: 6, loop: true);
		}

		public void SetState(BossState s)
		{
			State = s;
			stateStart = DateTime.UtcNow;

			switch (s)
			{
				case BossState.Idle:
					Animator.Play("Idle", fps: IsPhase2 ? 8 : 6, loop: true);
					vx = 0; break;

				case BossState.WindupCharge:
				case BossState.WindupCharge2:
					Animator.Play("Windup", fps: IsPhase2 ? 8 : 6, loop: true);
					vx = 0; break;

				case BossState.Charging:
				case BossState.Charging2:
					Animator.Play("Charge", fps: IsPhase2 ? 20 : 16, loop: true);
					break;

				case BossState.Recover:
					Animator.Play("Idle", fps: IsPhase2 ? 6 : 4, loop: true);
					vx = 0; break;

				case BossState.AoEWarning:
				case BossState.AoEActive:
					Animator.Play("AoE", fps: IsPhase2 ? 10 : 6, loop: true);
					vx = 0; break;
			}
		}

		public void UpdateAI(double playerX, double groundY, Action<BossState> onStateChange, Action? onPhase2Enter = null)
		{
			if (!IsPhase2 && HP <= MaxHP / 2)
			{
				IsPhase2 = true;
				ChargeSpeed = 15.0;
				WalkSpeed = 2.2;
				Damage = 8;
				aoeRadius = 220;
				onPhase2Enter?.Invoke();
			}

			Animator.Update();
			double elapsed = (DateTime.UtcNow - stateStart).TotalMilliseconds;

			var scale = Sprite.RenderTransform as ScaleTransform;
			if (scale != null) scale.ScaleX = (playerX >= X) ? 1 : -1;

			switch (State)
			{
				case BossState.Idle:
					if (Math.Abs(X - playerX) > 300) vx = (playerX > X) ? WalkSpeed : -WalkSpeed;
					else vx = 0;
					X += vx; Y = groundY;
					if (elapsed > (IsPhase2 ? 900 : 1200))
						onStateChange(IsPhase2 ? BossState.WindupCharge2 : BossState.WindupCharge);
					break;

				case BossState.WindupCharge:
					if (elapsed > 700) onStateChange(BossState.Charging);
					break;

				case BossState.Charging:
					if (elapsed < 60) vx = (playerX > X) ? ChargeSpeed : -ChargeSpeed;
					X += vx; Y = groundY;
					if (elapsed > 500) onStateChange(BossState.Recover);
					break;

				case BossState.Recover:
					if (elapsed > (IsPhase2 ? 500 : 800)) onStateChange(BossState.AoEWarning);
					break;

				case BossState.AoEWarning:
					if (elapsed > (IsPhase2 ? 700 : 1000)) onStateChange(BossState.AoEActive);
					break;

				case BossState.AoEActive:
					if (elapsed > (IsPhase2 ? 900 : 800))
						onStateChange(IsPhase2 ? BossState.WindupCharge2 : BossState.Idle);
					break;

				case BossState.WindupCharge2:
					if (elapsed > 450) onStateChange(BossState.Charging2);
					break;

				case BossState.Charging2:
					if (elapsed < 50) vx = (playerX > X) ? ChargeSpeed : -ChargeSpeed;
					X += vx; Y = groundY;
					if (elapsed > 450) onStateChange(BossState.Recover);
					break;
			}
		}

		public double GetAoERadius() => aoeRadius;
	}
}
