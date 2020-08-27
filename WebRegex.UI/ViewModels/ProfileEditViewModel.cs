using Caliburn.Micro;
using System.Linq;
using WebRegex.Core.Models;
using WebRegex.Data;
using WebRegex.UI.Core;

namespace WebRegex.UI.ViewModels
{
    class ProfileEditViewModel : Screen
    {
        public ProfileEditViewModel(Profile profile)
        {
            profile.RegexExpressions = new DataHandling().ListToBindableCollection(new ProfileSQLData().GetExpressions(profile.Id, Helper.CnnVal("WebRegexDB"))).ToList();
            Profile = profile;
        }

        public Profile Profile;

        public BindableCollection<Expression> Expressions
        {
            get
            {
                return new DataHandling().ListToBindableCollection(Profile.RegexExpressions);
            }
        }
    }
}
