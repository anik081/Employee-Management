using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeInfoSystem.Models
{
    public class ImdbMovieModel
    {
        public string searchType { get; set; }
        public string expression { get; set; }
        public List<ImdbSearchModel> results { get; set; }
        public string errorMessage { get; set; }
    }
}
