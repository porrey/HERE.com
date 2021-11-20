using Diamond.Core.Workflow;
using HERE.Api;
using Microsoft.Extensions.Logging;

namespace HERE.GpsSimulator
{
	public class GetHereTokenStep : WorkflowItem
	{
		public GetHereTokenStep(ILogger<GetHereTokenStep> logger, IHereTokenFactory hereTokenFactory)
			: base(logger)
		{
			this.HereTokenFactory = hereTokenFactory;
		}

		protected IHereTokenFactory HereTokenFactory { get; set; }

		protected override async Task<bool> OnExecuteStepAsync(IContext context)
		{
			bool returnValue = false;

			//
			// Get the credentials from the context.
			//
			HereCredentials credentials = context.Properties.Get<HereCredentials>(WellKnown.Context.Credentials);

			//
			// Get a HERE token.
			//
			this.Logger.LogInformation("Requesting access token from HERE.");
			HereToken? token = await this.HereTokenFactory.CreateTokenAsync(credentials);

			if (token != null)
			{
				//
				// Save the token to the context.
				//
				this.Logger.LogInformation("Successfully retrieved access token.");
				context.Properties.Set(WellKnown.Context.Token, token);
				returnValue = true;
			}
			else
			{
				await this.StepFailedAsync(context, "Failed to get access token for HERE API.");
			}

			return returnValue;
		}
	}
}
