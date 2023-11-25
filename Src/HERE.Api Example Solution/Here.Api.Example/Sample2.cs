/*
 *
 * MIT License
 * 
 * Copyright (c) 2021-2024 Daniel Porrey
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
using System.Threading.Tasks;

namespace HERE.Api.Example
{
	public class Sample2 : SampleTemplate
	{
		public override async Task<bool> RunAsync(HereHttpClient client)
		{
			bool returnValue = false;

			//
			// Use the GeoCode service to find and address.
			//
			IHereGeoCodeService hereGeoCodeService = new HereGeoCodeService();

			(HereGeoCodeList result, HereApiError error) = await hereGeoCodeService.FindAddressAsync(client, new HereAddress()
			{
				Street = "600 E. Grand Avenue",
				City = "Chicago",
				StateCode = "IL",
				PostalCode = "60611-3419"
			});

			if (error == null)
			{
				returnValue = true;
				Console.WriteLine($"Result: {result.Items[0].Title}, {result.Items[0].Id}");
			}
			else
			{
				Console.WriteLine($"Error: {error.Title}");
			}

			return returnValue;
		}
	}
}
