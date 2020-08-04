using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SmallPortal.Models
{
    [NotMapped]
    public class Recipient
    {
        public Metadata metadata { get; set; }
        public string businessName { get; set; }
        public string streetAddress { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postalCode { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string tin { get; set; }
    }
}
