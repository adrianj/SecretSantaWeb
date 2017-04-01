using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecretSantaWeb.Models
{
    public class Mapping
    {
        public int MappingID { get; set; }

        public int FamilyID { get; set; }
        public virtual Family Family { get; set; }

        public int GiverID { get; set; }
        public virtual Person Giver { get; set; }

        public int ReceiverID { get; set; }
        public virtual Person Receiver { get; set; }

    }
}