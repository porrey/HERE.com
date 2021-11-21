using System.CommandLine.Rendering;
using Diamond.Core.Workflow;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HERE.GpsSimulator
{
	public class WriteOutputFile : TemplateWorkflowStep
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

			this.Logger.LogInformation("Creating output file '{file}'.", options.OutputFile.FullName);
			this.Render(context, $"Creating output file '{StyleSpan.BoldOn()}{ForegroundColorSpan.Yellow()}{options.OutputFile.FullName}{ForegroundColorSpan.Reset()}{StyleSpan.BoldOff()}'.");

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
			this.Logger.LogInformation("Writing output to '{name}'.", options.OutputFile.FullName);
			File.WriteAllText(options.OutputFile.FullName, json);

			this.Logger.LogInformation("The output file was successfully created.");
			this.Render(context, $"\tOutput file: {StyleSpan.BoldOn()}{ForegroundColorSpan.White()}Created{ForegroundColorSpan.Reset()}{StyleSpan.BoldOff()}");
			this.Render(context, $"");

			returnValue = true;

			return Task.FromResult(returnValue);
		}
	}
}
