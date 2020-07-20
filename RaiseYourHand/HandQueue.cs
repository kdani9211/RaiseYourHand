using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace RaiseYourHand
{
	public class HandQueue : IEnumerable<Hand>
	{
		public event Action ClientsChanged;
		private readonly ObservableCollection<Hand> hands = new ObservableCollection<Hand>();
		public IReadOnlyList<Hand> Hands =>
			this.hands;

		public HandQueue() =>
			this.hands.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) => this.OnClientsChanged();
		public bool RaiseHand(Participant participant, Hand.EHandType handType)
		{
			lock (this.hands)
			{
				if (this.hands.IsEmpty(h => h.Participant == participant))
				{
					int idx = 0;
					for (; idx < this.hands.Count; ++idx)
						if (GetPrio(this.hands[idx].HandType) > GetPrio(handType))
							break;
					this.hands.Insert(idx, new Hand(participant, handType));
					return true;
				}
			}
			return false;
		}
		public bool DownHand(Participant participant)
		{
			Hand hand = this.hands.FirstOrDefault(h => h.Participant == participant);
			if (hand is null)
				return false;
			return this.hands.Remove(hand);
		}
		public (int, Hand.EHandType)? GetNumber(Participant participant)
		{
			Hand hand = this.hands.FirstOrDefault(h => h.Participant == participant);
			if (participant is null)
				return null;
			int number = this.hands.IndexOf(hand);
			if (number < 0)
				return null;
			return (number, hand.HandType);
		}
		private void OnClientsChanged() =>
			this.ClientsChanged?.Invoke();
		private static int GetPrio(Hand.EHandType handType) =>
			(int)handType;
		public IEnumerator<Hand> GetEnumerator() =>
			this.Hands.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() =>
			this.Hands.GetEnumerator();
	}
}