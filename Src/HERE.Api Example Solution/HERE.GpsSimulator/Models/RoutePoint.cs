namespace HERE.GpsSimulator
{
	public class RoutePoint
	{
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public decimal Distance { get; set; }
		public decimal Duration { get; set; }
		public double? SpeedLimit { get; set; }
		public DateTimeOffset Timestamp { get; set; }
	}
}
