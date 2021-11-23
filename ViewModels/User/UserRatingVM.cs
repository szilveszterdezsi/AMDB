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
    public class UserRatingVM
    {
        public int UserId { get; set; }

        public int RatingId { get; set; }

        public int Value { get; set; }
    }
}
