using RaiseYourHand.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RaiseYourHand
{
	public class ConnectionMapping<TClient> : Singleton<ConnectionMapping<TClient>> where TClient : Client
	{
		private readonly Dictionary<TClient, HashSet<string>> clientToConnections = new Dictionary<TClient, HashSet<string>>();
		private readonly Dictionary<string, TClient> connectionToClient = new Dictionary<string, TClient>();
		public IEnumerable<string> this[TClient client]
		{
			get
			{
				if (this.clientToConnections.TryGetValue(client, out HashSet<string> myConnections))
					return myConnections;
				return Enumerable.Empty<string>();
			}
		}
		public TClient this[string connectionId]
		{
			get
			{
				this.connectionToClient.TryGetValue(connectionId, out TClient client);
				return client;
			}
		}
		public int Count =>
			this.clientToConnections.Count;

		public TClient Add(string connectionId, TClient client)
		{
			lock (this.clientToConnections)
				lock (this.connectionToClient)
				{
					TClient existsClient = this[connectionId];
					if (existsClient != null)
						return existsClient;
					if (!this.clientToConnections.TryGetValue(client, out HashSet<string> myConnections))
					{
						myConnections = new HashSet<string>();
						this.clientToConnections.Add(client, myConnections);
					}
					this.connectionToClient.Add(connectionId, client);
					lock (myConnections)
					{
						myConnections.Add(connectionId);
					}
					return client;
				}
		}
		public bool Remove(string connectionId)
		{
			lock (this.clientToConnections)
				lock (this.connectionToClient)
				{
					TClient client = this[connectionId];
					if (client is null)
						return true;
					this.connectionToClient.Remove(connectionId);
					HashSet<string> myConnections = this.clientToConnections[client];
					lock (myConnections)
					{
						myConnections.Remove(connectionId);
						if (myConnections.IsEmpty())
						{
							this.clientToConnections.Remove(client);
							return true;
						}
						return false;
					}
				}
		}
	}
}