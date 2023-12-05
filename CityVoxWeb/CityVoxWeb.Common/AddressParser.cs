using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CityVoxWeb.Common
{
    public static class AddressParser
    {
        public static string[] ParseAddress(string address)
        {
            // Regular expression to match the different parts of the address
            // This regex accounts for an optional "zh.k." and street numbers
            // Format: "Street Name [Street Number], [zh.k. Area,] City, Postal Code, Country"
            var regex = new Regex(
                @"^(?<streetname>[^\d,]+)\s*(?<streetnumber>\d*)?,?\s*(zh\.k\.\s*)?(?<area>[^,]*),\s*(?<city>[^,]+)\s+(?<postalcode>\d+),\s*(?<country>.+)$",
                RegexOptions.IgnoreCase);

            var match = regex.Match(address);

            if (!match.Success)
            {
                throw new ArgumentException("The address format is not recognized.");
            }

            // Extracting the matched groups based on the named groups in the regular expression
            var streetName = match.Groups["streetname"].Value.Trim();
            var streetNumber = match.Groups["streetnumber"].Value.Trim();
            var area = match.Groups["area"].Value.Trim();
            var city = match.Groups["city"].Value.Trim();
            var postalCode = match.Groups["postalcode"].Value.Trim();
            var country = match.Groups["country"].Value.Trim();

  

            // Create an array with the extracted values, empty string for missing parts
            return new string[] { streetName, streetNumber, area, city, postalCode, country };
        }
    }
}
