using Diamond.Core.Rules;
using Microsoft.Extensions.Logging;

namespace HERE.GpsSimulator
{
	public class LastRestRule : RuleTemplate<OptionsViewModel, IOptionsRuleResult>
	{
		public LastRestRule(ILogger<LastRestRule> logger)
			: base(logger, nameof(OptionsViewModel))
		{
		}

		protected override Task<IOptionsRuleResult> OnValidateAsync(OptionsViewModel item)
		{
			IOptionsRuleResult returnValue = new OptionsRuleResult();

			if (item.LastRest >= 0)
			{
				returnValue.Passed = true;
				returnValue.ErrorMessage = null;
			}
			else
			{
				returnValue.Passed = false;
				returnValue.ErrorMessage = $"Value ({item.LastRest}) must be greater than or equal to 0.";
				returnValue.Parameter = nameof(OptionsViewModel.LastRest);
			}

			return Task.FromResult(returnValue);
		}
	}
}
