using System;
using System.Collections.Generic;

namespace flip_flop.Models
{
    public partial class TicketsHistory
    {
        public int Key { get; set; }
        public int KeySeller { get; set; }
        public int KeyBuyer { get; set; }
        public int KeyTicket { get; set; }
        public DateTime DateOfTrade { get; set; }

        public Users KeyBuyerNavigation { get; set; }
        public Users KeySellerNavigation { get; set; }
        public PlainTickets KeyTicketNavigation { get; set; }
    }
}
