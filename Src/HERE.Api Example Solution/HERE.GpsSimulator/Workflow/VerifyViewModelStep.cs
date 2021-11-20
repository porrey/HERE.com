using Diamond.Core.Workflow;
using Microsoft.Extensions.Logging;
using Diamond.Core.Rules;

namespace HERE.GpsSimulator
{
	public class VerifyViewModelStep : WorkflowItem
	{
		public VerifyViewModelStep(ILogger<VerifyViewModelStep> logger, IRulesFactory rulesFactory)
			: base(logger)
		{
			this.RulesFactory = rulesFactory;
		}

		protected IRulesFactory RulesFactory { get; set; }

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
			string? error = await this.RulesFactory.EvaluateAsync(nameof(OptionsViewModel), options);

			if (string.IsNullOrWhiteSpace(error))
			{
				this.Logger.LogDebug("Options have been successfully validated.");
				returnValue = true;
			}
			else
			{
				this.Logger.LogError("Parameters errors: {rules}.", error);
			}

			return returnValue;
		}
	}
}
