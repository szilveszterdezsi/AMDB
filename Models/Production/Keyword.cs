using AMDB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMDB.Models
{
    public class Keyword
    {
        public int KeywordId { get; set; }

        public string Name { get; set; }

        public ICollection<ProductionKeyword> Productions { get; set; }
    }
}
