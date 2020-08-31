using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebRegex.Core.Models;

namespace WebRegex.Web.Models
{
    public class ResultViewModel
    {
        public List<Result> Results;
        public List<Profile> Profiles;

        public ResultViewModel(List<Result> results, List<Profile> profiles)
        {
            Results = results;
            Profiles = profiles;
        }
    }
}
