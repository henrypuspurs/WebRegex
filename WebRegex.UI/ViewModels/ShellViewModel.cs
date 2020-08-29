using Caliburn.Micro;
using System.Linq;
using System.Net;
using System.Windows;
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
        private BindableCollection<WebRegex.Core.Models.Expression> _expressions;

        public ShellViewModel()
        {
            Profiles = new DataHandling().ListToBindableCollection(new SqlData(Helper.CnnVal("WebRegexDB")).GetProfiles());
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

        public BindableCollection<WebRegex.Core.Models.Expression> Expressions
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
                if (SelectedProfile != null)
                {
                    LoadProfile();
                }
                NotifyOfPropertyChange(() => SelectedProfile);
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
            Expressions = new DataHandling().ListToBindableCollection(new SqlData(Helper.CnnVal("WebRegexDB")).GetExpressions(SelectedProfile.Id));
        }

        public void AddExpression()
        {
            Expressions.Add(new WebRegex.Core.Models.Expression() { Name = "New Expression", Regex = "New Regex", ProfileId = SelectedProfile.Id });
        }

        public void RemoveExpression()
        {
            if (Expressions.Count > 0)
            {
                var lastExpression = Expressions.LastOrDefault();
                Expressions.Remove(lastExpression);
            }
        }

        public void LoadPage()
        {
            PageBody = new WebClient().DownloadString(Url);
        }

        public void SaveProfile()
        {
            if (SelectedProfile.Id == 0 && new SqlData(Helper.CnnVal("WebRegexDB")).GetProfiles().All(i => i.Name != SelectedProfile.Name))
            {
                new SqlData(Helper.CnnVal("WebRegexDB")).SaveNewProfile(SelectedProfile, new DataHandling().BindableCollectionToList(Expressions));
                MessageBox.Show("New Profile Saved");
            }
            else
            {
                MessageBox.Show("Profile Saved");
            }
        }

        public void DeleteProfile()
        {
            new SqlData(Helper.CnnVal("WebRegexDB")).DeleteProfile(SelectedProfile);
            Profiles.Remove(SelectedProfile);
            SelectedProfile = null;
        }
    }
}
