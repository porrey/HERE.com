/*
 *
 * MIT License
 * 
 * Copyright (c) 2021-2022 Daniel Porrey
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this
 * software and associated documentation files (the "Software"), to deal in the Software
 * without restriction, including without limitation the rights to use, copy, modify, merge,
 * publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
 * to whom the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
 * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 */
using Diamond.Core.CommandLine;
using Diamond.Core.Extensions.DependencyInjection;
using Diamond.Core.Extensions.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

//
// See https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-5.0
// for details on host based applications.
//

namespace HERE.GpsSimulator
{
	/// <summary>
	/// 
	/// </summary>
	public class Program
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		static Task<int> Main(string[] args) => Host.CreateDefaultBuilder(args)
							.AddRootCommand("HERE GPS Simulator", args)
							.UseStartup<ConsoleStartup>()
							.UseSerilog()
							.ConfigureServicesFolder("Services")
							.UseConfiguredServices()
							.UseConsoleLifetime()
							.Build()
							.RunWithExitCodeAsync();
	}
}