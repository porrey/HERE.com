using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Here.Api
{
	class Program
	{
		static async Task Main(string[] args)
		{
			//
			// Go to your project at here.com and create OAuth 2.0 (JSON Web Tokens) credentials. This will prompt you
			// to download a file named credentials.properties. Copy it to this project folder.
			//

			//
			// Read the credentials.properties file given by the HERE API.
			//
			var credentials = new Dictionary<string, string>(from tbl in File.ReadAllLines("./credentials.properties")
															 let x = tbl.Split(" = ")
															 select new KeyValuePair<string, string>(x[0], x[1]));

			//
			// Create an instance of OAuth1
			//
			OAuth1 oauth = new(credentials["here.token.endpoint.url"], credentials["here.access.key.id"], credentials["here.access.key.secret"]);
			Token token = await oauth.GetTokenAsync();

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

				//
				// Get the image.
				//
				byte[] image = await client.GetByteArrayAsync("https://1.base.maps.ls.hereapi.com/maptile/2.1/maptile/newest/normal.day/13/4400/2686/256/png8");

				//
				// Save the image to a temporary file.
				//
				string tempFile = $"{Path.GetTempFileName()}.png";
				await File.WriteAllBytesAsync(tempFile, image);

				//
				// Open the image with the default system viewer.
				//
				Process.Start(new ProcessStartInfo(tempFile) { UseShellExecute = true });
			}
		}
	}
}
