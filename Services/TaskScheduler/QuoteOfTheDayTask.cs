using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EngineerTest.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EngineerTest.Services
{
    // uses https://theysaidso.com/api/
    public class MovieFetchTask : IScheduledTask
    {
        public string Schedule => "* */6 * * *";
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Get context
        /// </summary>
        /// <param name="context">Database information</param>
        public MovieFetchTask(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Execute automatically  and get movie list from api and save in database
        /// </summary>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns></returns>
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            MovieService movieService = new MovieService(_context);
            movieService.PopulateMovies(true);
            movieService.PopulateMovies(false);
        }
    }
    
}