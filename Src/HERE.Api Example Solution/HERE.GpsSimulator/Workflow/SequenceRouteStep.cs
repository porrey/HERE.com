using Diamond.Core.Workflow;
using Microsoft.Extensions.Logging;

namespace HERE.GpsSimulator
{
	public class SequenceRouteStep : TemplateWorkflowStep
	{
		public SequenceRouteStep(ILogger<SequenceRouteStep> logger)
			: base(logger)
		{
		}

		protected override Task<bool> OnExecuteStepAsync(IContext context)
		{
			bool returnValue = false;

			this.Logger.LogInformation("Sequencing GPS data.");

			//
			// Get the route data from the context.
			//
			RoutePoint[] routePoints = context.Properties.Get<RoutePoint[]>(WellKnown.Context.RoutePoints);
			RouteSpan[] routeSpans = context.Properties.Get<RouteSpan[]>(WellKnown.Context.RouteSpans);

			//
			// Get the options from the context.
			//
			OptionsViewModel options = context.Properties.Get<OptionsViewModel>(WellKnown.Context.Options);

			//
			// Each span represents a set of route points.
			//
			for (int i = 0; i < routeSpans.Count(); i++)
			{
				//
				// Determine the starting point in the route points array
				// represented by this span.
				//
				int startIndex = routeSpans[i].Offset;

				//
				// Determine the ending point in the route points array
				// represented by this span.
				//
				int endIndex = i < routeSpans.Count() - 1 ? routeSpans[i + 1].Offset : routePoints.Count() - 1;

				//
				// Get the count of route points
				//
				int count = endIndex - startIndex;

				//
				// Divide the spans total duration by the number of points.
				//
				decimal incrementalDuration = routeSpans[i].Duration / count;

				//
				// Divide the spans total distance by the number of points.
				//
				decimal incrementalDistance = routeSpans[i].Distance / count;

				//
				// Assign the smaller incremental values to each route point.
				//
				for (int j = startIndex; j <= endIndex; j++)
				{
					RoutePoint routePoint = routePoints[j];
					routePoint.Duration = incrementalDuration;
					routePoint.Distance = incrementalDistance;
					routePoint.SpeedLimit = routeSpans[i].SpeedLimit;
				}
			}

			this.Render(context, $"Sequencing of GPS data completed.");
			this.Render(context, $"");
			this.Logger.LogInformation("Sequencing of GPS data completed.");
			returnValue = true;

			return Task.FromResult(returnValue);
		}
	}
}
