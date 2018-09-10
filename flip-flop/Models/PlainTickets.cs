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
        public int TargetKey { get; set; }
        public DateTime DateOfFlight { get; set; }
        public string FlightNumber { get; set; }
        public int OwnerId { get; set; }
        public int CancleFee { get; set; }
        public bool Food { get; set; }
        public bool Baggage { get; set; }
        public int ClassKey { get; set; }
        public int Price { get; set; }

        public ICollection<TicketsHistory> TicketsHistory { get; set; }
    }
}
