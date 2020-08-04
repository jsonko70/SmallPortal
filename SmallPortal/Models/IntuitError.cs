using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SmallPortal.Models
{
    [NotMapped]
    public class IntuitError
    {
        public string intuit_tid { get; set; }
        public string type { get; set; }
        public long timestamp { get; set; }
        public Error[] errors { get; set; }
    }
    [NotMapped]
    public class Error
    {
        public string field { get; set; }
        public string detail { get; set; }
    }
}