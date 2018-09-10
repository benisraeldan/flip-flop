using System;
using System.Collections.Generic;

namespace flip_flop.Models
{
    public partial class Countries
    {
        public Countries()
        {
            Targets = new HashSet<Targets>();
        }

        public int Key { get; set; }
        public string CountryName { get; set; }

        public ICollection<Targets> Targets { get; set; }
    }
}
