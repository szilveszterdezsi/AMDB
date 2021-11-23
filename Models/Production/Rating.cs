using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AMDB.Models
{
    public class Rating
    {
        public int RatingId { get; set; }

        public double AverageVote { get => UserRatings.Count == 0 ? 0.0 : UserRatings.Average(x => x.Value); }

        public ICollection<UserRating> UserRatings { get; set; }
    }
}
