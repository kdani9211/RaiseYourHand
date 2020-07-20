using System;
using System.Collections.Generic;

namespace RaiseYourHand
{
	public class Participant : Client, IEquatable<Participant>
	{
		public string Name { get; }

		public Participant(Room room, string name)
			: base(room) =>
			this.Name = name.Trim();
		public void RaiseHand(Hand.EHandType hand) =>
			this.Room.RaiseHand(this, hand);
		public void DownHand() =>
			this.Room.DownHand(this);
		public override bool Equals(object obj) =>
			this.Equals(obj as Participant);
		public bool Equals(Participant other) =>
			other != null &&
			base.Equals(other) &&
			this.Name == other.Name;
		public override int GetHashCode()
		{
			int hashCode = 890389916;
			hashCode = hashCode * -1521134295 + base.GetHashCode();
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Name);
			return hashCode;
		}

		public static bool operator ==(Participant participant1, Participant participant2) =>
			EqualityComparer<Participant>.Default.Equals(participant1, participant2);
		public static bool operator !=(Participant participant1, Participant participant2) =>
			!(participant1 == participant2);
	}
}