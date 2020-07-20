using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RaiseYourHand
{
	public class Room
	{
		private readonly HandQueue handQueue = new HandQueue();
		private readonly HashSet<Speaker> speakers = new HashSet<Speaker>();
		private IHubContext HubContext =>
			GlobalHost.ConnectionManager.GetHubContext<MainHub>();

		public Room() =>
			this.handQueue.ClientsChanged += this.BroadcastQueue;
		public void SpeakerConnected(Speaker speaker)
		{
			this.speakers.Add(speaker);
			this.BroadcastQueue(speaker);
		}
		public void RaiseHand(Participant participant, Hand.EHandType hand)
		{
			if (!this.handQueue.RaiseHand(participant, hand))
				this.BroadcastNumber(participant);
		}
		public void DownHand(Participant participant)
		{
			if (this.handQueue.DownHand(participant))
				this.BroadcastNumber(participant);
		}
		private void BroadcastQueue()
		{
			// Send to speakers
			foreach (Speaker speaker in this.speakers)
				this.BroadcastQueue(speaker);
			// Send to participant
			foreach (Hand hand in this.handQueue)
				this.BroadcastNumber(hand.Participant);
		}
		public void BroadcastNumber(Participant participant)
		{
			(int Num, Hand.EHandType Hand)? number = this.handQueue.GetNumber(participant);
			foreach (string connectionId in ConnectionMapping<Participant>.Instance[participant])
			{
				if (number.HasValue)
					this.HubContext.Clients.Client(connectionId).updateNumber(number.Value.Num + 1, Hand.GetText(number.Value.Hand).ToLower());
				else
					this.HubContext.Clients.Client(connectionId).downHand();
			}
		}
		public void BroadcastQueue(Speaker speaker)
		{
			foreach (string connection in ConnectionMapping<Speaker>.Instance[speaker])
				this.HubContext.Clients.Client(connection)?.updateQueue(this.handQueue.Hands);
		}
	}
}