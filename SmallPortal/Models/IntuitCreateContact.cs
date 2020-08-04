using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmallPortal.Models
{
    public class IntuitCreateContact
    {
        public string id { get; set; }
        public long createdDate { get; set; }
        public long updatedDate { get; set; }
        public Metadata metadata { get; set; }
        public string businessName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string streetAddress { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postalCode { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string tin { get; set; }
        public bool readyToSubmit { get; set; }
        public string validationStatus { get; set; }
    }
}
