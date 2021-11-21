using System.CommandLine.Rendering;
using Diamond.Core.Workflow;
using HERE.Api;
using Microsoft.Extensions.Logging;

namespace HERE.GpsSimulator
{
	public class ResolveDestinationPointStep : TemplateWorkflowStep
	{
		public ResolveDestinationPointStep(ILogger<ResolveDestinationPointStep> logger, IHereTokenFactory hereTokenFactory, IHereGeoCodeService hereGeoCodeService)
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
			// Call the HERE service.
			//
			(HereGeoCodeList destinationList, HereApiError destinationError) = await this.HereGeoCodeService.FindAddressAsync(client, options.Destination );

			//
			// Check the result.
			//
			if (destinationError == null)
			{
				HereGeoCodeItem destination = destinationList.Items[0];

				this.Render(context, $"destination: '{ForegroundColorSpan.Yellow()}{StyleSpan.BoldOn()}{options.Destination}{StyleSpan.BoldOff()}{ForegroundColorSpan.Reset()}':");
				this.Render(context, $"\tID                  : {ForegroundColorSpan.White()}{StyleSpan.BoldOn()}{destination.Id}{StyleSpan.BoldOff()}{ForegroundColorSpan.Reset()}");
				this.Render(context, $"\tGEO Coordinates     : {ForegroundColorSpan.White()}{StyleSpan.BoldOn()}Latitude = {destination.Position.Latitude}, Longitude = {destination.Position.Longitude}, Elevation = {destination.Position.Elevation}{StyleSpan.BoldOff()}{ForegroundColorSpan.Reset()}");
				this.Render(context, $"\tStandardized Address: {ForegroundColorSpan.White()}{StyleSpan.BoldOn()}{destination.Address.Label.ToUpper()}{StyleSpan.BoldOff()}{ForegroundColorSpan.Reset()}");
				this.Render(context, $"");

				context.Properties.Set(WellKnown.Context.Destination, destination);
				returnValue = true;
			}
			else
			{
				this.Render(context, $"Invalid destination address: '{BackgroundColorSpan.Red()}{ForegroundColorSpan.White()}{destinationError}{BackgroundColorSpan.Reset()}{ForegroundColorSpan.Reset()}'");
				this.Render(context, $""); 
				await this.StepFailedAsync(context, $"Destination error: '{destinationError}'.");
			}

			return returnValue;
		}
	}
}
