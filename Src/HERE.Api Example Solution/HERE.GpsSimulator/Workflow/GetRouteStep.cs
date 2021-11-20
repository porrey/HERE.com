using System.Web;
using Diamond.Core.Workflow;
using HERE.Api;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

#pragma warning disable CS8604
#pragma warning disable CS8629

namespace HERE.GpsSimulator
{
	public class GetRouteStep : WorkflowItem
	{
		public GetRouteStep(ILogger<GetRouteStep> logger, IHereTokenFactory hereTokenFactory, IHereGeoCodeService hereGeoCodeService)
			: base(logger)
		{
			this.HereTokenFactory = hereTokenFactory;
			this.HereGeoCodeService = hereGeoCodeService;
		}

		protected IHereTokenFactory HereTokenFactory { get; set; }
		protected IHereGeoCodeService HereGeoCodeService { get; set; }

		protected override async Task<bool> OnExecuteStepAsync(IContext context)
		{
			bool returnValue = false;

			//
			// Get the token from the context.
			//
			HereToken token = context.Properties.Get<HereToken>(WellKnown.Context.Token);

			//
			// Create a client.
			//
			HereHttpClient client = this.HereTokenFactory.CreateHttpClient(token);

			//
			// Get the options from the context.
			//
			OptionsViewModel options = context.Properties.Get<OptionsViewModel>(WellKnown.Context.Options);

			//
			// Get the origin and destination from the context.
			//
			HereGeoCodeItem origin = context.Properties.Get<HereGeoCodeItem>(WellKnown.Context.Origin);
			HereGeoCodeItem destination = context.Properties.Get<HereGeoCodeItem>(WellKnown.Context.Destination);

			IDictionary<string, string> parameters = new Dictionary<string, string>
					{
						{ "transportMode", "truck" },
						{ "origin", $"{origin.Position.Latitude},{origin.Position.Longitude}" },
						{ "destination", $"{destination.Position.Latitude},{destination.Position.Longitude}" },
						{ "return", "travelSummary,polyline,routingZones" },
						{ "spans", "speedLimit,duration,length" },
						{ "routingMode", options.RoutingMode.ToString() },
						{ "lang", "en-US" },
						{ "departureTime", options.DepartureTime.ToString("yyyy-MM-ddTHH:mm:sszzz") },
						{ "units", "metric" },
						{ "grossWeight", $"{options.GrossWeight / 2.20462262}" },
						{ "axleCount", $"{options.AxleCount}" },
						{ "trailerCount", $"{options.TrailerCount}" },
						{ "type", $"{options.Type}" }
					};

			//
			// Convert the parameters.
			//
			string query = string.Join("&", parameters.Select(kvp => $"{HttpUtility.UrlEncode(kvp.Key)}={HttpUtility.UrlEncode(kvp.Value)}"));

			//
			// Make the API call.
			//
			this.Logger.LogInformation("Requesting route from HERE API.");
			HttpResponseMessage response = await client.GetAsync($"https://router.hereapi.com/v8/routes?{query}");

			//
			// Get the response content.
			//
			string responseContent = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
			{
				//
				// Deserialize the response.
				//
				dynamic item = JObject.Parse(responseContent);

				//
				// get the encoded polyline string.
				//
				string polyline = item.routes[0].sections[0].polyline;

				//
				// Decode the polyline into geo coordinates.
				//
				IEnumerable<RoutePoint> points = (from tbl in PolylineEncoderDecoder.Decode(polyline)
												  select new RoutePoint()
												  {
													  Latitude = tbl.Latitude,
													  Longitude = tbl.Longitude,
													  Distance = (decimal)0,
													  Duration = 0
												  }).ToArray();

				//
				// Get the spans with speed limits.
				//
				IEnumerable<RouteSpan> spans = (from tbl in (JArray)item.routes[0].sections[0].spans
												select new RouteSpan()
												{
													Offset = tbl["offset"].Value<int>(),
													SpeedLimit = tbl["speedLimit"] != null ? Math.Round(tbl["speedLimit"].Value<double?>().Value * 2.23693629, 0) : (double?)null,
													Distance = tbl["length"].Value<decimal>() / 1609.344M,
													Duration = tbl["duration"].Value<decimal>()
												}).ToArray();
				//
				// Save the results to the context.
				//
				this.Logger.LogInformation("Total route miles are {miles}.", spans.Sum(t => t.Distance).ToString("#,###.##"));
				this.Logger.LogInformation("Total route time is {time}.", TimeSpan.FromSeconds((double)spans.Sum(t => t.Duration)).ToReadableFormat());

				context.Properties.Set(WellKnown.Context.RoutePoints, points);
				context.Properties.Set(WellKnown.Context.RouteSpans, spans);
				returnValue = true;
			}
			else
			{
				await this.StepFailedAsync(context, $"Failed to get route: [{response.StatusCode}] {response.ReasonPhrase}");
			}

			return returnValue;
		}
	}
}
