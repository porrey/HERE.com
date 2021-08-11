![](https://github.com/porrey/HERE.com/raw/main/Images/logo.png)

## HERE.com

Example code to Authenticate to the HERE.com REST API in C# using OAuth 1.0 and OAuth 2.0. Also demonstrates a map request and Geo Code request.


	using System;
	using System.Diagnostics;
	using System.IO;
	using System.Threading.Tasks;
	using Here.Api.Models;
	
	namespace Here.Api
	{
		class Program
		{
			static async Task Main(string[] args)
			{
				//
				// Find and address.
				//
				(GeoCodeList result, ApiError error) = await Api.CheckAddressAsync(new Address()
				{
					Street = "600 E. Grand Avenue",
					City = "Chicago",
					State = "IL"
				});
	
				if (error == null)
				{
					Console.WriteLine($"Result: {result.Items[0].Title}, {result.Items[0].Id}");
				}
				else
				{
					Console.WriteLine($"Error: {error.Title}");
				}
	
				//
				// Get a sample map image.
				//
				byte[] imageData = await Api.GetSampleMapImageAsync();
	
				//
				// Save the image to a temporary file.
				//
				string tempFile = $"{Path.GetTempFileName()}.png";
				await File.WriteAllBytesAsync(tempFile, imageData);
	
				//
				// Open the image with the default system viewer.
				//
				Process.Start(new ProcessStartInfo(tempFile) { UseShellExecute = true });
			}
		}
	}