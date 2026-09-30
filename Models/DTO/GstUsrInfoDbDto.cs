using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DTO
{
    public class CountRowsInTablesDbDto
    {
        public int NrUsers { get; set; }
        public int NrAttractionsWithReviews { get; set; }
        public int NrAttractionsWithoutReviews { get; set; }
        public int NrTotalAttractions { get; set; }
        public int NrCategories { get; set; }
        public int NrCountries { get; set; }
        public int NrCities { get; set; }
        public int NrReviews { get; set; }
    }
}
