using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaiseYourHand
{
	public class MainHub : Hub
	{
		public void SpeakerConnected(string room)
		{
			Speaker speaker =
				ConnectionMapping<Speaker>.Instance
					.Add
					(
						this.Context.ConnectionId,
						new Speaker
						(
							RoomManager.Instance[room]
						)
					);
			speaker.Room.SpeakerConnected(speaker);
		}
		public void ParticipantConnected(string room, string name)
		{
			Participant participant =
				ConnectionMapping<Participant>.Instance
					.Add
					(
						this.Context.ConnectionId,
						new Participant
						(
							RoomManager.Instance[room],
							name
						)
					);
			participant.Room.BroadcastNumber(participant);
		}
		public void RaiseHandTech() =>
			this.RaiseHand(Hand.EHandType.Technical);
		public void RaiseHandBullsh() =>
			this.RaiseHand(Hand.EHandType.Bullshit);
		public void RaiseHandTooM() =>
			this.RaiseHand(Hand.EHandType.TooMuch);
		public void RaiseHandComment() =>
			this.RaiseHand(Hand.EHandType.Comment);
		public void RaiseHandNewT() =>
			this.RaiseHand(Hand.EHandType.NewTopic);
		private void RaiseHand(Hand.EHandType hand)
		{
			Participant participant = ConnectionMapping<Participant>.Instance[this.Context.ConnectionId];
			participant?.RaiseHand(hand);
		}
		public void DownHand()
		{
			Participant participant = ConnectionMapping<Participant>.Instance[this.Context.ConnectionId];
			participant?.DownHand();
		}
		public void DownHandBySpeaker(string participantName)
		{
			Room room = ConnectionMapping<Speaker>.Instance[this.Context.ConnectionId]?.Room;
			Participant participant = new Participant(room, participantName);
			participant.DownHand();
		}
		public override Task OnDisconnected(bool stopCalled)
		{
			Participant participant = ConnectionMapping<Participant>.Instance[this.Context.ConnectionId];
			if (ConnectionMapping<Participant>.Instance.Remove(this.Context.ConnectionId))
				participant?.DownHand();
			return base.OnDisconnected(stopCalled);
		}
		public override Task OnReconnected()
		{
			Participant participant = ConnectionMapping<Participant>.Instance[this.Context.ConnectionId];
			participant?.Room.BroadcastNumber(participant);
			Speaker speaker = ConnectionMapping<Speaker>.Instance[this.Context.ConnectionId];
			speaker?.Room.BroadcastQueue(speaker);
			return base.OnReconnected();
		}
	}
}