using System.Collections.Generic;

namespace System.Linq
{
	public static class Extended
	{
		public static bool IsEmpty<T>(this IEnumerable<T> source) =>
			!source.Any();
		public static bool IsEmpty<T>(this IEnumerable<T> source, Func<T, bool> predicate) =>
			!source.Any(predicate);
	}
}

namespace System
{
	public static class Extended
	{
		public static bool IsNullOrEmpty(this string value) =>
			String.IsNullOrWhiteSpace(value);
	}
}