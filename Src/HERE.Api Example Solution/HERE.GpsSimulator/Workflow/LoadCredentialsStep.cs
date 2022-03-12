using Diamond.Core.Workflow;
using HERE.Api;
using Microsoft.Extensions.Logging;
using System.CommandLine.Rendering;

namespace HERE.GpsSimulator
{
	public class LoadCredentialsStep : TemplateWorkflowStep
	{
		public LoadCredentialsStep(ILogger<LoadCredentialsStep> logger)
			: base(logger)
		{
		}

		protected override async Task<bool> OnExecuteStepAsync(IContext context)
		{
			bool returnValue = false;

			FileInfo file = new("./here.credentials.properties");

			try
			{
				//
				// Load the HERE credentials from the file.
				//
				this.Logger.LogDebug("Loading HERE credentials from the credentials file.");
				HereCredentials credentials = HereCredentials.FromFile(file.FullName);

				//
				// Save the credentials to the context.
				//
				this.Logger.LogInformation("HERE credentials have been successfully loaded.");
				this.Render(context, $"The credentials file '{BackgroundColorSpan.Blue()}{ForegroundColorSpan.White()}{file.Name}{ForegroundColorSpan.Reset()}{BackgroundColorSpan.Reset()}' was loaded successfully.");
				this.Render(context, $"");

				context.Properties.Set(WellKnown.Context.Credentials, credentials);
				returnValue = true;
			}
			catch (FileNotFoundException)
			{
				this.Logger.LogError($"The credentials file '{file.FullName}' was not found.");
				this.Render(context, $"The credentials file '{BackgroundColorSpan.Red()}{ForegroundColorSpan.White()}{file.Name}{ForegroundColorSpan.Reset()}{BackgroundColorSpan.Reset()}' was not found.");
				this.Render(context, $""); 
				await this.StepFailedAsync(context, $"Credentials file not found.");
			}
			catch (Exception ex)
			{
				this.Logger.LogError(nameof(LoadCredentialsStep), ex);
				this.Render(context, $"An exception occurred while loading the credentials file '{BackgroundColorSpan.Red()}{ForegroundColorSpan.White()}{file.Name}{ForegroundColorSpan.Reset()}{BackgroundColorSpan.Reset()}'. See log for more details.");
				this.Render(context, $""); 
				await this.StepFailedAsync(context, $"Exception occurred in {nameof(LoadCredentialsStep)}");
			}

			return returnValue;
		}
	}
}
