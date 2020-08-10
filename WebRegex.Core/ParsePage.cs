using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using WebRegex.Core.Models;

namespace WebRegex.Core
{
    public class ParsePage
    {
        public string PageUrl { get; private set; }
        public Profile Profile { get; private set; }
        public string Html { get; set; }

        public ParsePage(Profile profile, string pageUrl)
        {
            PageUrl = pageUrl;
            Profile = profile;
            Html = new WebClient().DownloadString(PageUrl);
        }

        public List<Result> ReturnResults()
        {
            var results = new List<Result>();

            foreach (var section in Profile.RegexExpressions)
            {
                var matches = MatchCollectionToString(new Regex(section.Regex).Matches(Html));
                results.Add(new Result(section.Name, matches) { ProfileId = Profile.Id});
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
