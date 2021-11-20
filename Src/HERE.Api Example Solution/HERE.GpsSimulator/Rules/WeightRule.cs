using Diamond.Core.Rules;
using Microsoft.Extensions.Logging;

namespace HERE.GpsSimulator
{
	public class WeightRule : RuleTemplate<OptionsViewModel>
	{
		public WeightRule(ILogger<WeightRule> logger)
			: base(logger, nameof(OptionsViewModel))
		{
		}

		protected override Task<IRuleResult> OnValidateAsync(OptionsViewModel item)
		{
			IRuleResult returnValue = new RuleResultTemplate();

			if (item.GrossWeight > 0 && item.GrossWeight < 80000)
			{
				returnValue.Passed = true;
				returnValue.ErrorMessage = null;
			}
			else
			{
				returnValue.Passed = false;
				returnValue.ErrorMessage = "Invalid Weight.";
			}

			return Task.FromResult(returnValue);
		}
	}
}
