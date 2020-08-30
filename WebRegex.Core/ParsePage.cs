using System.Collections.Generic;
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
        public string Html { get; set; }
        public string Origin {get; set;}

        public ParsePage(string origin)
        {
            Origin = origin;
        }

        public List<Result> GetResults(List<Expression> expressions, string body)
        {
            var results = new List<Result>();
            foreach (var section in expressions)
            {
                try
                {
                    var matches = MatchCollectionToString(new Regex(section.Regex).Matches(body));
                    results.Add(new Result(section.Name, matches, Origin, section.ProfileId));
                }
                catch
                {
                    results.Add(new Result(section.Name, "Invalid Regex", Origin, section.ProfileId));
                }
            }
            return results;
        }
        public List<Result> GetFirstResult(List<Expression> expressions, string body)
        {
            var results = new List<Result>();
            foreach (var section in expressions)
            {
                try
                {
                    var match = new Regex(section.Regex).Match(body).Value;
                    results.Add(new Result(section.Name, match, Origin, section.ProfileId));
                }
                catch
                {
                    results.Add(new Result(section.Name, "Invalid Regex", Origin, section.ProfileId));
                }
            }
            return results;
        }

        public List<Result> AutoGetResults(Profile profile, string url)
        {
            Html = new WebClient().DownloadString(url);
            var results = new List<Result>();
            foreach (var section in profile.RegexExpressions)
            {
                try
                {
                    var matches = MatchCollectionToString(new Regex(section.Regex).Matches(Html));
                    results.Add(new Result(section.Name, matches, Origin, profile.Id));
                }
                catch
                {
                    results.Add(new Result(section.Name, "Invalid Regex", Origin, profile.Id));
                }
            }
            return results;
        }

        public List<Result> AutoGetFirstResult(Profile profile, string url)
        {
            Html = new WebClient().DownloadString(url);
            var results = new List<Result>();
            foreach (var section in profile.RegexExpressions)
            {
                try
                {
                    var match = new Regex(section.Regex).Match(Html).Value;
                    results.Add(new Result(section.Name, match, Origin, profile.Id));
                }
                catch
                {
                    results.Add(new Result(section.Name, "Invalid Regex", Origin, profile.Id));
                }
            }
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
