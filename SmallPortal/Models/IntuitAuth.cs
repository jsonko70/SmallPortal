using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmallPortal.Models
{
    public class IntuitAuth
    {
        public string Token_type { get; set; }
        public string Access_token { get; set; }
        public int Expires_in { get; set; }
    }
}
