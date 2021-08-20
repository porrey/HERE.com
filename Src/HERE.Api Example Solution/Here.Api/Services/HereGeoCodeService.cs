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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Here.Api
{
	public class HereGeoCodeService : IHereGeoCodeService
	{
		public HereGeoCodeService()
			:this("https://geocode.search.hereapi.com/v1/geocode")
		{
		}

		public HereGeoCodeService(string baseUrl)
		{
			this.Baseurl = baseUrl;
		}

		protected string Baseurl { get; set; }

		public async Task<(HereGeoCodeList, HereApiError)> FindAddressAsync(HereToken token, HereAddress address)
		{
			(HereGeoCodeList addressResponse, HereApiError error) = (null, null);

			//
			// Make and API call.
			//
			using (HttpClient client = new())
			{
				//
				// Set up the headers.
				//
				client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", string.Format("Bearer {0}", token.AccessToken));
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				string uri = $"{this.Baseurl}?qq={address}";
				using (HttpResponseMessage response = await client.GetAsync(uri))
				{
					string json = await response.Content.ReadAsStringAsync();

					if (response.IsSuccessStatusCode)
					{
						addressResponse = JsonConvert.DeserializeObject<HereGeoCodeList>(json);
					}
					else
					{
						error = JsonConvert.DeserializeObject<HereApiError>(json);
					}
				}
			}

			return (addressResponse, error);
		}
	}
}
