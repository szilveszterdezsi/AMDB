using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMDB.Models
{
    public class DirectorPerson
    {
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public int ProductionId { get; set; }
        public Production Production { get; set; }
    }
}
