using System;
using System.Collections.Generic;

namespace flip_flop.Models
{
    public partial class Users
    {
        public Users()
        {
            Complains = new HashSet<Complains>();
            TicketsHistoryKeyBuyerNavigation = new HashSet<TicketsHistory>();
            TicketsHistoryKeySellerNavigation = new HashSet<TicketsHistory>();
        }

        public int Key { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }

        public ICollection<Complains> Complains { get; set; }
        public ICollection<TicketsHistory> TicketsHistoryKeyBuyerNavigation { get; set; }
        public ICollection<TicketsHistory> TicketsHistoryKeySellerNavigation { get; set; }
    }
}
