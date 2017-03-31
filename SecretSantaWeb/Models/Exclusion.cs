using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecretSantaWeb.Models
{
    public class Exclusion
    {
        public int ExclusionID { get; set; }

        public int NotBuyingForID { get; set; }

        public virtual Person NotBuyingFor { get; set; }

        public int OwnerID { get; set; }

        public virtual Person Owner { get; set; }
    }
}