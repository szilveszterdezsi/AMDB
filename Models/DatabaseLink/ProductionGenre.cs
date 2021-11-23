/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2020-04-07
/// Modified: -
/// ---------------------------

namespace AMDB.Models
{
    public class ProductionGenre
    {
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public int ProductionId { get; set; }
        public Production Production { get; set; }
    }
}