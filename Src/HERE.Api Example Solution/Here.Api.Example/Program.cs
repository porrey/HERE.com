/*
 *
 * MIT License
 * 
 * Copyright (c) 2021 Daniel Porrey
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
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Here.Api.Example
{
	class Program
	{
		static async Task Main(string[] args)
		{
			//
			// Go to your project at here.com and create OAuth 2.0 (JSON Web Tokens) credentials. This will prompt you
			// to download a file named credentials.properties. Copy it to this project folder.
			//
			HereCredentials credentials = HereCredentials.FromFile("./credentials.properties");
			IHereTokenFactory hereTokenFactory = new HereTokenFactory();
			HereToken token = await hereTokenFactory.CreateTokenAsync(credentials);

			//
			// Get a sample map image.
			//
			using (HttpClient client = hereTokenFactory.CreateHttpClient(token))
			{
				byte[] imageData = await client.GetByteArrayAsync("https://1.base.maps.ls.hereapi.com/maptile/2.1/maptile/newest/normal.day/13/4400/2686/256/png8");

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

			//
			// Use the GeoCode service to find and address.
			//
			IHereGeoCodeService hereGeoCodeService = new HereGeoCodeService();

			(HereGeoCodeList result, HereApiError error) = await hereGeoCodeService.FindAddressAsync(token, new HereAddress()
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
		}
	}
}
