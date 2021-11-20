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
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HERE.Api
{
	public class HereTokenFactory : IHereTokenFactory
	{
		public HereToken CreateToken(HereCredentials credentials)
		{
			return this.CreateTokenAsync(credentials).Result;
		}

		public Task<HereToken> CreateTokenAsync(HereCredentials credentials)
		{
			//
			// Create an instance of OAuth1
			//
			HereOAuth1 oauth = new(credentials.TokenEndPointUrl, credentials.AccessKeyId, credentials.AccessKeySecret);
			return oauth.GetTokenAsync();
		}

		public HereHttpClient CreateHttpClient(HereToken token)
		{
			HereHttpClient client = new()
			{
				Token = token
			};

			//
			// Set up the headers.
			//
			client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", string.Format("Bearer {0}", token.AccessToken));
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			return client;
		}

		public Task<HereHttpClient> CreateHttpClientAsync(HereToken token)
		{
			return Task.FromResult(this.CreateHttpClient(token));
		}
	}
}
