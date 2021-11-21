namespace HERE.GpsSimulator
{
	public class RoutePing
	{
		public int Id { get; set; } = 0;
		public double Latitude { get; set; } = 0.0;
		public double Longitude { get; set; } = 0.0;
		public DateTimeOffset Timestamp { get; set; }
		public TimeSpan Duration { get; set; } = TimeSpan.Zero;
		public decimal Distance { get; set; } = 0M;
		public decimal ElapsedMiles { get; set; } = 0M;
		public TimeSpan ElapsedTime { get; set; } = TimeSpan.Zero;
		public TimeSpan RemainingTime { get; set; } = TimeSpan.Zero;
		public decimal RemainingMiles { get; set; } = 0M;
		public string Reference1 { get; set; } = "";
		public string Reference2 { get; set; } = "";
		public string Reference3 { get; set; } = "";
	}
}
