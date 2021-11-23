using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AMDB.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool SignedIn { get; set; }

        public ICollection<UserRating> ProductionRatings { get; set; }

        [NotMapped]
        public string FullName { get => FirstName + " " + LastName; }

        public override string ToString() => FullName;
    }
}
