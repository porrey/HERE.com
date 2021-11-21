using System.CommandLine.Rendering;
using Diamond.Core.CommandLine;
using Diamond.Core.Rules;
using Diamond.Core.Workflow;
using Microsoft.Extensions.Logging;

namespace HERE.GpsSimulator
{
	public class ValidateViewModelStep : TemplateWorkflowStep
	{
		public ValidateViewModelStep(ILogger<ValidateViewModelStep> logger, IRulesFactory rulesFactory, IRootCommand rootCommand)
			: base(logger)
		{
			this.RulesFactory = rulesFactory;
			this.RootCommand = rootCommand;
		}

		protected IRulesFactory RulesFactory { get; set; }
		protected IRootCommand RootCommand { get; set; }

		protected override async Task<bool> OnExecuteStepAsync(IContext context)
		{
			bool returnValue = false;

			//
			// Get the options from the context.
			//
			this.Logger.LogDebug("Validating options.");
			var options = context.Properties.Get<OptionsViewModel>(WellKnown.Context.Options);

			//
			// Evaluate the model against the rules.
			//
			IEnumerable<IRule<OptionsViewModel, IOptionsRuleResult>> rules = await this.RulesFactory.GetAllAsync<OptionsViewModel, IOptionsRuleResult>(nameof(OptionsViewModel));

			//
			// Evaluate the rules.
			//
			IEnumerable<IOptionsRuleResult> results = rules.Select(t => t.ValidateAsync(options).Result);

			//
			// Get the errors.
			//
			IEnumerable<IOptionsRuleResult> errors = results.Where(t => !t.Passed);

			if (errors.Any())
			{
				this.Render(context, $"{StyleSpan.BoldOn()}{ForegroundColorSpan.White()}One or more parameters failed validation.{ForegroundColorSpan.Reset()}{StyleSpan.BoldOff()}");

				foreach (var error in errors)
				{
					this.Render(context, $"\t{StyleSpan.BoldOn()}{error.Parameter}{StyleSpan.BoldOff()} => {BackgroundColorSpan.Red()}{ForegroundColorSpan.White()}{error.ErrorMessage}{ForegroundColorSpan.Reset()}{BackgroundColorSpan.Reset()}");
					this.Logger.LogError("Parameter '{parameter}' failed: '{error}'.", error.Parameter, error.ErrorMessage);
				}

				this.Render(context, $"");
				await this.StepFailedAsync(context, "One or more parameters failed validation.");
			}
			else
			{
				this.Render(context, $"Parameters have been validated => {BackgroundColorSpan.Green()}{ForegroundColorSpan.White()}OK{ForegroundColorSpan.Reset()}{BackgroundColorSpan.Reset()}.");
				this.Render(context, $""); 
				this.Logger.LogDebug("Options have been successfully validated.");
				returnValue = true;
			}

			return returnValue;
		}
	}
}
