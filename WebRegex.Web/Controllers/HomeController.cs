using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebRegex.Core.Models;
using WebRegex.Data;
using WebRegex.Web.Data;
using WebRegex.Web.Models;

namespace WebRegex.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISqlData _data;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, ISqlData data)
        {
            _logger = logger;
            _configuration = configuration;
            _data = data;
            _data.GetConnectionString(_configuration.GetConnectionString("DefaultConnection"));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Profiles()
        {
            var profiles = _data.SqlQuery<Profile>(@"select Id, Name from dbo.Profiles");
            foreach (Profile profile in profiles)
            {
                _data.SqlQuery<Expression>($"select * from dbo.Expressions where ProfileId = {profile.Id}");
            }
            return View(new ProfileViewModel { Profiles = profiles});
        }

        public IActionResult Results()
        {
            var results = _data.SqlQuery<Result>(@"select ProfileId, Name, Regex, Origin, Identifier from dbo.Results");
            var profiles = _data.SqlQuery<Profile>(@"select Id, Name from dbo.Profiles");
            return View(new ResultViewModel(results, profiles));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
