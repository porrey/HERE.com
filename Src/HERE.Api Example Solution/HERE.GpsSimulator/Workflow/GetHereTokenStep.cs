using Diamond.Core.Workflow;
using HERE.Api;
using Microsoft.Extensions.Logging;

namespace HERE.GpsSimulator
{
	public class GetHereTokenStep : TemplateWorkflowStep
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

			try
			{
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
					this.Render(context, $"Successfully retrieved access token.");
					context.Properties.Set(WellKnown.Context.Token, token);
					returnValue = true;
				}
				else
				{
					this.Render(context, $"Failed to get access token for HERE API.");
					await this.StepFailedAsync(context, "Failed to get access token for HERE API.");
				}

				this.Render(context, $"");
			}
			catch (UnauthorizedAccessException)
			{
				this.Render(context, $"Unauthorized Access for HERE API.");
				this.Logger.LogError($"Unauthorized Access Exception in {nameof(GetHereTokenStep)}");
				this.Render(context, $""); 
				await this.StepFailedAsync(context, $"Exception occurred in {nameof(GetHereTokenStep)}");
			}
			catch (Exception ex)
			{
				this.Render(context, $"Failed to get access token for HERE API.");
				this.Logger.LogError(ex, $"Exception occurred in {nameof(GetHereTokenStep)}");
				this.Render(context, $""); 
				await this.StepFailedAsync(context, $"Exception occurred in {nameof(GetHereTokenStep)}");
			}

			return returnValue;
		}
	}
}
