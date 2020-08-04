using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SmallPortal.Models
{
    [NotMapped]
    public class Recipient1099NecInputModel
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
        [Display(Name = "1 Nonemployee compensation")]
        [Column(TypeName = "money")]
        public decimal box1 { get; set; }
        [Display(Name = "4 Federal income tax withheld")]
        [Column(TypeName = "money")]
        public decimal box4 { get; set; }
        [Display(Name = "5 State tax withheld")]
        [Column(TypeName = "money")]
        public decimal box5 { get; set; }
        [Display(Name = "6 State/Payer’s state no.")]
        public string box6 { get; set; }
        [Display(Name = "7 State income")]
        [Column(TypeName = "money")]
        public decimal box7 { get; set; }
    }
}