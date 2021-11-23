/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2020-04-07
/// Modified: -
/// ---------------------------

using System;
using System.Collections.Generic;

namespace AMDB.Models
{
    /// <summary>
    /// Interface for implementation by abstract class Production.
    /// </summary>
    interface IProduction
    {
        /// <summary>
        /// Gets and sets the production ID
        /// </summary>
        int ProductionId { get; set; }

        /// <summary>
        /// Gets and sets the production title
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets and sets the production release date
        /// </summary>
        DateTime ReleaseDate { get; set; }

        /// <summary>
        /// Gets and sets the production poster image
        /// </summary>
        string PosterImage { get; set; }

        /// <summary>
        /// Gets and sets the production rating
        /// </summary>
        Rating Rating { get; set; }

        /// <summary>
        /// Gets and sets the production genres
        /// </summary>
        ICollection<ProductionGenre> Genres { get; set; }

        /// <summary>
        /// Gets and sets the production runtime
        /// </summary>
        TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets and sets the production director
        /// </summary>
        DirectorPerson Director { get; set; }

        /// <summary>
        /// Gets and sets the production stars list
        /// </summary>
        ICollection<ProductionPerson> Stars { get; set; }

        /// <summary>
        /// Gets and sets the production description
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets and sets the production trailer link
        /// </summary>
        string TrailerURL { get; set; }

        /// <summary>
        /// Gets and sets the production keyword list
        /// </summary>
        ICollection<ProductionKeyword> Keywords { get; set; }
    }
}
