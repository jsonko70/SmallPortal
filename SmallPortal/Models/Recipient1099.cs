using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SmallPortal.Models
{
    [NotMapped]
    public class Recipient1099
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id { get; set; }
        //public Actions actions { get; set; }
        public DeliveryOptions deliveryOptions { get; set; }
        public Payer payer { get; set; }
        public Recipient recipient { get; set; }
        public BoxValues boxValues { get; set; }
    }
}