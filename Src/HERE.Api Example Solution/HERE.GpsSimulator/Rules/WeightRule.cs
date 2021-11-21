using Diamond.Core.Rules;
using Microsoft.Extensions.Logging;

namespace HERE.GpsSimulator
{
	public class WeightRule : RuleTemplate<OptionsViewModel, IOptionsRuleResult>
	{
		public WeightRule(ILogger<WeightRule> logger)
			: base(logger, nameof(OptionsViewModel))
		{
		}

		protected override Task<IOptionsRuleResult> OnValidateAsync(OptionsViewModel item)
		{
			IOptionsRuleResult returnValue = new OptionsRuleResult();

			if (item.GrossWeight > 0 && item.GrossWeight <= 80000)
			{
				returnValue.Passed = true;
				returnValue.ErrorMessage = null;
			}
			else
			{
				returnValue.Passed = false;
				returnValue.ErrorMessage = $"Value ({item.GrossWeight}) must be greater than 0 and less than or equal to 80,000.";
				returnValue.Parameter = nameof(OptionsViewModel.GrossWeight);
			}

			return Task.FromResult(returnValue);
		}
	}
}
