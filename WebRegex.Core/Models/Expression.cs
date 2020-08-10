using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebRegex.Core.Models
{
    public class Expression : IExpression
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public string Name { get; set; }
        public string Regex { get; set; }
    }
}
