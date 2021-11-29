namespace HERE.GpsSimulator
{
	public class RoutePoint
	{
		public decimal Latitude { get; set; }
		public decimal Longitude { get; set; }
		public decimal Elevation { get; set; }
		public decimal Distance { get; set; }
		public decimal Duration { get; set; }
		public double? SpeedLimit { get; set; }
		public DateTimeOffset Timestamp { get; set; }
	}
}
