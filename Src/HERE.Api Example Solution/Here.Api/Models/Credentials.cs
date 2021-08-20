using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Here.Api
{
	public class Credentials
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

		public static Credentials FromFile(string file)
		{
			//
			// Read the credentials.properties file given by the HERE API.
			//
			Dictionary<string, string> credentials = new(from tbl in File.ReadAllLines(file)
														 let x = tbl.Split(" = ")
														 select new KeyValuePair<string, string>(x[0], x[1]));

			return new Credentials()
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
