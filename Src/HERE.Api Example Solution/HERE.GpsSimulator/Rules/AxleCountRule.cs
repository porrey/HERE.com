using Diamond.Core.Rules;
using Microsoft.Extensions.Logging;

namespace HERE.GpsSimulator
{
	public class AxleCountRule : RuleTemplate<OptionsViewModel>
	{
		public AxleCountRule(ILogger<AxleCountRule> logger)
			: base(logger, nameof(OptionsViewModel))
		{
		}

		protected override Task<IRuleResult> OnValidateAsync(OptionsViewModel item)
		{
			IRuleResult returnValue = new RuleResultTemplate();

			if (item.AxleCount > 0)
			{
				returnValue.Passed = true;
				returnValue.ErrorMessage = null;
			}
			else
			{
				returnValue.Passed = false;
				returnValue.ErrorMessage = "Invalid Axle Count.";
			}

			return Task.FromResult(returnValue);
		}
	}
}
