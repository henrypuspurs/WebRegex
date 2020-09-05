using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using WebRegex.Core.Models;

namespace WebRegex.Core
{
    public class ParsePage
    {
        public string PageUrl { get; private set; }
        public string Origin { get; set; }
        public bool IdentifierFound { get; set; }
        public string IdentifierValue { get; set; }

        public ParsePage(string origin)
        {
            Origin = origin;
        }

        public List<Result> GetFirstResult(Profile profile, string body)
        {
            var results = new List<Result>();
            foreach (Expression expression in profile.RegexExpressions)
            {
                try
                {
                    var match = new Regex(expression.Regex).Match(body).Value;
                    AddResults(results, expression, profile, match);
                }
                catch
                {
                    AddResults(results, expression, profile, "Invalid Regex");
                }
            }
            return results;
        }

        public List<Result> GetResults(Profile profile, string body)
        {
            var results = new List<Result>();
            foreach (Expression expression in profile.RegexExpressions)
            {
                try
                {
                    var matches = MatchCollectionToString(new Regex(expression.Regex).Matches(body));
                    AddResults(results, expression, profile, matches);
                }
                catch
                {
                    AddResults(results, expression, profile, "Invalid Regex");
                }
            }
            return results;
        }

        public List<Result> AutoGetResults(Profile profile, string url)
        {
            var html = new WebClient().DownloadString(url);
            return GetResults(profile, html);
        }

        public List<Result> AutoGetFirstResult(Profile profile, string url)
        {
            var html = new WebClient().DownloadString(url);
            return GetFirstResult(profile, html);
        }

        private List<Result> AddResults(List<Result> results, Expression expression, Profile profile, string regex)
        {
            if (expression.IsIdentifier == true)
            {
                IdentifierValue = regex;
                foreach (Result result in results)
                {
                    result.Identifier = IdentifierValue;
                }
            }
            results.Add(new Result { Id = expression.Id, ProfileId = profile.Id, Origin = Origin, Regex = regex, IsIdentifier = expression.IsIdentifier, Identifier = IdentifierValue });
            return results;
        }

        private string MatchCollectionToString(MatchCollection matches)
        {
            List<string> matchList = new List<string>();
            foreach (Match match in matches)
            {
                matchList.Add(match.Value);
            }
            return string.Join("\n", matchList);
        }
    }
}
