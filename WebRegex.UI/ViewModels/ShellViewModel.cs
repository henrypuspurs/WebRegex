using Caliburn.Micro;
using System.Linq;
using System.Net;
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
        private BindableCollection<Result> _results;
        private BindableCollection<Expression> _expressions;

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

        public BindableCollection<Expression> Expressions
        {
            get 
            {
                return _expressions;
            }
            set
            {
                _expressions = value;
                NotifyOfPropertyChange(() => Expressions);
            }
        }

        public BindableCollection<Result> Results
        {
            get
            {
                return _results;
            }
            set
            {
                _results = value;
                NotifyOfPropertyChange(() => Results);
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
            var dataHandling = new DataHandling();
            Results = dataHandling.ListToBindableCollection(new ParsePage().GetFirstResult(dataHandling.BindableCollectionToList(Expressions), PageBody));
        }

        public void LoadProfile()
        {
            Expressions = new DataHandling().ListToBindableCollection(new ProfileSQLData().GetExpressions(SelectedProfile.Id, Helper.CnnVal("WebRegexDB")));
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
            PageBody = new WebClient().DownloadString(Url);
        }
    }
}
