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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace HERE.Api
{
	public class HereCredentials
	{
		[JsonProperty("userId")]
		public string UserId { get; set; }

		[JsonProperty("clientId")]
		public string ClientId { get; set; }

		[JsonProperty("accessKeyId")]
		public string AccessKeyId { get; set; }

		[JsonProperty("accessKeySecret")]
		public string AccessKeySecret { get; set; }

		[JsonProperty("tokenEndPointUrl")]
		public string TokenEndPointUrl { get; set; }

		public static class Key
		{
			public const string UserId = "here.user.id";
			public const string ClientId = "here.client.id";
			public const string AccessKeyId = "here.access.key.id";
			public const string AccessKeySecret = "here.access.key.secret";
			public const string TokenEndPointUrl = "here.token.endpoint.url";
		}

		public static HereCredentials FromFile(string file)
		{
			//
			// Read the credentials.properties file given by the HERE API.
			//
			Dictionary<string, string> credentials = new(from tbl in File.ReadAllLines(file)
														 let x = tbl.Split(" = ")
														 select new KeyValuePair<string, string>(x[0], x[1]));

			return new HereCredentials()
			{
				UserId = credentials.ContainsKey(Key.UserId) ? credentials[Key.UserId] : throw new KeyNotFoundException(Key.UserId),
				ClientId = credentials.ContainsKey(Key.ClientId) ? credentials[Key.ClientId] : throw new KeyNotFoundException(Key.ClientId),
				AccessKeyId = credentials.ContainsKey(Key.AccessKeyId) ? credentials[Key.AccessKeyId] : throw new KeyNotFoundException(Key.AccessKeyId),
				AccessKeySecret = credentials.ContainsKey(Key.AccessKeySecret) ? credentials[Key.AccessKeySecret] : throw new KeyNotFoundException(Key.AccessKeySecret),
				TokenEndPointUrl = credentials.ContainsKey(Key.TokenEndPointUrl) ? credentials[Key.TokenEndPointUrl] : throw new KeyNotFoundException(Key.TokenEndPointUrl)
			};
		}
	}
}
