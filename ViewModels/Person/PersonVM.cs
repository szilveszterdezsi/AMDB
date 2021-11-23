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
    public class PersonVM
    {
        [Required(ErrorMessage = "Please enter first name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Profile Image")]
        [DataType(DataType.Upload)]
        public IFormFile ProfileImageFormFile { get; set; }

        public string ProfileImageFileName { get; set; }

        [Required(ErrorMessage = "Please select date of birth")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please enter a biography")]
        [Display(Name = "Biography")]
        [DataType(DataType.MultilineText)]
        public string Biography { get; set; }
    }
}
