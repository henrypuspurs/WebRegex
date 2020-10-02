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
        public string Origin { get; set; }
        public bool IsIdentifier { get; set; }
        private string _identifier;
        private string _regex;

        public string Identifier
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_identifier))
                {
                    return Origin;
                }
                else
                {
                    return _identifier;
                }
            }
            set
            {
                _identifier = value;
            }
        }

        public string Regex 
        {
            get
            {
                if (IsIdentifier == false)
                {
                    return _regex;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(_regex) || _regex == "Invalid Regex")
                    {
                        return Origin;
                    }
                    else
                    {
                        return _regex;
                    }
                }
            }
            set
            {
                _regex = value;
            }
        }

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
