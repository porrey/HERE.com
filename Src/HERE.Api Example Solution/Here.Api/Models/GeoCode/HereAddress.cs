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
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace HERE.Api
{
	public class HereAddress
	{
		[JsonProperty("label")]
		public string Label { get; set; }

		[JsonProperty("street")]
		public string Street { get; set; }

		[JsonProperty("city")]
		public string City { get; set; }

		[JsonProperty("state")]
		public string State { get; set; }

		[JsonProperty("stateCode")]
		public string StateCode { get; set; }

		[JsonProperty("postalCode")]
		public string PostalCode { get; set; }

		[JsonProperty("county")]
		public string County { get; set; }

		[JsonProperty("district")]
		public string District { get; set; }

		[JsonProperty("houseNumber")]
		public string HouseNumber { get; set; }

		[JsonProperty("countryName")]
		public string CountryName { get; set; }

		[JsonProperty("in")]
		public HereIn In { get; set; }

		public string BuildUrl()
		{
			string returnValue = null;

			IList<string> items = new List<string>();

			if (!string.IsNullOrWhiteSpace(this.Street))
			{
				items.Add($"street={this.Street}");
			}

			if (!string.IsNullOrWhiteSpace(this.City))
			{
				items.Add($"city={this.City}");
			}

			if (!string.IsNullOrWhiteSpace(this.StateCode))
			{
				//
				// The parameter is state and not stateCode.
				//
				items.Add($"state={this.StateCode}");
			}

			if (!string.IsNullOrWhiteSpace(this.PostalCode))
			{
				items.Add($"postalCode={this.PostalCode}");
			}

			if (!string.IsNullOrWhiteSpace(this.County))
			{
				items.Add($"county={this.County}");
			}

			if (!string.IsNullOrWhiteSpace(this.District))
			{
				items.Add($"district={this.District}");
			}

			if (!string.IsNullOrWhiteSpace(this.HouseNumber))
			{
				items.Add($"houseNumber={this.HouseNumber}");
			}

			returnValue = string.Join(";", items);

			return returnValue;
		}

		public override string ToString()
		{
			string returnValue = null;

			//
			// Get the URL items.
			//
			string[] items = new string[]
			{
				this.BuildUrl(),
				this.In!=null ? this.In.ToString() : null
			};

			//
			// Build the URL; remove any null items.
			returnValue = $"{string.Join("&", items.Where(t => t != null))}";

			return returnValue;
		}
	}
}
