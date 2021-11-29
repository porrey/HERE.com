using System.CommandLine.Rendering;
using Diamond.Core.Workflow;
using Microsoft.Extensions.Logging;

namespace HERE.GpsSimulator
{
	public class ApplyDriverRegulationsStep : TemplateWorkflowStep
	{
		public ApplyDriverRegulationsStep(ILogger<ApplyDriverRegulationsStep> logger)
			: base(logger)
		{
		}

		protected override Task<bool> OnExecuteStepAsync(IContext context)
		{
			bool returnValue = false;

			//
			// Get the options from the context.
			//
			OptionsViewModel options = context.Properties.Get<OptionsViewModel>(WellKnown.Context.Options);

			//
			// Get the route data from the context.
			//
			RoutePoint[] routePoints = context.Properties.Get<RoutePoint[]>(WellKnown.Context.RoutePoints);

			if (options.Regulation)
			{
				this.Logger.LogInformation("Applying Driver Regulations.");
				this.Render(context, $"Applying Driver Regulations.");

				//
				// Create a new list.
				//
				List<RoutePoint> newRoutePoints = new();

				//
				// Driver time in seconds.
				//
				decimal driverTime = options.LastRest * 3600;
				this.Render(context, $"\tLast Rest: {ForegroundColorSpan.White()}{StyleSpan.BoldOn()}{TimeSpan.FromSeconds((double)driverTime).ToReadableFormat()}{StyleSpan.BoldOff()}{ForegroundColorSpan.Reset()}");

				bool thirtyMinuteRest = false;
				bool tenHourReast = false;

				foreach (RoutePoint routePoint in routePoints)
				{
					driverTime += routePoint.Duration;
					newRoutePoints.Add(routePoint);

					if (driverTime > (24 * 3600) && tenHourReast && thirtyMinuteRest)
					{
						//
						// Reset driver time.
						//
						driverTime = 0;
						thirtyMinuteRest = false;
						tenHourReast = false;
						this.Logger.LogInformation("Reset driver time.");
					}
					else if (driverTime > (11 * 3600) && !tenHourReast)
					{
						//
						// Add 10 hour rest.
						//
						this.Logger.LogInformation("Adding a 10 hour rest.");
						newRoutePoints.AddRange(this.CreateRestPoints(routePoint, TimeSpan.FromHours(10)));
						tenHourReast = true;
					}
					else if (driverTime > (8 * 3600) && !thirtyMinuteRest)
					{
						//
						// Add 30 minute rest.
						//
						this.Logger.LogInformation("Adding a 30 minute rest.");
						newRoutePoints.AddRange(this.CreateRestPoints(routePoint, TimeSpan.FromMinutes(30)));
						thirtyMinuteRest = true;
					}
				}

				//
				// Add the new points to the context.
				//
				string duration = TimeSpan.FromSeconds((double)newRoutePoints.Sum(t => t.Duration)).ToReadableFormat();

				this.Logger.LogInformation("Total route time has been expanded to {time}.", duration);
				context.Properties.Add(WellKnown.Context.RegulatedRoutePoints, newRoutePoints.ToArray());
				returnValue = true;

				this.Render(context, $"\tNew route duration: {StyleSpan.BoldOn()}{ForegroundColorSpan.White()}{duration}{ForegroundColorSpan.Reset()}{StyleSpan.BoldOff()}");
				this.Render(context, $"");
			}
			else
			{
				context.Properties.Add(WellKnown.Context.RegulatedRoutePoints, routePoints);
				this.Logger.LogInformation("Skipping Driver Regulations.");
				returnValue = true;
			}

			return Task.FromResult(returnValue);
		}

		protected IEnumerable<RoutePoint> CreateRestPoints(RoutePoint baseRoutePoint, TimeSpan rest)
		{
			IList<RoutePoint> returnValue = new List<RoutePoint>();

			decimal addedTime = 0;

			while (addedTime < (decimal)rest.TotalSeconds)
			{
				addedTime += baseRoutePoint.Duration;

				returnValue.Add(new RoutePoint()
				{
					Distance = 0,
					Latitude = baseRoutePoint.Latitude,
					Longitude = baseRoutePoint.Longitude,
					Elevation = baseRoutePoint.Elevation,
					Duration = baseRoutePoint.Duration,
					SpeedLimit = 0
				});
			}

			this.Logger.LogInformation("Added {count} resting route points.", returnValue.Count());
			return returnValue;
		}
	}
}
