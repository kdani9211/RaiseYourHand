using RaiseYourHand.Helpers;
using System;
using System.Collections.Generic;

namespace RaiseYourHand
{
	public class RoomManager : Singleton<RoomManager>
	{
		private readonly Dictionary<string, Room> keyToRoom = new Dictionary<string, Room>();
		private readonly Dictionary<Room, string> roomToKey = new Dictionary<Room, string>();
		public Room this[string roomName]
		{
			get
			{
				if (roomName.IsNullOrEmpty())
					roomName = String.Empty;
				roomName = roomName.Trim().ToLower();
				lock (this.keyToRoom)
					lock (this.roomToKey)
					{
						if (this.keyToRoom.ContainsKey(roomName))
							return this.keyToRoom[roomName];
						else
						{
							Room newRoom = new Room();
							this.keyToRoom[roomName] = newRoom;
							this.roomToKey[newRoom] = roomName;
							return newRoom;
						}
					}
			}
		}
		public string this[Room room]
		{
			get
			{
				this.roomToKey.TryGetValue(room, out string name);
				return name;
			}
		}
	}
}