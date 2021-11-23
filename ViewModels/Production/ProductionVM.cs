using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMDB.ViewModels
{
    public class ProductionVM
    {
        [Required(ErrorMessage = "Please enter title")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please select release date")]
        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Poster Image")]
        [DataType(DataType.Upload)]
        public IFormFile PosterFormFile { get; set; }

        public string PosterFileName { get; set; }

        [Required(ErrorMessage = "Please select runtime")]
        [Display(Name = "Duration")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "@{0:hh\\:mm}", ApplyFormatInEditMode = false)]
        public TimeSpan Duration { get; set; }

        public IEnumerable<SelectListItem> AllGenres { get; set; }

        [Required(ErrorMessage = "Please select at least one genre")]
        [Display(Name = "Genre(s)")]
        public IEnumerable<int> SelectedGenres { get; set; }

        [Required(ErrorMessage = "Please enter YouTube trailer URL")]
        [Display(Name = "YouTube Trailer URL")]
        [DataType(DataType.Url)]
        [RegularExpression(@"^((?:https?:)?\/\/)?((?:www|m)\.)?((?:youtube\.com|youtu.be))(\/(?:[\w\-]+\?v=|embed\/|v\/)?)([\w\-]+)(\S+)?$", ErrorMessage = "This is not a valid YouTube URL")]
        public string TrailerURL { get; set; }

        [Required(ErrorMessage = "Please enter a description")]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please select a director")]
        [Display(Name = "Director")]
        public int Director { get; set; }

        public IEnumerable<SelectListItem> AllPersons { get; set; }

        [Required(ErrorMessage = "Please select at least one star")]
        [Display(Name = "Star(s)")]
        public IEnumerable<int> SelectedStars { get; set; }

        public IEnumerable<SelectListItem> AllKeywords { get; set; }

        [Required(ErrorMessage = "Please select at least one keyword")]
        [Display(Name = "Keyword(s)")]
        public IEnumerable<string> SelectedKeywords { get; set; }
    }
}
