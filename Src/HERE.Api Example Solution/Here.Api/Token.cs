using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace Here.Api
{
	public class Token
	{
		public Token()
		{
			this.CreatedDateTime = DateTime.Now;
		}

		[Description("A token you can use to authenticate REST requests.")]
		[JsonProperty("access_token")]
		public string AccessToken { get; set; }

		[Description("The type of token issued by the Authentication and Authorization API. This value will always be 'bearer' since the API issues bearer tokens.")]
		[JsonProperty("token_type")]
		public string TokenType { get; set; }

		[Description("The number of seconds until the token expires. Tokens expire 24 hours after they are issued.")]
		[JsonProperty("expires_in")]
		public int ExpiresIn { get; set; }

		public DateTime CreatedDateTime { get; protected set; }

		public override string ToString()
		{
			DateTime expires = this.CreatedDateTime.AddSeconds(this.ExpiresIn);
			return $"Expires on {expires.ToLongDateString()} at {expires.ToLongTimeString()}";
		}
	}
}
