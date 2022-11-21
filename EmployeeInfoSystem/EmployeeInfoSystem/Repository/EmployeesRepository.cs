using EmployeeInfoSystem.Data;
using EmployeeInfoSystem.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace EmployeeInfoSystem.Repository
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly EmployeeContext _context;
        private readonly IConfiguration _configuration;

        public EmployeesRepository(EmployeeContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public List<EmployeeModel> employeeList = new List<EmployeeModel>();

        // Get all employees
        public async Task<List<EmployeeModel>> GetAllEmployee()
        {
            var records = await _context.Employee.Select(x => new EmployeeModel()
            {
                Id = x.Id,
                Name = x.Name,
                DOB = x.DOB,
                Sex = x.Sex,
                MovieName = x.MovieName,
                MovieRating = x.MovieRating

            }).ToListAsync();

            return records;
        }

        //Get Employee By Id
        public async Task<EmployeeModel> GetEmployeesById(int id)
        {
            var records = await _context.Employee.Where(x=> x.Id == id).Select(x => new EmployeeModel()
            {
                Id = x.Id,
                Name = x.Name,
                DOB = x.DOB,
                Sex = x.Sex,
                MovieName = x.MovieName,
                MovieRating = x.MovieRating

            }).FirstOrDefaultAsync();

            return records;
        }
        //Find employee by movie
        public async Task<List<EmployeeModel>> GetEmployeesByMovie(string movieName)
        {


            var records = await _context.Employee.Where(x => x.MovieName == movieName).Select(x => new EmployeeModel()
            {
                Id = x.Id,
                Name = x.Name,
                DOB = x.DOB,
                Sex = x.Sex,
                MovieName = x.MovieName,
                MovieRating = x.MovieRating

            }).ToListAsync();

            return records;
        }
        //Add new Employee
        public async Task<int> AddEmployee(EmployeeModel employeeModel)
        {
            //Get IMDB movie rating
            string movieRating = GetRating(employeeModel.MovieName);
            var employee = new Employee()
            {
                Id = employeeModel.Id,
                Name = employeeModel.Name,
                DOB = employeeModel.DOB,
                Sex = employeeModel.Sex,
                MovieName = employeeModel.MovieName,
                MovieRating = movieRating
            };
            _context.Employee.Add(employee);
            await _context.SaveChangesAsync();
            return employee.Id;
        }

        //Update employee using id
        public async Task<bool> UpdateEmployeeById(int id, EmployeeModel employeeModel)
        {
            string movieRating = GetRating(employeeModel.MovieName);
            var employee = new Employee()
            {
                Id = employeeModel.Id,
                Name = employeeModel.Name,
                DOB = employeeModel.DOB,
                Sex = employeeModel.Sex,
                MovieName = employeeModel.MovieName,
                MovieRating = movieRating
            };
            _context.Employee.Update(employee);
            await _context.SaveChangesAsync();
            return true;
        }
        //Delete employee using id
        public async Task<bool> DeleteEmployeeById(int id)
        {
            var employee = new Employee()
            {
                Id = id
            };

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }

        #region user defined methods
        //call imdb api to fetch movie id
        public ImdbMovieModel GetImdbId(string movieName)
        {
            var responseString = "";
            string url = _configuration["IMDBUrls:GetIMDBId"];
            //var request = (HttpWebRequest)WebRequest.Create("https://imdb-api.com/en/API/Search/k_e9u7xu69/" + movieName);
            var request = (HttpWebRequest)WebRequest.Create(url + movieName);
            request.Method = "GET";
            request.ContentType = "application/json";

            using (var response1 = request.GetResponse())
            {
                using (var reader = new StreamReader(response1.GetResponseStream()))
                {
                    responseString = reader.ReadToEnd();
                }
            }
            ImdbMovieModel model = JsonConvert.DeserializeObject<ImdbMovieModel>(responseString);        
            return model;
        }

        //Fetch movie rating from IMDB using movie name
        public string GetRating(string movieName)
        {
            string rating = "-1";
            ImdbMovieModel movieIdModel = new ImdbMovieModel();
            RatingModel ratingModel = new RatingModel();
            //Get movie id from imdb
            movieIdModel = GetImdbId(movieName);

            if (string.IsNullOrEmpty(movieIdModel.errorMessage))
            {
                //Get Movie Rating
                if (movieIdModel.results.Count > 0)
                {
                    string movieId = movieIdModel.results[0].Id;
                    ratingModel = GetMovieRatingById(movieId);
                    if (string.IsNullOrEmpty(ratingModel.ErrorMessage))
                    {
                        rating = ratingModel.TotalRating;
                    }
                }
             
            }

            return rating;
        }
        //Fetch movie rating from IMDB using movie id
        public RatingModel GetMovieRatingById(string id)
        {
            var responseString = "";
            string url = _configuration["IMDBUrls:GetIMDBRating"];
            //var request = (HttpWebRequest)WebRequest.Create("https://imdb-api.com/en/API/UserRatings/k_e9u7xu69/" + id);
            var request = (HttpWebRequest)WebRequest.Create(url + id);
            request.Method = "GET";
            request.ContentType = "application/json";

            using (var response1 = request.GetResponse())
            {
                using (var reader = new StreamReader(response1.GetResponseStream()))
                {
                    responseString = reader.ReadToEnd();
                }
            }
            RatingModel model = JsonConvert.DeserializeObject<RatingModel>(responseString);
            return model;

        }
        #endregion

    }
}
