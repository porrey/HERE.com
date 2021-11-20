namespace HERE.GpsSimulator
{
	public class RouteSpan
	{
		public int Offset { get; internal set; }
		public double? SpeedLimit { get; internal set; }
		public decimal Distance { get; internal set; }
		public decimal Duration { get; internal set; }
	}
}
