using System;
using System.Collections.Generic;

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
        public DateTime DateOfFlight { get; set; }
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
