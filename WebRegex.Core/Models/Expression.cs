using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebRegex.Core.Models
{
    public class Expression : IExpression
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProfileId { get; set; }
        public string Regex { get; set; }
        public bool IsIdentifier { get; set; } = false;
        public int IsIdentifierBit
        {
            get
            {
                if (IsIdentifier == false)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            set
            { 
                if (value == 0)
                {
                    IsIdentifier = false;
                }
                else
                {
                    IsIdentifier = true;
                }
            }
        }
    }
}
