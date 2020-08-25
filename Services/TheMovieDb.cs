using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EngineerTest.Services
{
    //this class is used for get API responce
    public class MovieDbResult
    {
        public int page { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
        public MovieDbMovie[] results { get; set; }
    }

    public class MovieDbMovie
    {
        public int vote_count { get; set; }
        public int id { get; set; }
        public bool video { get; set; }
        public double vote_average { get; set; }
        public string title { get; set; }
        public double popularity { get; set; }
        public string poster_path { get; set; }
        public string original_language { get; set; }
        public string original_title { get; set; }
        public int[] genre_ids { get; set; }
        public string backdrop_path { get; set; }
        public bool adult { get; set; }
        public string overview { get; set; }
        public string release_date { get; set; }
        public bool IsTopRated { get; set; }
    }

    public class TheMovieDb
    {

        
        /// <summary>
        /// API key for movie database
        /// </summary>
        private const string API_KEY= "70b1409183a756194225cb434669565b";
        /// <summary>
        /// ACCESS_TOKEN for movie database
        /// </summary>
        private const string ACCESS_TOKEN= "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI3MGIxNDA5MTgzYTc1NjE5NDIyNWNiNDM0NjY5NTY1YiIsInN1YiI6IjVhY2Q5MGQzOTI1MTQxNWE2ZDAwNDE3MCIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.5Md46Myu0Vftls8lk38PI-zB3iPNG89juMcQSsqrY1A";

        

        /// <summary>
        /// Get movie list from API
        /// </summary>
        /// <param name="IsTopRated">True if movie is top rated, false if up coming</param>
        /// <returns></returns>
        public MovieDbResult FetchMovies(bool IsTopRated)
        {
            string strtemp = "upcoming";
            if (IsTopRated)
                strtemp = "top_rated";

            MovieDbResult objCon = new MovieDbResult();

            var client = new RestClient("https://api.themoviedb.org/3/movie/" + strtemp + "?api_key=" + API_KEY + "&language=en-US&page=1");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<MovieDbResult>(response.Content);
            else
                return null;
        }
    }
}
