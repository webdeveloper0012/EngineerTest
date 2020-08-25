using EngineerTest.Models;
using EngineerTest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace EngineerTest.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginViewModel> _logger;        
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// initialize controller
        /// </summary>
        /// <param name="userManager">userManager</param>
        /// <param name="signInManager">signInManager</param>
        /// <param name="logger">logger</param>
        /// <param name="context">context</param>
        public MovieController(
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager, ILogger<LoginViewModel> logger, ApplicationDbContext context
           )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }
        /// <summary>
        /// Get list of movie and display it to user
        /// </summary>
        /// <returns>List of movie</returns>
        public IActionResult Index()
        {
            MovieService movieService = new MovieService(_context);
            var movieList = movieService.GetAll();
            if (movieList == null || movieList.Count == 0)
            {
                movieService.PopulateMovies(true);
                movieService.PopulateMovies(false);
                movieList = movieService.GetAll();
            }
            return View(movieList);
        }

    }
}