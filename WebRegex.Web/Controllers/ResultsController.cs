using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebRegex.Web.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebRegex.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private ApplicationDbContext _context { get; }

        public ResultsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<Results>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<Results>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<Results>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<Results>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Results>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
