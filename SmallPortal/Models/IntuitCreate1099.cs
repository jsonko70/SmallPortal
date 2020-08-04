using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SmallPortal.Models
{
    [NotMapped]
    public class IntuitCreate1099
    {
        public string id { get; set; }
        public Metadata1099 metadata { get; set; }
        public DeliveryOptions deliveryOptions { get; set; }
        public Actions actions { get; set; }
        public Payer payer { get; set; }
        public Recipient recipient { get; set; }
        public BoxValues boxValues { get; set; }
        public BoxValuesNec boxValuesNec { get; set; }
        public string validationStatus { get; set; }
        public Statuses statuses { get; set; }
    }
    [NotMapped]
    public class Metadata1099
    {
        public string companyId { get; set; }
        public string payerId { get; set; }
    }
    [NotMapped]
    public class Actions
    {
        public bool submit { get; set; }
    }
    [NotMapped]
    public class Statuses
    {
        public Mail[] mail { get; set; }
        public Efile[] efile { get; set; }
    }
    [NotMapped]
    public class Mail
    {
        public string status { get; set; }
        public string message { get; set; }
        public long updatedDate { get; set; }
    }
    [NotMapped]
    public class Efile
    {
        public string status { get; set; }
        public string message { get; set; }
        public long updatedDate { get; set; }
    }

}