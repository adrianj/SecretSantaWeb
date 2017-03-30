using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecretSantaWeb.Models
{
    public class Family
    {
        public int FamilyID { get; set; }
        public string FamilyName { get; set; }

        public virtual List<Person> FamilyMembers { get; set; }

    }
}