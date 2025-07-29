using System.Collections.Generic;

namespace HERE.Api
{
	public class HereIn
	{
		public List<string> Countries { get; set; } = [];

		public override string ToString()
		{
			string returnValue = null;

			if (this.Countries != null && this.Countries.Count > 0)
			{
				returnValue = $"in=countryCode:{string.Join(",", this.Countries)}";
			}

			return returnValue;
		}
	}
}
