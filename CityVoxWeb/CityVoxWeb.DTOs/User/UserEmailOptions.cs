using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.DTOs.User
{
    public class UserEmailOptions
    {
        public UserEmailOptions() 
        { 
            this.ToEmails = new List<string>();
            this.PlaceHolders = new List<KeyValuePair<string, string>>();
        }
        public List<string> ToEmails { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
        public List<KeyValuePair<string, string>> PlaceHolders { get; set; } = null!;
    }
}
