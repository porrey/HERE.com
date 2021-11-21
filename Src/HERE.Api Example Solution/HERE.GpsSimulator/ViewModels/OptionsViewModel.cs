using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HERE.GpsSimulator
{
	public enum RoutingMode
	{
		Fast,
		Short
	}

	public enum TruckType
	{
		Straight,
		Tractor
	}

	public class OptionsViewModel
	{
		[Required]
		[Display(Order = 1, Name = "origin", ShortName = "o", Description = "A location defining the origin of the trip.")]
		[JsonProperty("origin")]
		public string? Origin { get; set; }

		[Required]
		[Display(Order = 1, Name = "destination", ShortName = "d", Description = "A location defining the destination of the trip.")]
		[JsonProperty("destination")]
		public string? Destination { get; set; }

		[Display(Order = 2, Name = "departureTime", ShortName = "p", Description = "Specifies the time of departure. The default is the current date and time.")]
		[JsonProperty("departureTime")]
		public DateTimeOffset DepartureTime { get; set; } = DateTimeOffset.Now;

		[Display(Order = 3, Name = "grossWeight", ShortName = "g", Description = "Vehicle weight including trailers and shipped goods, in LBS. The default is 75,000.")]
		[JsonProperty("grossWeight")]
		public double GrossWeight { get; set; } = 75000;

		[Display(Order = 4, Name = "axleCount", ShortName = "a", Description = "Defines total number of axles in the vehicle. Default is 5.")]
		[JsonProperty("axleCount")]
		public int AxleCount { get; set; } = 5;

		[Display(Order = 5, Name = "trailerCount", ShortName = "c", Description = "Number of trailers attached to the vehicle. Default is 1.")]
		[JsonProperty("trailerCount")]
		public int TrailerCount { get; set; } = 1;

		[Display(Order = 6, Name = "truckType", ShortName = "t", Description = "Specifies the type of truck. Possible values are 'Straight' or 'Tractor'. The default is Tractor.")]
		[JsonProperty("truckType")]
		[JsonConverter(typeof(StringEnumConverter))]
		public TruckType TruckType { get; set; } = TruckType.Tractor;

		[Display(Order = 7, Name = "routingMode", ShortName = "m", Description = "Specifies which optimization is applied during route calculation. Can be 'Fast' or 'Short'. The default is Short.")]
		[JsonProperty("routingMode")]
		[JsonConverter(typeof(StringEnumConverter))]
		public RoutingMode RoutingMode { get; set; } = RoutingMode.Short;

		[Display(Order = 8, Name = "interval", ShortName = "i", Description = "Specifies the interval used to generate GPS updates. The default is every 15 minutes.")]
		[JsonProperty("interval")]
		public TimeSpan Interval { get; set; } = TimeSpan.FromMinutes(15);

		[Display(Order = 9, Name = "regulation", ShortName = "r", Description = "Specifies whether US driver regulations should be observed. The default is True.")]
		[JsonProperty("regulation")]
		public bool Regulation { get; set; } = true;

		[Display(Order = 10, Name = "lastRest", ShortName = "h", Description = "Specifies the number of hours since the driver's last rest before picking up the load. The default is 0.")]
		[JsonProperty("lastRest")]
		public decimal LastRest { get; set; } = 0;

		[Display(Order = 11, Name = "outputFile", ShortName = "f", Description = "Specifies the full path to the JSON output file.")]
		[JsonIgnore]
		public FileInfo OutputFile { get; set; } = new FileInfo("gps.json");

		[Display(Order = 12, Name = "reference1", ShortName = "r1", Description = "Specifies a reference value to be written the output file.")]
		[JsonProperty("reference1")]
		public string Reference1 { get; set; } = "";

		[Display(Order = 13, Name = "reference2", ShortName = "r2", Description = "Specifies a reference value to be written the output file.")]
		[JsonProperty("reference2")]
		public string Reference2 { get; set; } = "";

		[Display(Order = 14, Name = "reference3", ShortName = "r3", Description = "Specifies a reference value to be written the output file.")]
		[JsonProperty("reference3")]
		public string Reference3 { get; set; } = "";
	}
}
