using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SmallPortal.Models
{
    [NotMapped]
    public class BoxValuesNec

    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id { get; set; }
        [Column(TypeName = "money")]
        public decimal box1 { get; set; }
        [Column(TypeName = "money")]
        public decimal box4 { get; set; }
        [Column(TypeName = "money")]
        public decimal box5 { get; set; }
        public string box6 { get; set; }
        [Column(TypeName = "money")]
        public decimal box7 { get; set; }
    }
}