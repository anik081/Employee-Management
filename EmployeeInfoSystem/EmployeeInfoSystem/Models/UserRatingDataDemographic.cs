using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeInfoSystem.Models
{
    public class UserRatingDataDemographic
    {
        public UserRatingDataDemographicDetail AllAges { get; set; }
        public UserRatingDataDemographicDetail AgesUnder18 { get; set; }
        public UserRatingDataDemographicDetail Ages18To29 { get; set; }
        public UserRatingDataDemographicDetail Ages30To44 { get; set; }
        public UserRatingDataDemographicDetail AgesOver45 { get; set; }
    }
}
