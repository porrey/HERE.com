using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace Here.Api
{
	public static class Extensions
	{
		public static string Encode(this string value)
		{
			string unreserved = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

			StringBuilder result = new StringBuilder();

			foreach (char symbol in value)
			{
				if (unreserved.IndexOf(symbol) != -1)
				{
					result.Append(symbol);
				}
				else
				{
					string hex = String.Format("{0:X2}", (int)symbol);
					result.Append($"%{hex}");
				}
			}

			return result.ToString();
		}

		public static IDictionary<string, string> AsFiltered(this IDictionary<string, string> parameters)
		{
			var qry = from tbl in parameters
					  where tbl.Key.StartsWith("oauth_")
					  select tbl;

			return new Dictionary<string, string>(qry);
		}

		public static string AsString(this IDictionary<string, string> parameters)
		{
			return String.Join("&", parameters.OrderBy(t => t.Key).ThenBy(t => t.Value).Select(t => $"{t.Key}={t.Value}"));
		}

		public static string AsQuotedString(this IDictionary<string, string> parameters)
		{
			return String.Join(",", parameters.Select(t => $"{t.Key.Encode()}=\"{t.Value.Encode()}\""));
		}

		public static string AsSignatureBase(this IDictionary<string, string> parameters, string method, string requestUrl)
		{
			return $"{method.Encode()}&{requestUrl.Encode()}&{parameters.AsString().Encode()}";
		}

		public static string Sign(this IDictionary<string, string> parameters, HttpMethod method, string requestUrl, string clientKey)
		{
			string returnValue = string.Empty;

			string signatureBase = parameters.AsSignatureBase(method.ToString().ToUpper(), requestUrl);
			HMACSHA256 hmac = new(Encoding.ASCII.GetBytes($"{clientKey}&"));
			returnValue = Convert.ToBase64String(hmac.ComputeHash(Encoding.ASCII.GetBytes(signatureBase)));

			return returnValue;
		}

		public static string Timestamp(this DateTime dateTime)
		{
			return Math.Truncate((dateTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds).ToString();
		}

		public static string AuhorizationHeader(this IDictionary<string, string> parameters)
		{
			return $"OAuth {parameters.AsFiltered().AsQuotedString()}";
		}
	}
}
