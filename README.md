![](https://github.com/porrey/HERE.com/raw/main/Images/logo.png)

## HERE.GpsSimulator
The example code demonstrates how to use the HERE API to create a route between two points and then simulate GPS pings along the route and a specified interval. The output is written to a JSON file. The code sample is written for truck routing but can be esily adapted for other modes of transportation.

Example command line:

	HERE.GpsSimulator generate --origin "600 E Grand Avenue, Chicago IL" --destination "1300 Fox St Denver CO 80204" 
	--departuretime "11/21/2021 11:00 AM" --grossweight 75000 --interval 00:15:00 --axlecount 5 --trailercount 1 
	--trucktype tractor --routingmode short --regulation true --lastrest 2 
	--outputfile "%USERPROFILE%/Desktop/gps-data.json"

Type `HERE.GpsSimulator -h` for parameter info. You will get the output shown below.

	HERE.GpsSimulator
	
	Usage:
  	HERE.GpsSimulator [options] [command]

	Options:
  		--version       Show version information
  		-?, -h, --help  Show help and usage information

	Commands:
  		generate  Generates GPS pings from a specified origin point to a destination point.
  
For specific command help, type `HERE.GpsSimulator -h [command]`. For example, `1 gives the output shown below.

	generate
	  Generates GPS pings from a speified origin point to a destination point.

	Usage:
	  HERE.GpsSimulator [options] generate
	
	Options:
	  -o, --origin <origin> (REQUIRED)            A location defining the origin of the trip.
	  -d, --destination <destination> (REQUIRED)  A location defining the destination of the trip.
	  -p, --departuretime <departuretime>         Specifies the time of departure. The default is the current date and time.
	  -g, --grossweight <grossweight>             Vehicle weight including trailers and shipped goods, in LBS. The default is 75,000.
	  -a, --axlecount <axlecount>                 Defines total number of axles in the vehicle. Default is 5.
	  -c, --trailercount <trailercount>           Number of trailers attached to the vehicle. Default is 1.
	  -t, --type <type>                           Specifies the type of truck. Possible values are 'Straight' or 'Tractor'. The default is Tractor.
	  -m, --routingmode <routingmode>             Specifies which optimization is applied during route calculation. Can be 'Fast' or 'Short'. The default is Short.
	  -i, --interval <interval>                   Specifies the interval used to generate GPS updates. The default is every 15 minutes.
	  -r, --regulation <regulation>               Specifies whether US driver regulations should be observed. The default is True.
	  -h, --lastrest <lastrest>                   Specifies the number of hours since the driver's last rest before picking up the load. The default is 0.
	  -f, --outputfile <outputfile>               Specifies the full path to the JSON output file.
	  -?, -h, --help                              Show help and usage information                                 Show help and usage information


## HERE.Api and HERE.Api.Example

Example code to Authenticate to the HERE.com REST API in C# using OAuth 1.0 and OAuth 2.0. Also demonstrates a map request and Geo Code request.
	
	using System;
	using System.Diagnostics;
	using System.IO;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Threading.Tasks;

	namespace Here.Api.Example
	{
		class Program
		{
			static async Task Main(string[] args)
			{
				//
				// Go to your project at here.com and create OAuth 2.0 (JSON Web Tokens) credentials. This will prompt you
				// to download a file named credentials.properties. Copy it to this project folder.
				//
				HereCredentials credentials = HereCredentials.FromFile("./credentials.properties");
				IHereTokenFactory hereTokenFactory = new HereTokenFactory();
				HereToken token = await hereTokenFactory.CreateTokenAsync(credentials);

				//
				// Get a sample map image.
				//
				using (HttpClient client = hereTokenFactory.CreateHttpClient(token))
				{
					byte[] imageData = await client.GetByteArrayAsync("https://1.base.maps.ls.hereapi.com/maptile/2.1/maptile/newest/normal.day/13/4400/2686/256/png8");

					//
					// Save the image to a temporary file.
					//
					string tempFile = $"{Path.GetTempFileName()}.png";
					await File.WriteAllBytesAsync(tempFile, imageData);

					//
					// Open the image with the default system viewer.
					//
					Process.Start(new ProcessStartInfo(tempFile) { UseShellExecute = true });
				}

				//
				// Use the GeoCode service to find and address.
				//
				IHereGeoCodeService hereGeoCodeService = new HereGeoCodeService();

				(HereGeoCodeList result, HereApiError error) = await hereGeoCodeService.FindAddressAsync(token, new HereAddress()
				{
					Street = "600 E. Grand Avenue",
					City = "Chicago",
					State = "IL"
				});

				if (error == null)
				{
					Console.WriteLine($"Result: {result.Items[0].Title}, {result.Items[0].Id}");
				}
				else
				{
					Console.WriteLine($"Error: {error.Title}");
				}
			}
		}
	}
