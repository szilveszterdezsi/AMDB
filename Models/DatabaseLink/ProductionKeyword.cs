using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMDB.Models
{
    public class ProductionKeyword
    {
        public int KeywordId { get; set; }
        public Keyword Keyword { get; set; }
        public int ProductionId { get; set; }
        public Production Production { get; set; }
    }
}
