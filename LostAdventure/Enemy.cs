using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LostAdventureTest
{
	public class Enemy
	{
		public EnemyType Type { get; set; }
		
		public enum EtatBrute
		{		
			Marche,
			Attaque
        }

        public enum EtatBoss
		{
			BouleDeDeu,
            Griffure
        }

        public Image Sprite { get; set; }
		public SpriteAnimator? Animator { get; set; }

		public double X { get; set; }
		public double Y { get; set; }

		public double Speed { get; set; }
		public int HP { get; set; }
		public int MaxHP { get; set; }
		public int Damage { get; set; }
		public int GoldReward { get; set; }
		public int GoldPerHit { get; set; }

		public Border? HealthBarContainer { get; set; }
		public Border? HealthBarFill { get; set; }

		public bool IsAlive => HP > 0;

		public Enemy(EnemyType type, double x, double y)
		{
			Type = type;
			X = x;
			Y = y;

			Sprite = new Image
			{
				Width = 100,
				Height = 100,
				RenderTransformOrigin = new System.Windows.Point(0.5, 0.5),
				RenderTransform = new ScaleTransform(1, 1)
			};

			switch (type)
			{
				case EnemyType.Goblin:
					HP = 10;
					MaxHP = 10;
					Speed = 0.6;
					Damage = 1;
					GoldReward = 3;
					GoldPerHit = 1;
					Sprite.Width = 400;
					Sprite.Height = 400;
					try
					{
						var bitmap = new BitmapImage();
						bitmap.BeginInit();
						bitmap.UriSource = new Uri("pack://application:,,,/Img/Enemies/Gobelin.png", UriKind.Absolute);
						bitmap.CacheOption = BitmapCacheOption.OnLoad;
						bitmap.EndInit();
						Sprite.Source = bitmap;
					}
					catch
					{
						// L'image ne s'est pas chargé
					}
					break;

				case EnemyType.Brute:
					HP = 40;
                    MaxHP = 50;
					Speed = 3.0;
					Damage = 5;
					GoldReward = 30;
					GoldPerHit = 1;
					Sprite.Width = 500;
					Sprite.Height = 500;
					EtatBrute etat = EtatBrute.Immobile;
					Sprite = new Image
					{
						Width = 210,
						Height = 240,
						RenderTransformOrigin = new Point(0.5, 0.5),
						RenderTransform = new ScaleTransform(1, 1)
					};


					Animator = new SpriteAnimator(Sprite);

					Animator.DefineSequence("Immobile", "pack://application:,,,/Img/Brute/BruteImmobile.png", 1);
					Animator.DefineSequence("Marche", "pack://application:,,,/Img/Brute/BruteImmobile.png", 1);
					Animator.DefineSequence("Saut", "pack://application:,,,/Img/Brute/BruteAttaque(4).png", 3);
					Animator.DefineSequence("Attaque", "pack://application:,,,/Img/Brute/BruteAttaque(1).png", 7);
					X = x;
					Y = y;


					switch (etat)
					{
						case EtatBrute.Immobile:
							try
							{
								var bitmap = new BitmapImage();
								bitmap.BeginInit();
								bitmap.UriSource = new Uri("pack://application:,,,/Img/Brute/BruteImmobile.png", UriKind.Absolute);
								Animator.Play("Immobile", fps: 1, loop: true);
								bitmap.CacheOption = BitmapCacheOption.OnLoad;
								bitmap.EndInit();
								Sprite.Source = bitmap;
							}
							catch
							{
								// L'image ne s'est pas chargé
							}
							break;
						case EtatBrute.Marche:
							try
							{
								var bitmap = new BitmapImage();
								bitmap.BeginInit();
								bitmap.UriSource = new Uri("pack://application:,,,/Img/Brute/BruteImmobile.png", UriKind.Absolute);
								Animator.Play("Marche", fps: 5, loop: true);
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
								bitmap.EndInit();
								Sprite.Source = bitmap;

							}
							catch
							{
								// L'image ne s'est pas chargé
							} break;
						case EtatBrute.Saut:
							try
							{
								var bitmap = new BitmapImage();
								bitmap.BeginInit();
								bitmap.UriSource = new Uri("pack://application:,,,/Img/Brute/BruteAttaque(4).png", UriKind.Absolute);
								Animator.Play("Saut", fps: 5, loop: false);
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
								bitmap.EndInit();
							}
							catch
							{

								//L'image ne s'est pas chargé
							}
						break;
						case EtatBrute.Attaque:
							try
							{
								var bitmap = new BitmapImage();
								bitmap.BeginInit();
								bitmap.UriSource = new Uri("pack://application:,,,/Img/Brute/BruteAttaque(1).png", UriKind.Absolute);
								Animator.Play("Attaque", fps: 7, loop: false);
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
								bitmap.EndInit();
								Sprite.Source = bitmap;
							}
							catch
							{
								//L'image ne s'est pas chargé
							}
						break;
			}
				break; 

                case EnemyType.Boss:
					HP = 50;
					MaxHP = 50;
					Speed = 0.4;
					Damage = 5;
					GoldReward = 20;
					GoldPerHit = 2;
					Sprite.Width = 600;
					Sprite.Height = 600;
					try
					{
						var bitmap = new BitmapImage();
						bitmap.BeginInit();
						bitmap.UriSource = new Uri("pack://application:,,,/Img/Enemies/BossDragon.png", UriKind.Absolute);
						bitmap.CacheOption = BitmapCacheOption.OnLoad;
						bitmap.EndInit();
						Sprite.Source = bitmap;
					}
					catch
					{
						//L'image ne s'est pas chargé
					}
					break;
			}
		}

		public Rect GetHitbox()
		{
			// hitbox de l'enemie = taille du sprite
			return new Rect(X, Y, Sprite.Width, Sprite.Height);
		}

	public Rect GetAttackHitbox()
	{
		
		if (Type == EnemyType.Boss)
		{
			
			return new Rect(X + 150, Y + 250, 300, 350);
		}
		else if (Type == EnemyType.Goblin)
		{
			
			return new Rect(X + 100, Y + 250, 200, 150);
		}

		else if (Type == EnemyType.Brute)
			{
				return new Rect(X + 100, Y + 200, 300, 200);
            }


                
                return new Rect(X + 20, Y + 20, 60, 60);
	}
	}
}
