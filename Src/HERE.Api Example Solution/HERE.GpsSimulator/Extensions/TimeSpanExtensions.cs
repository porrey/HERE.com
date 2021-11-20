namespace HERE.GpsSimulator
{
	[Flags]
	public enum TimePartFlags : uint
	{
		None = 0,
		Days = 1,
		Hours = 2,
		Minutes = 4,
		Seconds = 8,
		Milliseconds = 16,
		All = Days | Hours | Minutes | Seconds | Milliseconds,
		Standard = Days | Hours | Minutes | Seconds,
		Short = Days | Hours | Minutes
	}

	public static class TimeSpanExtensions
	{
		public static string ToReadableFormat(this TimeSpan ts, TimePartFlags parts = TimePartFlags.Standard)
		{
			string returnValue = "";

			//
			// Use a list to hold the text of each time part.
			//
			List<string> items = new();

			//
			// Build the items.
			//
			items.Add(ts.Days.PluralizedLabel("Day", (parts & TimePartFlags.Days) == TimePartFlags.Days));
			items.Add(ts.Hours.PluralizedLabel("Hour", (parts & TimePartFlags.Hours) == TimePartFlags.Hours));
			items.Add(ts.Minutes.PluralizedLabel("Minute", (parts & TimePartFlags.Minutes) == TimePartFlags.Minutes));
			items.Add(ts.Seconds.PluralizedLabel("Second", (parts & TimePartFlags.Seconds) == TimePartFlags.Seconds));
			items.Add(ts.Milliseconds.PluralizedLabel("Millisecond", (parts & TimePartFlags.Milliseconds) == TimePartFlags.Milliseconds));

			//
			// Remove empty items.
			//
			var f1 = items.Where(t => !string.IsNullOrWhiteSpace(t));

			if (f1.Count() > 1)
			{
				//
				// Exclude the last item
				//
				var f2 = f1.Except(new string[] { f1.Last() });

				//
				// Join the output using 'and' on the last item.
				//
				returnValue = $"{string.Join(", ", f2)} and {f1.Last()}";
			}
			else if (f1.Count() == 1)
			{
				//
				// Single item output
				//
				returnValue = f1.First();
			}

			return returnValue;
		}

		private static string PluralizedLabel(this int value, string label, bool show)
		{
			string returnValue = "";

			if (value > 0 && show)
			{
				string plural = value == 1 ? "" : "s";
				returnValue = $"{value} {label}{plural}";
			}

			return returnValue;
		}
	}
}
