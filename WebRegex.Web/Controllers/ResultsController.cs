using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebRegex.Core.Models;
using WebRegex.Data;
using WebRegex.Web.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebRegex.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ISqlData _data;

        public ResultsController(IConfiguration configuration, ISqlData data)
        {
            _configuration = configuration;
            _data = data;
            _data.GetConnectionString(_configuration.GetConnectionString("DefaultConnection"));
        }

        // GET: api/<Results>
        [HttpGet]
        public IEnumerable<Result> Get()
        {
            return _data.SqlQuery<Result>($"Select * from dbo.Results");
        }

        // GET api/<Results>/5
        [HttpGet("{id}")]
        public IEnumerable<Result> Get(int id)
        {
            return _data.SqlQuery<Result>($"Select * from dbo.Results where profileId = {id}");
        }

        // POST api/<Results>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
    }
}
