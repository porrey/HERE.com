using Diamond.Core.Rules;
using Microsoft.Extensions.Logging;

namespace HERE.GpsSimulator
{
	public class HoursRule : RuleTemplate<OptionsViewModel>
	{
		public HoursRule(ILogger<HoursRule> logger)
			: base(logger, nameof(OptionsViewModel))
		{
		}

		protected override Task<IRuleResult> OnValidateAsync(OptionsViewModel item)
		{
			IRuleResult returnValue = new RuleResultTemplate();

			if (item.LastRest > 0)
			{
				returnValue.Passed = true;
				returnValue.ErrorMessage = null;
			}
			else
			{
				returnValue.Passed = false;
				returnValue.ErrorMessage = "Invalid Hours.";
			}

			return Task.FromResult(returnValue);
		}
	}
}
