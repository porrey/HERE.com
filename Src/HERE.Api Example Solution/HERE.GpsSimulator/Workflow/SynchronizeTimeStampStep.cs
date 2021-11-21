using Diamond.Core.Workflow;
using Microsoft.Extensions.Logging;

namespace HERE.GpsSimulator
{
	public class SynchronizeTimeStampStep : TemplateWorkflowStep
	{
		public SynchronizeTimeStampStep(ILogger<SynchronizeTimeStampStep> logger)
			: base(logger)
		{
		}

		protected override Task<bool> OnExecuteStepAsync(IContext context)
		{
			bool returnValue = false;

			this.Logger.LogInformation("Synchronizing Time Stamp.");

			//
			// Get the options from the context.
			//
			OptionsViewModel options = context.Properties.Get<OptionsViewModel>(WellKnown.Context.Options);

			//
			// Get the route data from the context.
			//
			RoutePoint[] routePoints = context.Properties.Get<RoutePoint[]>(WellKnown.Context.RegulatedRoutePoints);

			//
			// Process each route span.
			//
			DateTimeOffset timestamp = options.DepartureTime;

			foreach (RoutePoint routePoint in routePoints)
			{
				timestamp = timestamp.AddSeconds((double)routePoint.Duration);
				routePoint.Timestamp = timestamp;
			}

			//
			// Add the new points to the context.
			//
			returnValue = true;
			this.Render(context, $"Synchronization of time stamp completed.");
			this.Render(context, $"");

			return Task.FromResult(returnValue);
		}
	}
}
