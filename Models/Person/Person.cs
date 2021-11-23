/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2020-04-07
/// Modified: -
/// ---------------------------

using AMDB.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMDB.Models
{
    public class Person
    {
        public int PersonId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [NotMapped]
        public string FullName { get => FirstName + " " + LastName; set {; } }

        /// <summary>
        /// Gets and sets the production poster path
        /// </summary>
        public string ProfileImage { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Biography { get; set; }

        public ICollection<DirectorPerson> DirectorCredits { get; set; }

        public ICollection<ProductionPerson> ProductionCredits { get; set; }

        public override string ToString() => FullName;
    }
}
