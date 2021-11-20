using Diamond.Core.CommandLine.Model;
using Microsoft.Extensions.Logging;
using Diamond.Core.Workflow;

namespace HERE.GpsSimulator
{
	public class GenerateCommandHandler : ModelCommand<OptionsViewModel>
	{
		public GenerateCommandHandler(ILogger<GenerateCommandHandler> logger, IWorkflowManagerFactory workflowManagerFactory) :
			base(logger, "generate", "Generates GPS pings from a specified origin point to a destination point.")
		{
			this.WorkflowManagerFactory = workflowManagerFactory;
		}

		protected IWorkflowManagerFactory WorkflowManagerFactory { get; set; }

		protected override async Task<int> OnHandleCommand(OptionsViewModel item)
		{
			int returnValue = 0;

			//
			// Get the workflow.
			//
			IWorkflowManager workflow = await this.WorkflowManagerFactory.GetAsync(WellKnown.Workflow.Command);

			using (WorkflowContext workflowContext = new())
			{
				this.Logger.LogInformation("Starting command.");

				//
				// Place the arguments in the context.
				//
				workflowContext.Properties.Set(WellKnown.Context.Options, item);

				try
				{
					if (await workflow.ExecuteWorkflowAsync(workflowContext))
					{
						this.Logger.LogInformation("The command completed successfully.");
					}
					else
					{
						this.Logger.LogError(workflowContext.GetException(), "The command completed with errors: '{message}'.", workflowContext.FailureMessage());
						returnValue = 1;
					}
				}
				catch (WorkflowException ex)
				{
					this.Logger.LogError(ex, "The command completed with an exception.");
					returnValue = 2;
				}
			}

			return returnValue;
		}
	}
}
