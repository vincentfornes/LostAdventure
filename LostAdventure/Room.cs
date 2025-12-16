using System;
using System.Collections.Generic;
using System.Windows;

namespace LostAdventureTest
{
	public class Room
	{
		public string BackgroundUri { get; set; }
		public List<Point> SpawnPoints { get; } = new();

		public int GoblinCount { get; set; }
		public int BruteCount { get; set; }	
        public int BossCount { get; set; }

		public bool HasStatue { get; set; } = false;
		public bool CoffrePresent { get; set; } = false;	
        public Point StatuePoint { get; set; } = new Point(1200, 600);

		public Room? LeftRoom { get; set; }
		public Room? RightRoom { get; set; }
		public string RoomId { get; set; }

		public bool CanGoLeft => LeftRoom != null;
		public bool CanGoRight => RightRoom != null;

		public Room(string backgroundUri, string roomId = "")
		{
			BackgroundUri = backgroundUri;
			RoomId = roomId;
		}

		public List<Enemy> SpawnEnemies()
		{
			var enemies = new List<Enemy>();
			var random = new Random();

			int totalEnemies = GoblinCount + BruteCount + BossCount;
			if (totalEnemies == 0) return enemies;

			if (SpawnPoints.Count == 0)
			{
				for (int i = 0; i < totalEnemies; i++)
				{
					SpawnPoints.Add(new Point(300 + i * 200, 600));
				}
			}

			int spawnIndex = 0;


			for (int i = 0; i < GoblinCount; i++)
			{
				var spawnPoint = SpawnPoints[spawnIndex % SpawnPoints.Count];
				double offsetX = random.Next(-20, 21);
				double goblinY = 550;
				enemies.Add(new Enemy(EnemyType.Goblin, spawnPoint.X + offsetX, goblinY));
				spawnIndex++;
			}

			for (int i = 0; i < BruteCount; i++)
			{
				var spawnPoint = SpawnPoints[spawnIndex % SpawnPoints.Count];
				double offsetX = random.Next(-20, 21);
				double bruteY = 500;
				enemies.Add(new Enemy(EnemyType.Brute, spawnPoint.X + offsetX, bruteY));
				spawnIndex++;
            }

            for (int i = 0; i < BossCount; i++)
			{
				var spawnPoint = SpawnPoints[spawnIndex % SpawnPoints.Count];
				double offsetX = random.Next(-20, 21);
				double bossY = 350;
				enemies.Add(new Enemy(EnemyType.Boss, spawnPoint.X + offsetX, bossY));
				spawnIndex++;
			}

				return enemies;
		}
	}
}
