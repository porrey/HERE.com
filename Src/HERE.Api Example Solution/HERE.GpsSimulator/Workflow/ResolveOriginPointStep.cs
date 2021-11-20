using Diamond.Core.Workflow;
using HERE.Api;
using Microsoft.Extensions.Logging;

namespace HERE.GpsSimulator
{
	public class ResolveOriginPointStep : WorkflowItem
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
			(HereGeoCodeList origin, HereApiError originError) = await this.HereGeoCodeService.FindAddressAsync(client, options.Origin );

			//
			// Check the result.
			//
			if (originError == null)
			{
				context.Properties.Set(WellKnown.Context.Origin, origin.Items[0]);
				returnValue = true;
			}
			else
			{
				await this.StepFailedAsync(context, $"Origin error: '{originError}'.");
			}

			return returnValue;
		}
	}
}
