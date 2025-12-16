using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LostAdventureTest;

namespace LostAdventure
{
	public partial class UCJeu : UserControl
	{
		private Player player;
		private RoomManager roomManager;
		private List<Enemy> activeEnemies = new List<Enemy>();
		private System.Windows.Threading.DispatcherTimer gameTimer;
		private Image backgroundImage;
		private Image statueImage;
		private TextBlock interactionText;
		private bool isNearStatue = false;
		private HashSet<Enemy> enemiesHitThisAttack = new HashSet<Enemy>();
		private bool wasAttacking = false;

		public UCJeu()
		{
			InitializeComponent();
			InitializeGame();
			InitializeGameTimer();
		}

		private void InitializeGame()
		{
			player = new Player();
			JeuCanvas.Children.Add(player.Sprite);
			Canvas.SetLeft(player.Sprite, player.X);
			Canvas.SetTop(player.Sprite, player.Y);

			roomManager = new RoomManager();
			roomManager.InitializeRooms();

			LoadRoom(roomManager.CurrentRoom);

			UpdateHUD();
		}

		private void LoadRoom(Room room)
		{
			// Remove old enemies and their health bars
			foreach (var enemy in activeEnemies)
			{
				JeuCanvas.Children.Remove(enemy.Sprite);
				if (enemy.HealthBarContainer != null)
					JeuCanvas.Children.Remove(enemy.HealthBarContainer);
			}
			activeEnemies.Clear();

			// Set background
			if (backgroundImage == null)
			{
				backgroundImage = new Image
				{
					Stretch = Stretch.UniformToFill
				};
				JeuCanvas.Children.Add(backgroundImage);
				Panel.SetZIndex(backgroundImage, -1000);
			}

			backgroundImage.Source = new BitmapImage(new Uri(room.BackgroundUri));
			Canvas.SetLeft(backgroundImage, 0);
			Canvas.SetTop(backgroundImage, 0);
			backgroundImage.Width = JeuCanvas.ActualWidth > 0 ? JeuCanvas.ActualWidth : 1600;
			backgroundImage.Height = JeuCanvas.ActualHeight > 0 ? JeuCanvas.ActualHeight : 900;

			// Handle statue
			if (room.HasStatue)
			{
				if (statueImage == null)
				{
					statueImage = new Image
					{
						Width = 200,
						Height = 300,
						Source = new BitmapImage(new Uri("pack://application:,,,/Img/Statue/Statue.png"))
					};
					JeuCanvas.Children.Add(statueImage);

					interactionText = new TextBlock
					{
						Text = "Appuyez sur E pour intéragir",
						Foreground = Brushes.White,
						FontSize = 18,
						FontWeight = FontWeights.Bold,
						Background = new SolidColorBrush(Color.FromArgb(180, 0, 0, 0)),
						Padding = new Thickness(10),
						Visibility = Visibility.Collapsed
					};
					JeuCanvas.Children.Add(interactionText);
					Panel.SetZIndex(interactionText, 100);
				}

				statueImage.Visibility = Visibility.Visible;
				Canvas.SetLeft(statueImage, room.StatuePoint.X);
				Canvas.SetTop(statueImage, room.StatuePoint.Y);
			}
			else if (statueImage != null)
			{
				statueImage.Visibility = Visibility.Collapsed;
			}

			// spawn les enemies
			activeEnemies = room.SpawnEnemies();
			foreach (var enemy in activeEnemies)
			{
				JeuCanvas.Children.Add(enemy.Sprite);
				Canvas.SetLeft(enemy.Sprite, enemy.X);
				Canvas.SetTop(enemy.Sprite, enemy.Y);

				// ajoute une barre de vie aux enemies
				enemy.HealthBarContainer = new Border
				{
					Width = 60,
					Height = 6,
					Background = Brushes.DarkRed,
					BorderBrush = Brushes.Black,
					BorderThickness = new Thickness(1)
				};

				enemy.HealthBarFill = new Border
				{
					Width = 60,
					Height = 6,
					Background = Brushes.Red,
					HorizontalAlignment = HorizontalAlignment.Left
				};

				enemy.HealthBarContainer.Child = enemy.HealthBarFill;
				JeuCanvas.Children.Add(enemy.HealthBarContainer);
				Panel.SetZIndex(enemy.HealthBarContainer, 100);

				// centre la barre de vie pour les gobelins
				double initialHealthBarX = enemy.Type == EnemyType.Boss ? enemy.X + 270 : (enemy.Type == EnemyType.Goblin ? enemy.X + 170 : enemy.X + 20);
				Canvas.SetLeft(enemy.HealthBarContainer, initialHealthBarX);
				Canvas.SetTop(enemy.HealthBarContainer, enemy.Y - 10);
			}
		}

		private void InitializeGameTimer()
		{
			gameTimer = new System.Windows.Threading.DispatcherTimer();
			gameTimer.Tick += Jeu;
			gameTimer.Interval = TimeSpan.FromMilliseconds(16);
			gameTimer.Start();
		}

		public void Jeu(object sender, EventArgs e)
		{
			player.Update();
		// ajoute des limites à gauche de la salle 1 et a droite de la salle du boss
		const double LEFT_BOUNDARY = 0;
		double rightBoundary = JeuCanvas.ActualWidth - player.Sprite.Width;

		if (roomManager.CurrentRoom.RoomId == "entrance" && player.X < LEFT_BOUNDARY)
		{
			player.X = LEFT_BOUNDARY;
		}
		else if (roomManager.CurrentRoom.RoomId == "room4" && player.X > rightBoundary)
		{
			player.X = rightBoundary;
		}
			Canvas.SetLeft(player.Sprite, player.X);
			Canvas.SetTop(player.Sprite, player.Y);

			// Check si  le joueur est mort
			if (player.HP <= 0)
			{
				gameTimer.Stop();
				var gameOverScreen = new UCGameOver();
				RootGrid.Children.Add(gameOverScreen);
				Panel.SetZIndex(gameOverScreen, 2000);
				return;
			}

			// Track attack state to clear hit list when new attack starts
			if (player.State == PlayerState.Attacking && !wasAttacking)
			{
				// New attack started - clear the list of hit enemies
				enemiesHitThisAttack.Clear();
			}
			else if (player.State != PlayerState.Attacking && wasAttacking)
			{
				// Attack ended - clear the list
				enemiesHitThisAttack.Clear();
			}
			wasAttacking = player.State == PlayerState.Attacking;

		// Visuel pour l'invunerabilité du joueur après un coup subit
		if (player.IsInvulnerable)
		{
			player.Sprite.Opacity = (DateTime.UtcNow.Millisecond / 100) % 2 == 0 ? 0.5 : 1.0;
		}
		else
		{
			player.Sprite.Opacity = 1.0;
		}


			var newRoom = roomManager.CheckTransition(player, JeuCanvas.ActualWidth);
			if (newRoom != null)
			{
				if (player.X < 50)
					player.X = JeuCanvas.ActualWidth - 100;
				else
					player.X = 100;

				LoadRoom(newRoom);
			}

			foreach (var enemy in activeEnemies.ToList())
			{
				// les enemies avance vers le joueur
				if (enemy.X < player.X)
					enemy.X += enemy.Speed;
				else
					enemy.X -= enemy.Speed;

				Canvas.SetLeft(enemy.Sprite, enemy.X);

				// Update health bar position and width
				if (enemy.HealthBarContainer != null && enemy.HealthBarFill != null)
				{
					// centre la barre de vie au dessus de l'enemie
				double healthBarX = enemy.Type == EnemyType.Boss ? enemy.X + 270 : (enemy.Type == EnemyType.Goblin ? enemy.X + 170 : enemy.X + 20);
					double healthBarY = enemy.Type == EnemyType.Goblin ? enemy.Y - 10 : enemy.Y - 10;

					Canvas.SetLeft(enemy.HealthBarContainer, healthBarX);
					Canvas.SetTop(enemy.HealthBarContainer, healthBarY);

					double healthPercent = (double)enemy.HP / enemy.MaxHP;
					enemy.HealthBarFill.Width = 60 * healthPercent;
				}

				// Joueur attacque l'enemies
				if (player.State == PlayerState.Attacking)
				{
					var attackBox = player.GetAttackHitbox();
					var enemyBox = enemy.GetHitbox();
					if (attackBox.IntersectsWith(enemyBox))
					{
						// une attaque ne fait que les dégats d'une attaque
						if (!enemiesHitThisAttack.Contains(enemy))
						{
							enemiesHitThisAttack.Add(enemy);

							// les gobelins donne de l'or a chaque coup
							if (enemy.GoldPerHit > 0)
							{
								player.Gold += enemy.GoldPerHit;
							}

							enemy.HP -= player.Damage;
							if (!enemy.IsAlive)
							{
								JeuCanvas.Children.Remove(enemy.Sprite);
								if (enemy.HealthBarContainer != null)
									JeuCanvas.Children.Remove(enemy.HealthBarContainer);
								activeEnemies.Remove(enemy);
								player.Gold += enemy.GoldReward;
						// regarde si le boss est en vie et affiche le screen de la victoir
						if (enemy.Type == EnemyType.Boss)
						{
							gameTimer.Stop();
							var victoryScreen = new UCVictory();
							RootGrid.Children.Add(victoryScreen);
							Panel.SetZIndex(victoryScreen, 2000);
							return;
						}
							}
						}
					}
				}

				// Enemies attaque le joueur (detection des colisions)
				var playerBox = player.GetHitbox();
				var enemyAttackBox = enemy.GetAttackHitbox();
				if (playerBox.IntersectsWith(enemyAttackBox) && enemy.Damage > 0)
				{
					player.TakeDamage(enemy.Damage);
					// repousse l'enemie légerement apres une attaque
					if (enemy.X < player.X)
						enemy.X -= 20;
					else
						enemy.X += 20;
				}
			}

			// check la proximité avec la statue
			if (roomManager.CurrentRoom.HasStatue && statueImage != null)
			{
				double distanceToStatue = Math.Abs(player.X - roomManager.CurrentRoom.StatuePoint.X);
				isNearStatue = distanceToStatue < 150;

				if (isNearStatue && interactionText != null)
				{
					interactionText.Visibility = Visibility.Visible;
					Canvas.SetLeft(interactionText, roomManager.CurrentRoom.StatuePoint.X - 50);
					Canvas.SetTop(interactionText, roomManager.CurrentRoom.StatuePoint.Y - 40);
				}
				else if (interactionText != null)
				{
					interactionText.Visibility = Visibility.Collapsed;
				}
			}

			UpdateHUD();
		}

		private void UpdateHUD()
		{
			TxtHP.Text = $"{player.HP}/{player.MaxHP}";
			double hpPercent = (double)player.HP / player.MaxHP;
			HpFill.Width = 220 * hpPercent;

			TxtGold.Text = player.Gold.ToString();
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			Application.Current.MainWindow.KeyDown += canvasJeu_KeyDown;
			Application.Current.MainWindow.KeyUp += canvasJeu_KeyUp;
			this.Focus();
		}

		private void canvasJeu_KeyDown(object sender, KeyEventArgs e)
		{
			player.HandleKeyDown(e.Key);

			if (e.Key == Key.Space)
				player.Jump();
			else if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
				player.StartDodge();
			else if (e.Key == Key.E && isNearStatue)
				OpenStatueMenu();
		}

		private void canvasJeu_KeyUp(object sender, KeyEventArgs e)
		{
			player.HandleKeyUp(e.Key);
		}

		private void JeuCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			player.StartAttack();
		}

		private void OpenStatueMenu()
		{
			gameTimer.Stop();

			var statueOptions = new LostAdventureTest.StatueOptions
			{
				GetGold = () => player.Gold,
				GetHP = () => player.HP,
				GetMaxHP = () => player.MaxHP,
				GetHealCost = () => 5,
				GetDamageCost = () => 10,
				GetMaxHpCost = () => 15,

				TrySpendGold = (amount) =>
				{
					if (player.Gold >= amount)
					{
						player.Gold -= amount;
						return true;
					}
					return false;
				},

				HealToMax = () => player.HP = player.MaxHP,
				UpgradeDamage = () => player.Damage += 1,
				UpgradeMaxHP = () =>
				{
					player.MaxHP += 5;
					player.HP += 5;
				},

				RefreshHud = UpdateHUD,
				CloseMenu = CloseStatueMenu
			};

			var statueMenu = new UCStatueMenu(statueOptions);
			RootGrid.Children.Add(statueMenu);
			Panel.SetZIndex(statueMenu, 1000);
		}

		private void CloseStatueMenu()
		{
			var statueMenu = RootGrid.Children.OfType<UCStatueMenu>().FirstOrDefault();
			if (statueMenu != null)
			{
				RootGrid.Children.Remove(statueMenu);
			}
			gameTimer.Start();
		}
	}
}
