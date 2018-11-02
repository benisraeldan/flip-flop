using System;
using System.Collections.Generic;

namespace flip_flop.Models
{
    public partial class ComplainsStatus
    {
        public int Key { get; set; }
        public int ComplainKey { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }

        public Complains ComplainKeyNavigation { get; set; }

        public ComplainsStatus()
        {

        }

        public ComplainsStatus(int complainKey)
        {
            this.ComplainKey = complainKey;
            this.Status = "Waiting";
            this.Comments = "";
        }
    }
}
