using Diamond.Core.Workflow;
using HERE.Api;
using Microsoft.Extensions.Logging;

namespace HERE.GpsSimulator
{
	public class LoadCredentialsStep : WorkflowItem
	{
		public LoadCredentialsStep(ILogger<LoadCredentialsStep> logger)
			: base(logger)
		{
		}

		protected override Task<bool> OnExecuteStepAsync(IContext context)
		{
			bool returnValue = false;

			//
			// Load the HERE credentials from the file.
			//
			this.Logger.LogDebug("Loading HERE credentials from the credentials file.");
			HereCredentials credentials = HereCredentials.FromFile("./credentials.properties");

			//
			// Save the credentials to the context.
			//
			this.Logger.LogInformation("HERE credentials have been successfully loaded.");
			context.Properties.Set(WellKnown.Context.Credentials, credentials);
			returnValue = true;

			return Task.FromResult(returnValue);
		}
	}
}
