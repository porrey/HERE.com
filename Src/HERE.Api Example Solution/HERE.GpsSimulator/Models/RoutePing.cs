namespace HERE.GpsSimulator
{
	public class RoutePing
	{
		public int Id { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public DateTimeOffset Timestamp { get; set; }
		public TimeSpan Duration { get; set; }
		public decimal Distance { get; set; }
		public decimal ElapsedMiles { get; set; }
		public TimeSpan ElapsedTime { get; set; }
		public TimeSpan RemainingTime { get; set; }
		public decimal RemainingMiles { get; set; }
	}
}
