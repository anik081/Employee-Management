using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeInfoSystem.Models
{
    public class RatingModel
    {
        public string IMDbId { get; set; }
        public string Title { get; set; }
        public string FullTitle { get; set; }
        public string Type { get; set; }
        public string Year { get; set; }

        public string TotalRating { get; set; }
        public string TotalRatingVotes { get; set; }

        public List<UserRatingDataDetail> Ratings { get; set; }

        public UserRatingDataDemographic DemographicAll { get; set; }
        public UserRatingDataDemographic DemographicMales { get; set; }
        public UserRatingDataDemographic DemographicFemales { get; set; }

        public UserRatingDataDemographicDetail Top1000Voters { get; set; }
        public UserRatingDataDemographicDetail USUsers { get; set; }
        public UserRatingDataDemographicDetail NonUSUsers { get; set; }

        public string ErrorMessage { get; set; }
    }
}
