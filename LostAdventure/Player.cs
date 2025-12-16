using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LostAdventureTest
{
	public enum PlayerState
	{
		Idle,
		Walking,
		Attacking,
		Dodging
	}

	public class Player
	{
		public Image Sprite { get; }
		public SpriteAnimator Animator { get; }

		public double X { get; set; }
		public double Y { get; set; }

		private double velocityX = 0;
		private double velocityY = 0;
		private const double WALK_SPEED = 8.0;
		private const double JUMP_FORCE = -15.0;
		private const double GRAVITY = 0.8;
		private const double GROUND_Y = 600;
		private bool isGrounded = true;

		public int MaxHP { get; set; } = 30;
		public int HP { get; set; } = 30;
		public int Damage { get; set; } = 2;
		public int Gold { get; set; } = 0;

		public PlayerState State { get; private set; } = PlayerState.Idle;
		private bool facingRight = true;
		private DateTime attackStartTime;
		private const double ATTACK_DURATION_MS = 400;

		private DateTime dodgeStartTime;
		private const double DODGE_DURATION_MS = 250;
		private const double DODGE_SPEED = 20.0;
		private const double DODGE_COOLDOWN_MS = 500;
		private DateTime lastDodgeTime = DateTime.MinValue;

		private DateTime lastDamageTime = DateTime.MinValue;
		private const double INVULNERABILITY_MS = 500;

		private bool moveLeftPressed = false;
		private bool moveRightPressed = false;

		public Player()
		{
			Sprite = new Image
			{
				Width = 210,
				Height = 240,
				RenderTransformOrigin = new Point(0.5, 0.5),
				RenderTransform = new ScaleTransform(1, 1)
			};

			Animator = new SpriteAnimator(Sprite);

			Animator.DefineSequence("WalkRight", "pack://application:,,,/Img/Aventurier/AventurierMarche({0}).png", 8);
			Animator.DefineSequence("Idle", "pack://application:,,,/Img/Aventurier/AventurierMarche(1).png", 1);
			Animator.DefineSequence("Attack", "pack://application:,,,/Img/Aventurier/AventurierAttaque({0}).png", 6);

			Animator.Play("Idle", fps: 10, loop: true);

			X = 460;
			Y = GROUND_Y;
		}

		public void HandleKeyDown(Key key)
		{
			if (State == PlayerState.Attacking) return;

			if (key == Key.Q || key == Key.A)
				moveLeftPressed = true;
			if (key == Key.D)
				moveRightPressed = true;
		}

		public void HandleKeyUp(Key key)
		{
			if (key == Key.Q || key == Key.A)
				moveLeftPressed = false;
			if (key == Key.D)
				moveRightPressed = false;
		}

		public void StartAttack()
		{
			if (State == PlayerState.Attacking || State == PlayerState.Dodging) return;

			State = PlayerState.Attacking;
			attackStartTime = DateTime.UtcNow;

			Animator.Play("Attack", fps: 20, loop: false, onComplete: () =>
			{
				if (State == PlayerState.Attacking)
					State = PlayerState.Idle;
			});
		}

		public void Jump()
		{
			if (!isGrounded || State == PlayerState.Dodging) return;

			velocityY = JUMP_FORCE;
			isGrounded = false;
		}

		public void StartDodge()
		{
			if (State == PlayerState.Dodging) return;

			var timeSinceLastDodge = (DateTime.UtcNow - lastDodgeTime).TotalMilliseconds;
			if (timeSinceLastDodge < DODGE_COOLDOWN_MS) return;

			State = PlayerState.Dodging;
			dodgeStartTime = DateTime.UtcNow;
			lastDodgeTime = DateTime.UtcNow;
		}

		public void Update()
		{
			Animator.Update();

			// Statue d'esquive
			if (State == PlayerState.Dodging)
			{
				var elapsed = (DateTime.UtcNow - dodgeStartTime).TotalMilliseconds;
				if (elapsed >= DODGE_DURATION_MS)
				{
					State = PlayerState.Idle;
				}
				else
				{
					// vitesse du dash et direction
					velocityX = facingRight ? DODGE_SPEED : -DODGE_SPEED;
					X += velocityX;
				}
			}
			// Statue d'attaque
			else if (State == PlayerState.Attacking)
			{
				var elapsed = (DateTime.UtcNow - attackStartTime).TotalMilliseconds;
				if (elapsed >= ATTACK_DURATION_MS)
				{
					State = PlayerState.Idle;
				}
				// attaquer en mouvement
				velocityX = 0;
				if (moveLeftPressed)
				{
					velocityX = -WALK_SPEED;
					facingRight = false;
				}
				else if (moveRightPressed)
				{
					velocityX = WALK_SPEED;
					facingRight = true;
				}
				X += velocityX;
			}
			// mouvement normal
			else
			{
				velocityX = 0;

				if (moveLeftPressed)
				{
					velocityX = -WALK_SPEED;
					facingRight = false;
				}
				else if (moveRightPressed)
				{
					velocityX = WALK_SPEED;
					facingRight = true;
				}

				X += velocityX;

				// Update l'animation en fonction des mouvements
				if (velocityX != 0 && isGrounded)
				{
					if (State != PlayerState.Walking)
					{
						State = PlayerState.Walking;
						Animator.Play("WalkRight", fps: 10, loop: true);
					}
				}
				else if (isGrounded)
				{
					if (State == PlayerState.Walking)
					{
						State = PlayerState.Idle;
						Animator.Play("Idle", fps: 10, loop: true);
					}
				}
			}

			// gravité pour les sauts
			if (!isGrounded)
			{
				velocityY += GRAVITY;
			}

			Y += velocityY;

			// collision avec le sol
			if (Y >= GROUND_Y)
			{
				Y = GROUND_Y;
				velocityY = 0;
				isGrounded = true;
			}

			// Update sprite direction
			var scale = Sprite.RenderTransform as ScaleTransform;
			if (scale != null)
				scale.ScaleX = facingRight ? 1 : -1;
		}

		public Rect GetHitbox()
		{
			return new Rect(X, Y, Sprite.Width, Sprite.Height);
		}

		public Rect GetAttackHitbox()
		{
			if (State != PlayerState.Attacking)
				return Rect.Empty;

			double attackX = facingRight ? X + 100 : X - 80;
			return new Rect(attackX, Y, 80, 100);
		}

	public bool TakeDamage(int damage)
	{
		var timeSinceLastDamage = (DateTime.UtcNow - lastDamageTime).TotalMilliseconds;
		if (timeSinceLastDamage < INVULNERABILITY_MS)
		{
			return false; // toujours invulnerable
		}

		HP -= damage;
		if (HP < 0) HP = 0;
		lastDamageTime = DateTime.UtcNow;
		return true;
	}

		public bool IsAlive => HP > 0;

	public bool IsInvulnerable
	{
		get
		{
			var timeSinceLastDamage = (DateTime.UtcNow - lastDamageTime).TotalMilliseconds;
			return timeSinceLastDamage < INVULNERABILITY_MS;
		}
	}
	}
}
