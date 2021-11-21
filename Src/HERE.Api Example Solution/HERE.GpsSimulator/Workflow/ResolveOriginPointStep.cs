using Diamond.Core.Workflow;
using HERE.Api;
using Microsoft.Extensions.Logging;
using System.CommandLine.Rendering;

namespace HERE.GpsSimulator
{
	public class ResolveOriginPointStep : TemplateWorkflowStep
	{
		public ResolveOriginPointStep(ILogger<ResolveOriginPointStep> logger, IHereTokenFactory hereTokenFactory, IHereGeoCodeService hereGeoCodeService)
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
			(HereGeoCodeList originList, HereApiError originError) = await this.HereGeoCodeService.FindAddressAsync(client, options.Origin );
			
			//
			// Check the result.
			//
			if (originError == null)
			{
				HereGeoCodeItem origin = originList.Items[0];

				this.Render(context, $"Origin: '{ForegroundColorSpan.Yellow()}{StyleSpan.BoldOn()}{options.Origin}{StyleSpan.BoldOff()}{ForegroundColorSpan.Reset()}':");
				this.Render(context, $"\tID                  : {ForegroundColorSpan.White()}{StyleSpan.BoldOn()}{origin.Id}{StyleSpan.BoldOff()}{ForegroundColorSpan.Reset()}");
				this.Render(context, $"\tGEO Coordinates     : {ForegroundColorSpan.White()}{StyleSpan.BoldOn()}Latitude = {origin.Position.Latitude}, Longitude = {origin.Position.Longitude}, Elevation = {origin.Position.Elevation}{StyleSpan.BoldOff()}{ForegroundColorSpan.Reset()}");
				this.Render(context, $"\tStandardized Address: {ForegroundColorSpan.White()}{StyleSpan.BoldOn()}{origin.Address.Label.ToUpper()}{StyleSpan.BoldOff()}{ForegroundColorSpan.Reset()}");
				this.Render(context, $"");

				context.Properties.Set(WellKnown.Context.Origin, origin);
				returnValue = true;
			}
			else
			{
				this.Render(context, $"Invalid origin address: '{BackgroundColorSpan.Red()}{ForegroundColorSpan.White()}{originError}{BackgroundColorSpan.Reset()}{ForegroundColorSpan.Reset()}'");
				this.Render(context, $""); 
				await this.StepFailedAsync(context, $"Origin error: '{originError}'.");
			}

			return returnValue;
		}
	}
}
