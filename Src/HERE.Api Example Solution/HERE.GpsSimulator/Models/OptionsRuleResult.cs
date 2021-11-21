using Diamond.Core.Rules;

namespace HERE.GpsSimulator
{
	public class OptionsRuleResult : RuleResultTemplate, IOptionsRuleResult
	{
		public string? Parameter { get; set; }
	}
}
