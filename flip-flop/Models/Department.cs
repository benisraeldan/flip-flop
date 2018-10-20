using System;
using System.Collections.Generic;

namespace flip_flop.Models
{
    public partial class Department
    {
        public Department()
        {
            PlainTickets = new HashSet<PlainTickets>();
        }

        public int Key { get; set; }
        public string Type { get; set; }

        public ICollection<PlainTickets> PlainTickets { get; set; }
    }
}
