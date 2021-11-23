using AMDB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMDB.Models
{
    public class Genre
    {
        public int GenreId { get; set; }

        public string Name { get; set; }

        public ICollection<ProductionGenre> Productions { get; set; }
    }
}
