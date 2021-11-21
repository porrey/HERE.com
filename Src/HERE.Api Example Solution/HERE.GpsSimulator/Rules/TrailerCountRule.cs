using Diamond.Core.Rules;
using Microsoft.Extensions.Logging;

namespace HERE.GpsSimulator
{
	public class TrailerCountRule : RuleTemplate<OptionsViewModel, IOptionsRuleResult>
	{
		public TrailerCountRule(ILogger<TrailerCountRule> logger)
			: base(logger, nameof(OptionsViewModel))
		{
		}

		protected override Task<IOptionsRuleResult> OnValidateAsync(OptionsViewModel item)
		{
			IOptionsRuleResult returnValue = new OptionsRuleResult();

			if (item.TrailerCount > 0)
			{
				returnValue.Passed = true;
				returnValue.ErrorMessage = null;
			}
			else
			{
				returnValue.Passed = false;
				returnValue.ErrorMessage = $"Value ({item.TrailerCount}) must be greater than 0.";
				returnValue.Parameter = nameof(OptionsViewModel.TrailerCount);
			}

			return Task.FromResult(returnValue);
		}
	}
}
