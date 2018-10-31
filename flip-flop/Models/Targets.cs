using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [StringLength(60, MinimumLength = 3)]
        public string CityName { get; set; }

        public Countries CountryKeyNavigation { get; set; }
        public ICollection<PlainTickets> PlainTickets { get; set; }
    }
}
