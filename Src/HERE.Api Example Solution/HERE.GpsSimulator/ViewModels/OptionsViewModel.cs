using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
		[JsonPropertyName("origin")]
		public string? Origin { get; set; }

		[Required]
		[Display(Order = 1, Name = "destination", ShortName = "d", Description = "A location defining the destination of the trip.")]
		[JsonPropertyName("destination")]
		public string? Destination { get; set; }

		[Display(Order = 1, Name = "departureTime", ShortName = "p", Description = "Specifies the time of departure. The default is the current date and time.")]
		[JsonPropertyName("departureTime")]
		public DateTimeOffset DepartureTime { get; set; } = DateTimeOffset.Now;

		[Display(Order = 1, Name = "grossWeight", ShortName = "g", Description = "Vehicle weight including trailers and shipped goods, in LBS. The default is 75,000.")]
		[JsonPropertyName("grossWeight")]
		public double GrossWeight { get; set; } = 75000;

		[Display(Order = 1, Name = "axleCount", ShortName = "a", Description = "Defines total number of axles in the vehicle. Default is 5.")]
		[JsonPropertyName("axleCount")]
		public uint AxleCount { get; set; } = 5;

		[Display(Order = 1, Name = "trailerCount", ShortName = "c", Description = "Number of trailers attached to the vehicle. Default is 1.")]
		[JsonPropertyName("trailerCount")]
		public uint TrailerCount { get; set; } = 1;

		[Display(Order = 1, Name = "type", ShortName = "t", Description = "Specifies the type of truck. Possible values are 'Straight' or 'Tractor'. The default is Tractor.")]
		[JsonPropertyName("type")]
		public TruckType Type { get; set; } = TruckType.Tractor;

		[Display(Order = 1, Name = "routingMode", ShortName = "m", Description = "Specifies which optimization is applied during route calculation. Can be 'Fast' or 'Short'. The default is Short.")]
		[JsonPropertyName("routingMode")]
		public RoutingMode RoutingMode { get; set; } = RoutingMode.Short;

		[Display(Order = 1, Name = "interval", ShortName = "i", Description = "Specifies the interval used to generate GPS updates. The default is every 15 minutes.")]
		[JsonPropertyName("interval")]
		public TimeSpan Interval { get; set; } = TimeSpan.FromMinutes(15);

		[Display(Order = 1, Name = "regulation", ShortName = "r", Description = "Specifies whether US driver regulations should be observed. The default is True.")]
		[JsonPropertyName("regulation")]
		public bool Regulation { get; set; } = true;

		[Display(Order = 1, Name = "lastRest", ShortName = "h", Description = "Specifies the number of hours since the driver's last rest before picking up the load. The default is 0.")]
		[JsonPropertyName("lastRest")]
		public decimal LastRest { get; set; } = 0;

		[Display(Order = 1, Name = "outputFile", ShortName = "f", Description = "Specifies the full path to the JSON output file.")]
		[JsonPropertyName("outputFile")]
		public FileInfo OutputFile { get; set; } = new FileInfo("gps.json");
	}
}
