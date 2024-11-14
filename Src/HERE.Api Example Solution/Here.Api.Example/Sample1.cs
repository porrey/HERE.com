/*
 *
 * MIT License
 * 
 * Copyright (c) 2021-2025 Daniel Porrey
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
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace HERE.Api.Example
{
	public class Sample1 : SampleTemplate
	{
		public override async Task<bool> RunAsync(HereHttpClient client)
		{
			using (client)
			{
				//
				// Get a sample map image.
				//
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

			return true;
		}
	}
}
