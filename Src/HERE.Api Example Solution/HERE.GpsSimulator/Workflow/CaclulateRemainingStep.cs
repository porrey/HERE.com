using Diamond.Core.Workflow;
using Microsoft.Extensions.Logging;

namespace HERE.GpsSimulator
{
	public class CaclulateRemainingStep : WorkflowItem
	{
		public CaclulateRemainingStep(ILogger<CaclulateRemainingStep> logger)
			: base(logger)
		{
		}

		protected override Task<bool> OnExecuteStepAsync(IContext context)
		{
			bool returnValue = false;

			this.Logger.LogInformation("Calculating remaining time and miles.");

			//
			// Get the route data from the context.
			//
			RoutePing[] routePings = context.Properties.Get<RoutePing[]>(WellKnown.Context.RoutePings);

			//
			// Process each route span.
			//
			for (int i = routePings.Length - 1; i >= 0; i--)
			{
				RoutePing currentPing = routePings[i];

				if (i == 0)
				{
					currentPing.RemainingTime = routePings.Last().ElapsedTime;
					currentPing.RemainingMiles = routePings.Last().ElapsedMiles;
				}
				else if (i < (routePings.Length - 1))
				{
					RoutePing nextPing = routePings[i + 1];
					currentPing.RemainingTime = nextPing.RemainingTime.Add(currentPing.Duration);
					currentPing.RemainingMiles = nextPing.RemainingMiles + currentPing.Distance;
				}
			}

			//
			// Save the pings to the context.
			//
			this.Logger.LogInformation("Successfully calculated remaining time and miles.");
			returnValue = true;

			return Task.FromResult(returnValue);
		}
	}
}
