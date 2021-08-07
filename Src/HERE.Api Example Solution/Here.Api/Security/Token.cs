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
