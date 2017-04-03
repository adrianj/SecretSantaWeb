using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecretSantaWeb.Models
{
    public class Person
    {
        public int PersonID { get; set; }

        public string Name { get; set; }

        public int FamilyID { get; set; }
        public virtual Family Family { get; set; }
        
        public virtual List<Exclusion> Exclusions { get; set; }
        

    }
}