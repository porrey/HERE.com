using Diamond.Core.Workflow;
using Microsoft.Extensions.Logging;

namespace HERE.GpsSimulator
{
	public abstract class TemplateWorkflowStep : WorkflowItem
	{
		public TemplateWorkflowStep(ILogger<TemplateWorkflowStep> logger)
			: base(logger)
		{
		}

		protected void Render(IContext context, FormattableString value) => context.Properties.Get<IConsoleRenderer>(WellKnown.Context.Renderer).Render(value);
	}
}
