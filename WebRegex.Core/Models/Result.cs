using System;
using System.Collections.Generic;
using System.Text;

namespace WebRegex.Core.Models
{
    public class Result : IExpression
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProfileId { get; set; }
        public string Regex { get; set; }
        public string Origin { get; set; }

        public Result(string name, string regex, string origin, int profileId)
        {
            Name = name;
            Regex = regex;
            ProfileId = profileId;
            Origin = origin;
        }
    }
}
