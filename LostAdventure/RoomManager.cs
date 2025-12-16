using System;
using System.Collections.Generic;

namespace LostAdventureTest
{
	public class RoomManager
	{
		private Room currentRoom;
		private Dictionary<string, Room> allRooms;
		private Random random = new Random();
		private string[] backgrounds = new[]
		{
			"pack://application:,,,/Img/SalleFond/Salle1.PNG",
			"pack://application:,,,/Img/SalleFond/Salle2.PNG",
			"pack://application:,,,/Img/SalleFond/Salle3.PNG",
			"pack://application:,,,/Img/SalleFond/Salle4.PNG",
			"pack://application:,,,/Img/SalleFond/Salle5.PNG",
            "pack://application:,,,/Img/SalleFond/Salle6.PNG"
        };

		public Room CurrentRoom => currentRoom;

		public RoomManager()
		{
			allRooms = new Dictionary<string, Room>();
		}

		private void ShuffleBackgrounds()
		{
			// random background entre les 6
			for (int i = backgrounds.Length-1 ; i > 0; i--)
			{
				int j = random.Next(i + 1);
				string temp = backgrounds[i];
				backgrounds[i] = backgrounds[j];
				backgrounds[j] = temp;
			}
		}

		public void InitializeRooms()
		{
			// une image par salle
			ShuffleBackgrounds();

			var room1 = new Room(backgrounds[0], "entrance")
			{
				GoblinCount = 3,
				HasStatue = true
            };

			var room2 = new Room(backgrounds[1], "room2")
			{
				GoblinCount = 5,
				
				HasStatue = true
            };

			var room3 = new Room(backgrounds[2], "room3")
			{
				GoblinCount = 4,
				
                HasStatue = true
			};

			var room4 = new Room(backgrounds[3], "room4")
			{
				GoblinCount = 0,
				BruteCount = 1,
                HasStatue = true

            };

			var room5 = new Room(backgrounds[4], "room5")
			{
				BruteCount = 0,
				BossCount = 1,
				HasStatue = true
			};

			var room6 = new Room(backgrounds[5], "room6")
			{
				BossCount = 0,
				HasStatue = false,
                CoffrePresent = true
            };


            room1.RightRoom = room2;
			room2.LeftRoom = room1;
			room2.RightRoom = room3;
			room3.LeftRoom = room2;
			room3.RightRoom = room4;
			room4.LeftRoom = room3;
			room4.RightRoom = room5;
			room5.LeftRoom = room4;
			room5.RightRoom = room6;
			room6.LeftRoom = room5;
            allRooms["entrance"] = room1;
			allRooms["room2"] = room2;
			allRooms["room3"] = room3;
			allRooms["room4"] = room4;
			allRooms["room5"] = room5;
			allRooms["room6"] = room6;
            currentRoom = room1;
		}

		public Room? CheckTransition(Player player, double canvasWidth)
		{
			const double EDGE_THRESHOLD = 50;

			if (player.X < EDGE_THRESHOLD && currentRoom.CanGoLeft)
			{
				currentRoom = currentRoom.LeftRoom;
				return currentRoom;
			}
			else if (player.X > canvasWidth - EDGE_THRESHOLD && currentRoom.CanGoRight)
			{
				currentRoom = currentRoom.RightRoom;
				return currentRoom;
			}

			return null;
		}
	}
}
