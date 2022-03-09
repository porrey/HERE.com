using Newtonsoft.Json;

namespace HERE.Api
{
	public class HereMapView
	{
		[JsonProperty("west")]
		public decimal West { get; set; }

		[JsonProperty("south")]
		public decimal South { get; set; }

		[JsonProperty("east")]
		public decimal East { get; set; }

		[JsonProperty("north")]
		public decimal North { get; set; }
	}
}
