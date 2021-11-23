using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMDB.Models
{
    public class UserRating
    {
        public int RatingId { get; set; }

        public Rating Rating { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int Value { get; set; }
    }
}
