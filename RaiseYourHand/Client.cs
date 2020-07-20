using System;
using System.Collections.Generic;

namespace RaiseYourHand
{
	public abstract class Client : IEquatable<Client>
	{
		public Room Room { get; }

		public Client(Room room) =>
			this.Room = room;
		public override bool Equals(object obj) =>
			this.Equals(obj as Client);
		public bool Equals(Client other) =>
			other != null &&
			EqualityComparer<Room>.Default.Equals(this.Room, other.Room);
		public override int GetHashCode() =>
			-1865389910
				+ EqualityComparer<Room>.Default.GetHashCode(this.Room);

		public static bool operator ==(Client client1, Client client2) =>
			EqualityComparer<Client>.Default.Equals(client1, client2);
		public static bool operator !=(Client client1, Client client2) =>
			!(client1 == client2);
	}
}