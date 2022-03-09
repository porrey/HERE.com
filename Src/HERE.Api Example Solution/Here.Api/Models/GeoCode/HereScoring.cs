using Newtonsoft.Json;

namespace HERE.Api
{
	public class HereScoring
	{
		[JsonProperty("queryScore")]
		public float QueryScore { get; set; }

		[JsonProperty("fieldScore")]
		public HereFieldScore FieldScore { get; set; }
	}

	public class HereFieldScore
	{
		[JsonProperty("state")]
		public float State { get; set; }

		[JsonProperty("city")]
		public float City { get; set; }

		[JsonProperty("streets")]
		public float[] Streets { get; set; }

		[JsonProperty("postalCode")]
		public float PostalCode { get; set; }
	}
}
