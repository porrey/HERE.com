using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Here.Api
{
	// See
	// https://developer.here.com/documentation/identity-access-management/dev_guide/topics/sdk.html#step-2-create-a-signature
	// for details on how to use OAuth 1.0 to get a token for the here.com REST API.

	public class OAuth1
	{
		public OAuth1(string url, string clientId, string clientKey)
		{
			this.Url = url;
			this.ClientId = clientId;
			this.ClientKey = clientKey;
		}

		protected string Url { get; set; }
		protected string ClientId { get; set; }
		protected string ClientKey { get; set; }

		public async Task<Token> GetTokenAsync()
		{
			Token returnValue = null;

			//
			// Create the nonce.
			//
			string nonce = (new Random()).Next(0, 1999999999).ToString("10000000000");

			//
			// Create the time stamp.
			//
			string timestamp = DateTime.UtcNow.Timestamp();

			//
			// Add all of the parameters
			//
			IDictionary<string, string> parameters = new Dictionary<string, string>
			{
				{ "grant_type", "client_credentials" },
				{ "oauth_consumer_key", this.ClientId},
				{ "oauth_nonce", nonce},
				{ "oauth_signature_method", "HMAC-SHA256" },
				{ "oauth_timestamp", timestamp },
				{ "oauth_version", "1.0" }
			};

			//
			// Get the signed request.
			//
			parameters.Add("oauth_signature", parameters.Sign(HttpMethod.Post, this.Url, this.ClientKey));

			using (HttpClient client = new())
			{
				client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", parameters.AuhorizationHeader());

				//
				// Create the body content.
				//
				using (FormUrlEncodedContent content = new(new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("grant_type", "client_credentials") }))
				{
					//
					// Make the request.
					//
					using (HttpResponseMessage response = await client.PostAsync(this.Url, content))
					{
						//
						// Read the response from the server.
						//
						string responseContent = await response.Content.ReadAsStringAsync();

						if (response.IsSuccessStatusCode)
						{
							//
							// Deserialize the token.
							//
							returnValue = JsonConvert.DeserializeObject<Token>(responseContent);
						}
						else
						{
							//
							// Determine the error and throw an exception.
							//
							string errorMessage = string.Empty;

							errorMessage = response.StatusCode switch
							{
								HttpStatusCode.NotFound => $"The URL '{this.Url}' was not found.",
								HttpStatusCode.Unauthorized => JObject.Parse(responseContent)["error_description"].ToString(),
								_ => "Failed to retrieve token.",
							};

							throw new Exception(errorMessage);
						}
					}
				}
			}

			return returnValue;
		}
	}
}
