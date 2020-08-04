using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SmallPortal.Models
{
    [NotMapped]
    public class BoxValues
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id { get; set; }
        [Column(TypeName = "money")]
        public decimal box1 { get; set; }
        [Column(TypeName = "money")]
        public decimal box2 { get; set; }
        [Column(TypeName = "money")]
        public decimal box3 { get; set; }
        [Column(TypeName = "money")]
        public decimal box4 { get; set; }
        [Column(TypeName = "money")]
        public decimal box5 { get; set; }
        [Column(TypeName = "money")]
        public decimal box6 { get; set; }
        public bool box7 { get; set; }
        [Column(TypeName = "money")]
        public decimal box8 { get; set; }
        [Column(TypeName = "money")]
        public decimal box9 { get; set; }
        [Column(TypeName = "money")]
        public decimal box10 { get; set; }
        [Column(TypeName = "money")]
        public decimal box12 { get; set; }
        [Column(TypeName = "money")]
        public decimal box13 { get; set; }
        [Column(TypeName = "money")]
        public decimal box14 { get; set; }
        [Column(TypeName = "money")]
        public decimal box15 { get; set; }
        public string box16 { get; set; }
        [Column(TypeName = "money")]
        public decimal box17 { get; set; }
    }
}