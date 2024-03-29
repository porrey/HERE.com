﻿using Diamond.Core.Rules;
using Microsoft.Extensions.Logging;

namespace HERE.GpsSimulator
{
	public class AxleCountRule : RuleTemplate<OptionsViewModel, IOptionsRuleResult>
	{
		public AxleCountRule(ILogger<AxleCountRule> logger)
			: base(logger, nameof(OptionsViewModel))
		{
		}

		protected override Task<IOptionsRuleResult> OnValidateAsync(OptionsViewModel item)
		{
			IOptionsRuleResult returnValue = new OptionsRuleResult();

			if (item.AxleCount > 0)
			{
				returnValue.Passed = true;
				returnValue.ErrorMessage = null;
			}
			else
			{
				returnValue.Passed = false;
				returnValue.ErrorMessage = $"Value ({item.AxleCount}) must be greater than 0.";
				returnValue.Parameter = nameof(OptionsViewModel.AxleCount);
			}

			return Task.FromResult(returnValue);
		}
	}
}
