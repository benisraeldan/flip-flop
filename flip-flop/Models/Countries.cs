using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace flip_flop.Models
{
    public partial class Countries
    {
        public Countries()
        {
            Targets = new HashSet<Targets>();
        }

        public int Key { get; set; }

        [StringLength(60, MinimumLength = 3)]
        public string CountryName { get; set; }

        public ICollection<Targets> Targets { get; set; }
    }

}
