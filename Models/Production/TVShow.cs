using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMDB.Models
{
    public class TVShow : Production
    {
        public int Seasons { get; set; }
        public DateTime EndDate { get; set; }
    }
}
