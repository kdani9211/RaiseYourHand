using System;

namespace RaiseYourHand.Helpers
{
	public abstract class Singleton<T> where T : new()
	{
		private static readonly Lazy<T> lazyInstance =
			new Lazy<T>(() => new T());
		public static T Instance =>
			lazyInstance.Value;
	}
}