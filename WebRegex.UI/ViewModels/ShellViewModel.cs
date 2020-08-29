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

        public string ConnectionString
        {
            get
            {
                return Helper.CnnVal("WebRegexDB");
            }
        }

        public ShellViewModel()
        {
            Profiles = new DataHandling().ToBindableCollection(new SqlData(ConnectionString)
                        .LoadData<Profile>(@"select * from dbo.Profiles"));
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
            Results = dataHandling.ToBindableCollection(new ParsePage().GetFirstResult(Expressions.ToList(), PageBody));
        }

        public void LoadProfile()
        {
            Expressions = new DataHandling().ToBindableCollection(new SqlData(Helper.CnnVal("WebRegexDB"))
                        .LoadData<Expression>(@$"select * from dbo.Expressions where ProfileId = {SelectedProfile.Id}"));
        }

        public void AddExpression()
        {
            Expressions.Add(new Expression() { Name = "New Expression", Regex = "New Regex", ProfileId = SelectedProfile.Id });
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
            if (SelectedProfile.Id == 0)
            {
                SaveNewProfile();
                System.Windows.MessageBox.Show("New Profile Saved");
            }
            else
            {
                SaveExistingProfile();
                System.Windows.MessageBox.Show("Profile Saved");
            }
        }

        public void DeleteProfile()
        {
            new SqlData(Helper.CnnVal("WebRegexDB")).SaveData(@"delete from dbo.Profiles where Id = @Id; delete from dbo.Expressions where ProfileId = @Id", SelectedProfile);
            Profiles.Remove(SelectedProfile);
            SelectedProfile = null;
        }

        private void SaveNewProfile()
        {
            var sqlData = new SqlData(Helper.CnnVal("WebRegexDB"));
            sqlData.SaveData(@"insert into dbo.Profiles values (@Name)", SelectedProfile);
            var Id = sqlData.LoadData<Profile>(@$"select Id from dbo.Profiles where Name = '{SelectedProfile.Name}'").First().Id;
            foreach (Expression expression in Expressions)
            {
                sqlData.SaveData(@$"insert into dbo.Expressions (ProfileId, Name, Regex) values ('{Id}', @Name, @Regex)", expression);
            }
        }

        private void SaveExistingProfile()
        {
            var sqlData = new SqlData(Helper.CnnVal("WebRegexDB"));
            sqlData.SaveData($"update dbo.Profiles set Name = @Name where Id = @Id", SelectedProfile);
            foreach (Expression expression in Expressions)
            {
                if (expression.Id != 0)
                {
                    sqlData.SaveData($"update dbo.Expressions set Name = @Name where Id = @Id", expression);
                    sqlData.SaveData($"update dbo.Expressions set Regex = @Regex where Id = @Id", expression);
                }
                else
                {
                    sqlData.SaveData(@$"insert into dbo.Expressions (ProfileId, Name, Regex) values ('{SelectedProfile.Id}', @Name, @Regex)", expression);
                }
            }
            foreach (Expression savedexpression in sqlData.LoadData<Expression>(@$"select * from dbo.Expressions where ProfileId = '{SelectedProfile.Id}'"))
            {
                if (Expressions.All(n => n.Name != savedexpression.Name))
                {
                    sqlData.SaveData($"delete from dbo.Expressions where Id = @Id", savedexpression);
                }
            }
        }
    }
}
