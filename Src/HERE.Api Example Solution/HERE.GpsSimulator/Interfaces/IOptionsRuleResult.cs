using Diamond.Core.Rules;

namespace HERE.GpsSimulator
{
	public interface IOptionsRuleResult : IRuleResult
	{
		string? Parameter { get; set; }
	}
}
