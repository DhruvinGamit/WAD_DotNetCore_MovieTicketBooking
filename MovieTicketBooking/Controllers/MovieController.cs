/*using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieTicketBooking.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieTicketBooking.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepo;

        public MovieController(IMovieRepository movieRepo)
        {
            this._movieRepo = movieRepo;
        }

        public ViewResult Index(string type)
        {
            var model = new List<Movie>();
            var movies = _movieRepo.GetAllMovies();
            if (type == "Realeased" || string.IsNullOrEmpty(type))
            {
                foreach(var movie in movies)
                {
                    if(DateTime.Compare(DateTime.Parse(movie.ReleaseDate), DateTime.Now.Date) <= 0)
                    {
                        model.Add(movie);
                    }
                }
                ViewBag.Type = "NotRealeased";
            }
            else
            {
                foreach (var movie in movies)
                {
                    if (DateTime.Compare(DateTime.Parse(movie.ReleaseDate), DateTime.Now.Date) >= 0)
                    {
                        model.Add(movie);
                    }
                }
                ViewBag.Type = "Realeased";
            }
            return View(model);
        }

        public async Task<ViewResult> Details(int id)
        {
            Movie model = _movieRepo.GetMovie(id);

            dynamic movieDetails = await FetchMovieDetails(model.Title,DateTime.Parse(model.ReleaseDate).ToString("yyyy"));

            ViewBag.Title = movieDetails["Title"];
            ViewBag.Year = movieDetails["Year"];
            ViewBag.Runtime = movieDetails["Runtime"];

            string[] languages = movieDetails["Language"].ToString().Split(',');
            ViewBag.Languages = languages;
            ViewBag.Plot = movieDetails["Plot"];

            var movies = _movieRepo.GetAllMovies();
            var similarMovies = new List<Movie>();

            ViewBag.SimilarMovies = similarMovies;
            return View(model);
        }

        private async Task<dynamic> FetchMovieDetails(string title, string releaseYear)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://www.omdbapi.com/?apikey=56b876a&t=" + title + "&y=" + releaseYear))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<dynamic>(apiResponse);
                }
            }
        }
    }
}
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieTicketBooking.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieTicketBooking.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepo;

        public MovieController(IMovieRepository movieRepo)
        {
            this._movieRepo = movieRepo;
        }

        public ViewResult Index(string type)
        {
            var model = new List<Movie>();
            var movies = _movieRepo.GetAllMovies();
            if (type == "Realeased" || string.IsNullOrEmpty(type))
            {
                foreach (var movie in movies)
                {
                    if (DateTime.Compare(DateTime.Parse(movie.ReleaseDate), DateTime.Now.Date) <= 0)
                    {
                        model.Add(movie);
                    }
                }
                ViewBag.Type = "NotRealeased";
            }
            else
            {
                foreach (var movie in movies)
                {
                    if (DateTime.Compare(DateTime.Parse(movie.ReleaseDate), DateTime.Now.Date) >= 0)
                    {
                        model.Add(movie);
                    }
                }
                ViewBag.Type = "Realeased";
            }
            return View(model);
        }

        public async Task<ViewResult> Details(int id)
        {
            Movie model = _movieRepo.GetMovie(id);

            dynamic movieDetails = await FetchMovieDetails(model.Title, DateTime.Parse(model.ReleaseDate).ToString("yyyy"));

            ViewBag.Title = movieDetails["Title"];
            ViewBag.Year = movieDetails["Year"];
            // ViewBag.Runtime = movieDetails["Runtime"];
            ViewBag.Released = movieDetails["Released"];
            ViewBag.Genre = movieDetails["Genre"];

            string[] languages = movieDetails["Language"].ToString().Split(',');
            ViewBag.Languages = languages;

            ViewBag.Plot = movieDetails["Plot"];

            var movies = _movieRepo.GetAllMovies();
            char[] c = { ',', ' ' };
            var genres = model.Genre.Split(c);
            var similarMovies = new List<Movie>();

            foreach (var movie in movies)
            {
                if (genres.Intersect<string>(movie.Genre.Split(c, StringSplitOptions.RemoveEmptyEntries)).Count() >= 2 && model.Id != movie.Id)
                {
                    similarMovies.Add(movie);
                }
            }
            ViewBag.SimilarMovies = similarMovies;
            return View(model);
        }

        private async Task<dynamic> FetchMovieDetails(string title, string releaseYear)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://www.omdbapi.com/?apikey=56b876a&t=" + title + "&y=" + releaseYear))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<dynamic>(apiResponse);
                }
            }
        }
    }
}
