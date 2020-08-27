using Caliburn.Micro;
using System.Linq;
using WebRegex.Core;
using WebRegex.Core.Models;
using WebRegex.Data;
using WebRegex.UI.Core;

namespace WebRegex.UI.ViewModels
{
    class ShellViewModel : Conductor<object>
    {
        private BindableCollection<Profile> _profiles;
        private Profile _selectedProfile;
        private string _pageBody;

        public ShellViewModel()
        {
            Profiles = new DataHandling().ListToBindableCollection(new ProfileSQLData().GetProfiles(Helper.CnnVal("WebRegexDB")));
            PageBody = "Load HTML to be run";
        }

        public string Url { get; set; }
        public string PageBody
        {
            get
            {
                return _pageBody;
            }
            set
            {
                _pageBody = value;
                NotifyOfPropertyChange(() => PageBody);
            }
        }

        public Result Result
        {
            get
            {
                return Result;
            }
            set
            {
                Result = value;
                NotifyOfPropertyChange(() => Result);
            }
        }


        public Profile SelectedProfile
        {
            get
            {
                return _selectedProfile;
            }
            set
            {
                _selectedProfile = value;
                NotifyOfPropertyChange(() => SelectedProfile);
                if (SelectedProfile != null)
                {
                    LoadProfile();
                }
            }
        }


        public BindableCollection<Profile> Profiles
        {
            get
            {
                return _profiles;
            }
            set
            {
                _profiles = value;
                NotifyOfPropertyChange(() => Profiles);
            }
        }

        public void AddProfile()
        {
            var newProfile = new Profile() { Name = "New Profile" };
            Profiles.Add(newProfile);
            SelectedProfile = newProfile;
        }

        public void LoadResults()
        {
            var results = new DataHandling().ListToBindableCollection(new ParsePage(SelectedProfile, Url).ReturnResults());
            ActivateItemAsync(new ResultViewModel(results));
        }

        public void LoadProfile()
        {
            ActivateItemAsync(new ProfileEditViewModel(SelectedProfile));
        }

        public void AddExpression()
        {
            SelectedProfile.RegexExpressions.Add(new Expression() { Name = "New Expression", Regex = "New Regex", ProfileId = SelectedProfile.Id });
        }

        public void RemoveExpression()
        {
            if (SelectedProfile.RegexExpressions.Count > 0)
            {
                var lastExpression = SelectedProfile.RegexExpressions.Last();
                SelectedProfile.RegexExpressions.Remove(lastExpression);
            }
        }

        public void LoadPage()
        {
            var page = new ParsePage(SelectedProfile, Url);
            PageBody = page.Html;
        }
    }
}
