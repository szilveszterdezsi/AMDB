using AMDB.Data;
using AMDB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AMDB.ViewModels
{
    public class SearchResultVM
    {
        public string SearchString { get; set; }

        public ICollection<Production> Movies { get; set; }

        public ICollection<Production> TVShows { get; set; }

        public ICollection<Person> People { get; set; }
    }
}
