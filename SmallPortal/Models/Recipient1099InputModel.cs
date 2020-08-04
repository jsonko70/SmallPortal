using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SmallPortal.Models
{
    [NotMapped]
    public class Recipient1099InputModel
    {

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [Key]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Business Name")]
        public string BusinessName { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }
        //[RegularExpression("^\\d{3}-\\d{2}-\\d{4}$",
        //                ErrorMessage = "Invalid TaxId Number")]
        [Display(Name = "Tax ID Number")]
        public string TaxIDNumber { get; set; }
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        [Display(Name = "1 Rents")]
        [Column(TypeName = "money")]
        public decimal box1 { get; set; }
        [Display(Name = "2 Royalties")]
        [Column(TypeName = "money")]
        public decimal box2 { get; set; }
        [Display(Name = "3 Other income")]
        [Column(TypeName = "money")]
        public decimal box3 { get; set; }
        [Display(Name = "4 Federal income tax withheld")]
        [Column(TypeName = "money")]
        public decimal box4 { get; set; }
        [Display(Name = "5 Fishing boat proceeds")]
        [Column(TypeName = "money")]
        public decimal box5 { get; set; }
        [Display(Name = "6 Medical and health care payments")]
        [Column(TypeName = "money")]
        public decimal box6 { get; set; }
        [Display(Name = "7 Payer made direct sales of $5000 or more of consumer products to a buyer(recipient) for resale")]
        public bool box7 { get; set; }
        [Display(Name = "8 Substitute payments in lieu of dividends or interest")]
        [Column(TypeName = "money")]
        public decimal box8 { get; set; }
        [Display(Name = "9 Crop insurance proceeds")]
        [Column(TypeName = "money")]
        public decimal box9 { get; set; }
        [Display(Name = "10 Gross proceeds paid to an attorney")]
        [Column(TypeName = "money")]
        public decimal box10 { get; set; }
        [Display(Name = "12 Section 409A deferrals")]
        [Column(TypeName = "money")]
        public decimal box12 { get; set; }
        [Display(Name = "13 Excess golden parachute payments")]
        [Column(TypeName = "money")]
        public decimal box13 { get; set; }
        [Display(Name = "14 Nonqualified deferred compensation")]
        [Column(TypeName = "money")]
        public decimal box14 { get; set; }
        [Display(Name = "15 State tax withheld")]
        [Column(TypeName = "money")]
        public decimal box15 { get; set; }
        [Display(Name = "16 State/Payer’s state no.")]
        //[Column(TypeName = "money")]
        public string box16 { get; set; }
        [Display(Name = "17 State income")]
        [Column(TypeName = "money")]
        public decimal box17 { get; set; }
    }

}