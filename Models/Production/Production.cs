/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2020-04-07
/// Modified: -
/// ---------------------------

using AMDB.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMDB.Models
{
    /// <summary>
    /// Abstract class for handling Movie and TV Show productions.
    /// </summary>
    public abstract class Production : IProduction
    {
        /// <summary>
        /// Gets and sets the production ID
        /// </summary>
        public int ProductionId { get; set; }

        /// <summary>
        /// Gets and sets the production title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets and sets the production release date
        /// </summary>
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// Gets and sets the production poster path
        /// </summary>
        public string PosterImage { get; set; }

        /// <summary>
        /// Gets and sets the production rating
        /// </summary>
        public Rating Rating { get; set; }

        /// <summary>
        /// Gets and sets the production genres
        /// </summary>
        public ICollection<ProductionGenre> Genres { get; set; }

        /// <summary>
        /// Gets and sets the production runtime
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets and sets the production director
        /// </summary>
        public DirectorPerson Director { get; set; }

        /// <summary>
        /// Gets and sets the production stars
        /// </summary>
        public ICollection<ProductionPerson> Stars { get; set; }

        /// <summary>
        /// Gets and sets the production description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets and sets the production trailer link
        /// </summary>
        public string TrailerURL { get; set; }

        /// <summary>
        /// Gets and sets the production tag list
        /// </summary>
        public ICollection<ProductionKeyword> Keywords { get; set; }

        [NotMapped]
        public ICollection<Production> Suggestions { get; set; }
    }
}
