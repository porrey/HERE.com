using System.CommandLine;
using System.CommandLine.Invocation;
using Diamond.Core.CommandLine;
using Diamond.Core.CommandLine.Model;
using Diamond.Core.Workflow;
using Microsoft.Extensions.Logging;

namespace HERE.GpsSimulator
{
	public class GenerateCommandHandler : ModelCommand<OptionsViewModel>
	{
		public GenerateCommandHandler(ILogger<GenerateCommandHandler> logger, IRootCommand rootCommand, IWorkflowManagerFactory workflowManagerFactory) :
			base(logger, "generate", "Generates GPS pings from a specified origin point to a destination point.")
		{
			this.RootCommand = rootCommand;
			this.WorkflowManagerFactory = workflowManagerFactory;
		}

		protected IRootCommand RootCommand { get; set; }
		protected IWorkflowManagerFactory WorkflowManagerFactory { get; set; }

		protected override async Task<int> OnHandleCommand(OptionsViewModel item)
		{
			int returnValue = 0;

			//
			// Initialize the renderer for displaying console messages.
			//
			IConsoleRenderer renderer = await this.IntializeConsoleRendering();

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
				workflowContext.Properties.Set(WellKnown.Context.Renderer, renderer);

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

		protected Task<IConsoleRenderer> IntializeConsoleRendering()
		{
			InvocationContext ic = new(((RootCommand)this.RootCommand).Parse(this.RootCommand.Args));
			return Task.FromResult<IConsoleRenderer>(new InternalConsoleRenderer(ic));
		}
	}
}
