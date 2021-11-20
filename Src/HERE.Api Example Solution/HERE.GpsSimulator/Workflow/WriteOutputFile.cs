using System.Web;
using Diamond.Core.Workflow;
using HERE.Api;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#pragma warning disable CS8604
#pragma warning disable CS8629

namespace HERE.GpsSimulator
{
	public class WriteOutputFile : WorkflowItem
	{
		public WriteOutputFile(ILogger<WriteOutputFile> logger)
			: base(logger)
		{
		}

		protected override Task<bool> OnExecuteStepAsync(IContext context)
		{
			bool returnValue = false;

			//
			// Get the options from the context.
			//
			OptionsViewModel options = context.Properties.Get<OptionsViewModel>(WellKnown.Context.Options);

			//
			// Get the route pings from the context.
			//
			RoutePing[] routePings = context.Properties.Get<RoutePing[]>(WellKnown.Context.RoutePings);

			//
			// Check if the output file exists.
			//
			if (options.OutputFile.Exists)
			{
				this.Logger.LogWarning("Overwriting existing output file.");
			}

			//
			// Serialize the list to JSON.
			//
			string json = JsonConvert.SerializeObject(routePings, Formatting.Indented);

			//
			// Write the output file.
			//
			this.Logger.LogInformation("Writing put to '{name}'.", options.OutputFile.FullName);
			File.WriteAllText(options.OutputFile.FullName, json);
			returnValue = true;

			return Task.FromResult(returnValue);
		}
	}
}
