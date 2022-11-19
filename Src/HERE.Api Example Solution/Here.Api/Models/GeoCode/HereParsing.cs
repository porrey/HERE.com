using Newtonsoft.Json;

namespace HERE.Api.Models.GeoCode
{
	public class HereParsing
	{
		[JsonProperty("start")]
		public int Start { get; set; }

		[JsonProperty("end")]
		public int End { get; set; }

		[JsonProperty("value")]
		public string Value { get; set; }

		[JsonProperty("qq")]
		public string Query { get; set; }
    }
}
