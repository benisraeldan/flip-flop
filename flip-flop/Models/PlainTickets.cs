using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace flip_flop.Models
{
    public partial class PlainTickets
    {
        public PlainTickets()
        {
            TicketsHistory = new HashSet<TicketsHistory>();
        }

        public int Key { get; set; }

      
        public int Target { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        [Required(ErrorMessage = "Date is mandatory")]
        [RestrictedDate]
        public DateTime DateOfFlight { get; set; }

        [StringLength(10)]
        public string FlightNumber { get; set; }
        public int OwnerId { get; set; }
        public int CancleFee { get; set; }
        public bool Food { get; set; }
        public bool Baggage { get; set; }
        public int Class { get; set; }

        public int Price { get; set; }
        public bool IsSold { get; set; }

        public Department ClassKeyNavigation { get; set; }
        public Users Owner { get; set; }
        public Targets TargetKeyNavigation { get; set; }
        public ICollection<TicketsHistory> TicketsHistory { get; set; }
    }
}

public class RestrictedDate : ValidationAttribute
{
    public override bool IsValid(object date)
    {
        DateTime datea = (DateTime)date;
        return datea > DateTime.Now;
    }
}
