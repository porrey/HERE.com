using Diamond.Core.Workflow;
using Microsoft.Extensions.Logging;

namespace HERE.GpsSimulator
{
	public class CreateGpsPointsStep : WorkflowItem
	{
		public CreateGpsPointsStep(ILogger<CreateGpsPointsStep> logger)
			: base(logger)
		{
		}

		protected override Task<bool> OnExecuteStepAsync(IContext context)
		{
			bool returnValue = false;

			this.Logger.LogInformation("Generating GPS ping data.");

			//
			// Get the route data from the context.
			//
			RoutePoint[] routePoints = context.Properties.Get<RoutePoint[]>(WellKnown.Context.RegulatedRoutePoints);

			//
			// Get the options from the context.
			//
			OptionsViewModel options = context.Properties.Get<OptionsViewModel>(WellKnown.Context.Options);

			//
			// Create an list of RoutePing models.
			//
			IList<RoutePing> routePings = new List<RoutePing>();

			//
			// Create an ID for each ping.
			//
			int id = 1;

			//
			// Create the starting point.
			//
			routePings.Add(new RoutePing()
			{
				Id = id,
				Latitude = routePoints[0].Latitude,
				Longitude = routePoints[0].Longitude,
				Timestamp = options.DepartureTime,
				ElapsedMiles = 0
			});

			//
			// Track accumulative time and miles within each
			// ping.
			//
			decimal accumlativeTime = 0;
			decimal accumlativeMiles = 0;

			//
			// Track total elapsed time and distance.
			//
			decimal elapsedMiles = 0;
			decimal elapsedTime = 0;

			//
			// Process each route span.
			//
			foreach (RoutePoint routePoint in routePoints)
			{
				//
				// Accumulate the time and distance for the current ping.
				//
				accumlativeTime += routePoint.Duration;
				accumlativeMiles += routePoint.Distance;

				//
				// Check if we have reached the time interval or the end of the data.
				//
				if (accumlativeTime >= (decimal)options.Interval.TotalSeconds || routePoint == routePoints.Last())
				{
					//
					// Increment the elapsed time and distance.
					//
					elapsedMiles += accumlativeMiles;
					elapsedTime += accumlativeTime;

					//
					// Increment the ID.
					//
					id++;

					//
					// Create the ping.
					//
					routePings.Add(new RoutePing()
					{
						Id = id,
						Latitude = routePoint.Latitude,
						Longitude = routePoint.Longitude,
						Timestamp = routePoint.Timestamp,
						Duration = TimeSpan.FromSeconds((double)accumlativeTime),
						Distance = accumlativeMiles,
						ElapsedMiles = elapsedMiles,
						ElapsedTime = TimeSpan.FromSeconds((double)elapsedTime),
					});

					//
					// Reset ping accumulation.
					//
					accumlativeTime = 0;
					accumlativeMiles = 0;
				}
			}

			//
			// Save the pings to the context.
			//
			this.Logger.LogInformation("Generation of GPS ping data completed successfully.");
			context.Properties.Set(WellKnown.Context.RoutePings, routePings.ToArray());
			returnValue = true;

			return Task.FromResult(returnValue);
		}
	}
}
