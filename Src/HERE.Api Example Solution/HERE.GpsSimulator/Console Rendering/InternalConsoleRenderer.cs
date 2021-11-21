using System.CommandLine.Invocation;
using System.CommandLine.Rendering;

namespace HERE.GpsSimulator
{
	public class InternalConsoleRenderer : IConsoleRenderer
	{
		public InternalConsoleRenderer(InvocationContext ic)
		{
			this.InvocationContext = ic;

			this.ConsoleRenderer = new ConsoleRenderer(
									   this.InvocationContext.Console,
									   mode: this.InvocationContext.BindingContext.OutputMode(),
									   resetAfterRender: true);
		}

		public InvocationContext InvocationContext { get; }
		public Region? Region { get; set; }
		public ConsoleRenderer ConsoleRenderer { get; }

		public void Render(FormattableString value)
		{
			this.Region = new(0, Console.CursorTop + 1,
							  Console.WindowWidth,
							  Console.WindowHeight,
							  false);

			this.ConsoleRenderer.RenderToRegion(value, this.Region);
		}
	}
}
