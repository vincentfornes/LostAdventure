using System;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LostAdventureTest
{
	public class Enemy
	{
		public EnemyType Type { get; set; }
		
		

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

		public bool JoueurDansPortee(Player player)
		{
			Rect enemyHitbox = GetHitbox();
			Rect playerHitbox = new Rect(player.X, player.Y, player.Sprite.Width, player.Sprite.Height);
			return enemyHitbox.IntersectsWith(playerHitbox);
        }

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

                    try
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri("pack://application:,,,/Img/Brute/BruteImmobile.png", UriKind.Absolute);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        Sprite.Source = bitmap;
                    }
                    catch
                    {
                        // L'image ne s'est pas chargé
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
