using System;
using System.Collections.Generic;
using System.Text;

namespace WebRegex.Core.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Expression> RegexExpressions { get; set; }

        public Profile()
        {
            RegexExpressions = new List<Expression>();
        }
    }
}
