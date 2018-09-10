using System;
using System.Collections.Generic;

namespace flip_flop.Models
{
    public partial class Targets
    {
        public int Key { get; set; }
        public int CountryKey { get; set; }
        public string CityName { get; set; }

        public Countries CountryKeyNavigation { get; set; }
    }
}
