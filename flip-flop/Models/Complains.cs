using System;
using System.Collections.Generic;

namespace flip_flop.Models
{
    public partial class Complains
    {
        public Complains()
        {
            ComplainsStatus = new HashSet<ComplainsStatus>();
        }

        public int Key { get; set; }
        public string Title { get; set; }
        public int UserKey { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }

        public Users UserKeyNavigation { get; set; }
        public ICollection<ComplainsStatus> ComplainsStatus { get; set; }
    }
}
