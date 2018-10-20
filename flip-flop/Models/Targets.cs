using System;
using System.Collections.Generic;

namespace flip_flop.Models
{
    public partial class Targets
    {
        public Targets()
        {
            PlainTickets = new HashSet<PlainTickets>();
        }

        public int Key { get; set; }
        public int CountryName { get; set; }
        public string CityName { get; set; }

        public Countries CountryKeyNavigation { get; set; }
        public ICollection<PlainTickets> PlainTickets { get; set; }
    }
}
